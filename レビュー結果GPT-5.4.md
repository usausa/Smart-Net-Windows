# Smart-Net-Windows レビュー結果

## 対象
- `Smart.Windows`
- `Smart.Windows.Extensions`
- `Smart.Windows.Hosting`
- `Smart.Windows.Tests`

## 実施内容
- 重点領域の静的レビュー
- 既存テストの確認
- テスト実行結果の確認

## 総評
全体として、コンバーター・式評価・コマンドの基本機能には広めのテストがあります。一方で、WPF のビヘイビア、トリガー、メッセンジャー、ホスティングのようなライフサイクル依存の部品はテストが薄く、実運用でバグが出やすい箇所が未検証のまま残っています。

特に、`ExecuteCommandAction` の依存関係プロパティ型不整合、`CallMethodAction` の引数選択ロジック、再アタッチ時の再購読漏れは、実害が出やすい不具合候補です。パフォーマンス面では、`ReactiveMessenger` の静的 `Subject<T>` 共有や、反射呼び出しの頻度が気になりました。

## テスト状況
- `Smart.Windows.Tests` の 151 件は実行成功
- テストは主に以下へ集中
  - コンバーター
  - 式評価
  - `DelegateCommand` / `AsyncCommand`
- 直接テストが見当たらない主要領域
  - `Smart.Windows/Windows/Interactivity/ExecuteCommandAction.cs`
  - `Smart.Windows/Windows/Interactivity/CallMethodAction.cs`
  - `Smart.Windows.Extensions/Windows/Interactivity/MessageTrigger.cs`
  - `Smart.Windows.Extensions/Windows/Interactivity/RequestTriggerBase.cs`
  - `Smart.Windows.Extensions/Windows/Messaging/ReactiveMessenger.cs`
  - `Smart.Windows/Windows/Interactivity/WindowPlacementBehavior.cs`
  - `Smart.Windows.Hosting/Windows/Hosting/ApplicationHostingService.cs`

---

## 重要度: 高

### 1. `ExecuteCommandAction` の依存関係プロパティ型が不正
**対象:** `Smart.Windows/Windows/Interactivity/ExecuteCommandAction.cs:12-20`

`CommandProperty` と `CommandParameterProperty` がどちらも `IValueConverter` 型で登録されています。

- `CommandProperty` は `ICommand` であるべきです
- `CommandParameterProperty` は `object` であるべきです

### リスク
- XAML から通常の `ICommand` を設定した時点で型不一致になる可能性があります
- 実行時バインディング失敗や `SetValue` 例外の原因になります
- コマンド動作不良が UI 側で断続的に見える形になりやすいです

### 推奨
- 依存関係プロパティ登録型を CLR プロパティと一致させる
- `Command` と `CommandParameter` の基本動作テストを追加する

---

### 2. `CallMethodAction` でイベント引数やコンバーター結果が実質使われない
**対象:** `Smart.Windows/Windows/Interactivity/CallMethodAction.cs:98-104`

引数選択が以下の条件になっています。

- `methodParameter is not null`
- `this.IsSet(MethodNameProperty)`

`MethodNameProperty` は通常ほぼ必ず設定されるため、この条件はほぼ常に真になります。結果として `MethodParameter` が未設定でも `Converter` や `parameter` へフォールバックせず、`null` をそのまま渡しやすい実装です。

### リスク
- イベント引数をメソッドへ渡したいシナリオで期待通りに動かない
- コンバーター指定が無視され、XAML 設定と実動作がずれる
- `null` 引数での反射呼び出しにより、対象メソッド側で例外や誤動作が起こり得る

### 推奨
- 判定対象は `MethodNameProperty` ではなく `MethodParameterProperty` にする
- 以下のテストを追加する
  - `MethodParameter` 未設定時にイベント引数が渡ること
  - `Converter` 指定時に変換結果が渡ること
  - 0 引数メソッドと 1 引数メソッドの両方

---

### 3. `MessageTrigger` / `RequestTriggerBase` が `Unloaded` 後に再購読しない
**対象:**
- `Smart.Windows.Extensions/Windows/Interactivity/MessageTrigger.cs:28-47`
- `Smart.Windows.Extensions/Windows/Interactivity/RequestTriggerBase.cs:25-45`

どちらも `Unloaded` でイベント購読解除していますが、`Loaded` 時の再購読処理がありません。WPF ではビューの再利用やタブ切替、テンプレート再適用で `Unloaded`/`Loaded` が複数回発生します。

### リスク
- 一度非表示や切り離しが起きた後、トリガーが二度と反応しなくなる
- 画面遷移後だけ再現する不安定な不具合になりやすい
- UI バグとして発見が遅れやすい

### 推奨
- `Loaded` 時に現在の `Messenger` / `Request` へ再購読する
- または `OnAttached` / `OnDetaching` のみで寿命管理できる設計へ整理する
- ビジュアルツリー再アタッチを想定したテストを追加する

---

### 4. `WindowPlacementBehavior` は初回配置タイミングに失敗しやすい
**対象:**
- `Smart.Windows/Windows/Interactivity/WindowPlacementBehavior.cs:34-40`
- `Smart.Windows/Windows/Interactivity/WindowPlacementHelper.cs:7-24`

配置計算は `OnAttached()` で一度だけ行われますが、その時点では `ActualWidth` / `ActualHeight` がまだ 0 または `NaN` のことがあります。`WindowPlacementHelper.UpdatePlacement()` はその場合 `false` を返すだけで、再試行がありません。

### リスク
- 画面サイズ未確定のタイミングで配置がスキップされる
- ウィンドウ初期位置が不定になる
- DPI や起動タイミング依存の再現しづらい UI バグになる

### 推奨
- `Loaded` または `ContentRendered` 後に再配置する
- `UpdatePlacement()` が失敗した場合の再試行方針を持つ
- 位置計算のユニットテストに加え、ロード後適用の結合テストを追加する

---

## 重要度: 中

### 5. `AsyncCommand` が `async void` で例外と再入に弱い
**対象:** `Smart.Windows/Windows/Input/AsyncCommand.cs:41-45, 88-91`

`ICommand.Execute` の制約上 `async void` になっていますが、現状は以下の保護がありません。

- 実行中フラグによる再入防止
- 例外の観測/通知
- キャンセル連携

さらに既存テストでは、`CanExecute == false` でも `Execute()` は普通に実行される現状が期待値として固定されています。

**参考:** `Smart.Windows.Tests/Windows/Input/AsyncCommandTest.cs:40-58, 146-164`

### リスク
- ダブルクリックで多重実行しやすい
- 非同期例外が UI スレッドへ伝播しアプリ停止要因になる
- 呼び出し側が `CanExecute` を信用しても、防御がコマンド内にない

### 推奨
- 実行中の再入制御を入れる
- 少なくとも `Execute` 内で `CanExecute` を再確認する
- 例外ハンドリング方針を明確化する
- テストへ以下を追加する
  - 実行中の二重起動防止
  - 例外発生時の通知/扱い
  - `CanExecute == false` 時の `Execute` 挙動

---

### 6. `ReactiveMessenger` が全インスタンスで静的 `Subject<T>` を共有している
**対象:** `Smart.Windows.Extensions/Windows/Messaging/ReactiveMessenger.cs:10-35`

`SubjectHolder<T>.Subject` が static のため、`ReactiveMessenger` のインスタンスを分けても、メッセージ経路は実質グローバル共有です。

### リスク
- テストや複数ホスト間でメッセージが漏れる
- インスタンス分離の期待に反する
- `Subject<T>` は並列 `OnNext` に強くないため、送受信が並行化すると競合し得る
- `HasObservers` が別インスタンスの購読も見てしまう

### パフォーマンス観点
- static のため型ごとの `Subject<T>` がプロセス寿命で残りやすい
- 長寿命購読が積み上がると不要な配信コストやメモリ保持につながります

### 推奨
- インスタンス単位でストリームを持つ設計へ変更する
- 必要ならスレッド安全なラッパーを使う
- 送受信の並行ケースとインスタンス分離テストを追加する

---

### 7. `ApplicationHostingService` が停止要求と例外を適切に扱っていない
**対象:** `Smart.Windows.Hosting/Windows/Hosting/ApplicationHostingService.cs:24-35`

別 STA スレッドで `app.Run()` を起動していますが、`stoppingToken` を見ていません。また、`app.Run()` または `GetRequiredService<TApp>()` で例外が起きると `tcs.SetResult()` も `StopApplication()` も呼ばれず、ホスト状態が不整合になる可能性があります。

### リスク
- ホスト停止時に WPF アプリ終了との同期が取れない
- 起動例外で `BackgroundService` が完了せず、停止処理が宙に浮く
- 障害時の診断が難しい

### 推奨
- `TaskCompletionSource` は成功/失敗/キャンセルを分けて完了させる
- `StopApplication()` は `finally` で扱う
- 停止要求時に `Application.Current.Dispatcher` 経由で終了させる流れを検討する
- 起動失敗ケースのテストを追加する

---

### 8. `ConvertHelper` が例外を握りつぶしており、不正入力を見逃しやすい
**対象:** `Smart.Windows/Windows/Expressions/ConvertHelper.cs:26-34`

`System.Convert.ChangeType()` の失敗を `catch (Exception)` で丸ごと吸収し、`null` を返しています。

### リスク
- 型変換失敗、カルチャ差異、オーバーフローなどが見えなくなる
- 呼び出し側で「比較結果が false だった」ように見えて、本来の入力不正が埋もれる
- 障害解析が難しい

### 推奨
- 捕捉例外を必要最小限に絞る
- 失敗時の扱いを API として明文化する
- 少なくとも失敗パターンのテストを追加する
  - 数値オーバーフロー
  - 不正フォーマット
  - `CurrentCulture` と `InvariantCulture` の差分

---

## 重要度: 低

### 9. 反射呼び出し系アクションはホットパス化するとコストが高い
**対象:**
- `Smart.Windows/Windows/Interactivity/ClipboardSetDataAction.cs:46-53`
- `Smart.Windows/Windows/Interactivity/CallMethodAction.cs:82-108`
- `Smart.Windows.Extensions/Windows/Interactivity/ResolveMethodAction.cs:51-64`
- `Smart.Windows.Extensions/Windows/Interactivity/ResolvePropertyAction.cs:50-62`

`CallMethodAction` / `ResolveMethodAction` / `ResolvePropertyAction` は最低限のキャッシュがありますが、`ClipboardSetDataAction` は毎回 `GetMethod()` を呼びます。頻度が高い UI 操作では小さな積み重ねになります。

### 推奨
- 頻発イベントで使う前提ならメソッド情報をキャッシュする
- パラメーターシグネチャを含めてキャッシュ条件を厳密化する
- 可能なら反射を避けた専用アクションへ寄せる

---

### 10. `ScrollIntoOnChangedBehavior` は `ItemsSource` 差し替えに追従しない
**対象:** `Smart.Windows/Windows/Interactivity/ScrollIntoOnChangedBehavior.cs:48-83`

`Loaded` 時点の `ItemsSource` にだけ購読しています。ロード後に `ItemsSource` が差し替わった場合、新しいコレクションを監視しません。

### リスク
- 画面初期表示後にデータソースを差し替える画面でだけ動かない
- 逆に古いコレクションへ購読が残る余地がある

### 推奨
- `ItemsSource` 変更監視を追加する
- 再バインドを含む UI テストを追加する

---

## 良い点
- コンバーターと式評価にテストが多く、基本仕様の保護ができています
- `CallMethodAction` / `ResolveMethodAction` などは最低限の反射キャッシュを持っています
- `DelegateCommand` / `AsyncCommand` の基本振る舞いを xUnit で明文化できています

## 優先対応順
1. `ExecuteCommandAction` の依存関係プロパティ型修正
2. `CallMethodAction` の `IsSet` 判定修正
3. `MessageTrigger` / `RequestTriggerBase` の再購読対応
4. `WindowPlacementBehavior` のロード後再配置
5. `AsyncCommand` の再入・例外設計見直し
6. `ReactiveMessenger` のインスタンス分離と並行性見直し
7. `ApplicationHostingService` の終了/例外処理強化
8. 未テスト領域への回帰テスト追加

## 追加するとよいテスト
- `ExecuteCommandAction`
  - `Command` 実行
  - `CommandParameter` 優先
  - `Converter` フォールバック
- `CallMethodAction`
  - 0 引数/1 引数メソッド
  - `MethodParameter` 未指定時のイベント引数伝播
  - `Converter` 使用時の引数変換
- `MessageTrigger` / `RequestTriggerBase`
  - `Unloaded` → `Loaded` 後も再度反応すること
- `WindowPlacementBehavior`
  - 初回 `OnAttached` でサイズ未確定でもロード後に配置されること
- `ReactiveMessenger`
  - インスタンス分離
  - 複数購読者
  - 並行送信
- `ApplicationHostingService`
  - 起動例外
  - 正常終了
  - 停止要求時の終了連携

## 補足
今回のレビューは、実装変更ではなく静的レビューと既存テスト確認を中心に実施しました。GUI ライフサイクルや Dispatcher を伴う箇所は、静的に読める範囲でも不具合の兆候が明確でした。特に `Interactivity` と `Hosting` は、現状のテスト密度に対して実行時依存が強いため、回帰テストの追加効果が大きいです。
