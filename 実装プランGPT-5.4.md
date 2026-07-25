# Smart.Windows レビュー対応 実装プラン

- 対象レビュー: `レビュー結果GPT-5.4.md`
- 対象プロジェクト: `Smart.Windows` / `Smart.Windows.Extensions` / `Smart.Windows.Hosting`（参考: `Smart.Windows.Tests`）
- 作成日: 2026-07-02

## 進め方
- **影響が小さく修正量の少ない項目を高優先**で並べています。
- 各項目は**着手前に対応要否を確認**します（全項目を対応するとは限りません）。
- **完了した項目は本ドキュメントから削除**し、残項目のみを残して進捗を可視化します。
- 実装時は**ビルド警告ゼロ**を維持します（`AGENTS.md` 準拠。警告抑制が必要な場合は事前確認）。
- 3 兄弟ライブラリ（Avalonia / Maui / Windows）は同種のため、**共通 ID の項目は 3 リポジトリ同時修正**します。ID は 3 リポジトリで共通です。

凡例: 共通 = 3 リポジトリ共通 / 単独 = Windows のみ

---

## チェックリスト（優先度順）

### 優先度：中 — 例外方針の判断が必要（共通は 3 リポジトリ同時）

- [ ] **CMD-1. `AsyncCommand` の例外処理／再入制御**（共通: all / 元#5）
  - 該当: `Smart.Windows/Windows/Input/AsyncCommand.cs:44,91`（`async void`・再入防止/例外観測/キャンセル連携なし）
  - 補足: 既存テスト（`AsyncCommandTest.cs:40-58,146-164`）が「`CanExecute==false` でも `Execute` 実行」を現状固定。
  - 修正量: 中（設計判断）
  - 影響/リスク: 中〜高（挙動/API 変更。テスト期待値の見直しを伴う）。3 ライブラリ同一実装のため方針統一。
  - 方針（**保留中**・2026-07-26 に「CMD-1 全体を保留」と判断）: (1) 実行中の再入制御、(2) `Execute` 内で `CanExecute` 再確認、(3) 例外ハンドリング方針明確化。
  - 補足: 例外の「診断経路を追加（互換維持）」方針は EXP-1 で実施済み（`Trace` 出力・API 追加なし）。CMD-1 に同方針を適用するかは未決。再入防止／`CanExecute` 再確認は**挙動変更**を伴うため要判断。
  - 完了条件: 二重起動防止／例外時通知／`CanExecute==false` 時挙動のテスト。

---

## 対応しないと判断した項目
- **UI/ホスト実行が必要な回帰テスト（旧 TEST-WIN）**: 非 UI 部分と `MessageTrigger` のテストは整備済み（テスト全 **180 件緑**。STA 実行ヘルパー `RunSta` は `MessageTriggerTest` に実装済み）。残る以下は **実 `Window` 表示やホスト起動が必要**なため、**対応しない**方針とした（2026-07-26 判断）。
  - `WindowPlacementBehavior` のロード後再配置（BHV-3）※`Window` の表示とサイズ確定が必要
  - `ScrollIntoOnChangedBehavior` の `ItemsSource` 追従（BHV-4）※`ListBox` のテンプレート適用が必要
  - `ApplicationHostingService` の停止/例外連携（HOST-1）※`Application.Run()` を伴う STA ホスト起動が必要
  - `DataContextResolver` の破棄所有権（RES-2）
