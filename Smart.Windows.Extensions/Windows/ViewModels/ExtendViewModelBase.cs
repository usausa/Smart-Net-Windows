namespace Smart.Windows.ViewModels;

using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

using Smart.Mvvm.ViewModels;
using Smart.Windows.Input;
using Smart.Windows.Internal;

public abstract class ExtendViewModelBase : ViewModelBase
{
    private static readonly ExtendViewModelOptions DefaultOptions = new();

    // ------------------------------------------------------------
    // Member
    // ------------------------------------------------------------

    private readonly CommandMode defaultMode;

    private readonly bool autoUpdateCommandState;

    private List<IObserveCommand>? commands;

    // ------------------------------------------------------------
    // Property
    // ------------------------------------------------------------

    protected bool AcceptsCommand { get; set; } = true;

    // ------------------------------------------------------------
    // Constructor
    // ------------------------------------------------------------

    protected ExtendViewModelBase(IExtendViewModelOptions? options = null)
        : base(options ?? DefaultOptions)
    {
        var mode = options?.CommandMode ?? DefaultOptions.CommandMode;
        defaultMode = mode == CommandMode.Default ? CommandMode.Standard : mode;
        autoUpdateCommandState = options?.AutoUpdateCommandState ?? DefaultOptions.AutoUpdateCommandState;
    }

    // ------------------------------------------------------------
    // Override
    // ------------------------------------------------------------

    protected override void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        base.RaisePropertyChanged(args);

        if (autoUpdateCommandState)
        {
            UpdateCommandState();
        }
    }

    // ------------------------------------------------------------
    // Command helper
    // ------------------------------------------------------------

    private void AddCommandObserver(IObserveCommand command)
    {
        if (commands is null)
        {
            commands = [];
            BusyState.PropertyChanged += BusyStateOnPropertyChanged;
            Disposables.Add(new DelegateDisposable(() => BusyState.PropertyChanged -= BusyStateOnPropertyChanged));
        }
        commands.Add(command);
    }

    private void BusyStateOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IBusyState.IsBusy))
        {
            UpdateCommandState();
        }
    }

    private void UpdateCommandState()
    {
        if (commands is not null)
        {
            foreach (var command in commands)
            {
                command.RaiseCanExecuteChanged();
            }
        }
    }

    protected TCommand Observe<T, TCommand>(IObservable<T> observable, TCommand command)
        where TCommand : IObserveCommand
    {
        Disposables.Add(observable.Subscribe(_ => command.RaiseCanExecuteChanged()));
        return command;
    }

    protected IObserveCommand MakeDelegateCommand(Action execute) =>
        MakeDelegateCommand(CommandMode.Default, execute, Functions.True);

    protected IObserveCommand MakeDelegateCommand(Action execute, Func<bool> canExecute) =>
        MakeDelegateCommand(CommandMode.Default, execute, canExecute);

    protected IObserveCommand MakeDelegateCommand(CommandMode mode, Action execute) =>
        MakeDelegateCommand(mode, execute, Functions.True);

    protected IObserveCommand MakeDelegateCommand(CommandMode mode, Action execute, Func<bool> canExecute)
    {
        DelegateCommand command;
        var resolved = ResolveMode(mode);
        if (resolved == CommandMode.Simple)
        {
            command = new DelegateCommand(() =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                execute();
            }, canExecute);
        }
        else if (resolved == CommandMode.ControlByBusyState)
        {
            command = new DelegateCommand(() =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute();
                }
            }, () => !BusyState.IsBusy && canExecute());
        }
        else
        {
            command = new DelegateCommand(() =>
            {
                if (!AcceptsCommand || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute();
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    protected IObserveCommand MakeDelegateCommand<TParameter>(Action<TParameter> execute) =>
        MakeDelegateCommand(CommandMode.Default, execute, Functions<TParameter>.True);

    protected IObserveCommand MakeDelegateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute) =>
        MakeDelegateCommand(CommandMode.Default, execute, canExecute);

    protected IObserveCommand MakeDelegateCommand<TParameter>(CommandMode mode, Action<TParameter> execute) =>
        MakeDelegateCommand(mode, execute, Functions<TParameter>.True);

    protected IObserveCommand MakeDelegateCommand<TParameter>(CommandMode mode, Action<TParameter> execute, Func<TParameter, bool> canExecute)
    {
        DelegateCommand<TParameter> command;
        var resolved = ResolveMode(mode);
        if (resolved == CommandMode.Simple)
        {
            command = new DelegateCommand<TParameter>(x =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                execute(x);
            }, canExecute);
        }
        else if (resolved == CommandMode.ControlByBusyState)
        {
            command = new DelegateCommand<TParameter>(x =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute(x);
                }
            }, x => !BusyState.IsBusy && canExecute(x));
        }
        else
        {
            command = new DelegateCommand<TParameter>(x =>
            {
                if (!AcceptsCommand || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute(x);
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    protected IObserveCommand MakeAsyncCommand(Func<Task> execute) =>
        MakeAsyncCommand(CommandMode.Default, execute, Functions.True);

    protected IObserveCommand MakeAsyncCommand(Func<Task> execute, Func<bool> canExecute) =>
        MakeAsyncCommand(CommandMode.Default, execute, canExecute);

    protected IObserveCommand MakeAsyncCommand(CommandMode mode, Func<Task> execute) =>
        MakeAsyncCommand(mode, execute, Functions.True);

    protected IObserveCommand MakeAsyncCommand(CommandMode mode, Func<Task> execute, Func<bool> canExecute)
    {
        AsyncCommand command;
        var resolved = ResolveMode(mode);
        if (resolved == CommandMode.Simple)
        {
            command = new AsyncCommand(async () =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                await execute().ConfigureAwait(true);
            }, canExecute);
        }
        else if (resolved == CommandMode.ControlByBusyState)
        {
            command = new AsyncCommand(async () =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute().ConfigureAwait(true);
                }
            }, () => !BusyState.IsBusy && canExecute());
        }
        else
        {
            command = new AsyncCommand(async () =>
            {
                if (!AcceptsCommand || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute().ConfigureAwait(true);
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    protected IObserveCommand MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute) =>
        MakeAsyncCommand(CommandMode.Default, execute, Functions<TParameter>.True);

    protected IObserveCommand MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute) =>
        MakeAsyncCommand(CommandMode.Default, execute, canExecute);

    protected IObserveCommand MakeAsyncCommand<TParameter>(CommandMode mode, Func<TParameter, Task> execute) =>
        MakeAsyncCommand(mode, execute, Functions<TParameter>.True);

    protected IObserveCommand MakeAsyncCommand<TParameter>(CommandMode mode, Func<TParameter, Task> execute, Func<TParameter, bool> canExecute)
    {
        AsyncCommand<TParameter> command;
        var resolved = ResolveMode(mode);
        if (resolved == CommandMode.Simple)
        {
            command = new AsyncCommand<TParameter>(async x =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                await execute(x).ConfigureAwait(true);
            }, canExecute);
        }
        else if (resolved == CommandMode.ControlByBusyState)
        {
            command = new AsyncCommand<TParameter>(async x =>
            {
                if (!AcceptsCommand)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute(x).ConfigureAwait(true);
                }
            }, x => !BusyState.IsBusy && canExecute(x));
        }
        else
        {
            command = new AsyncCommand<TParameter>(async x =>
            {
                if (!AcceptsCommand || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute(x).ConfigureAwait(true);
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CommandMode ResolveMode(CommandMode mode) =>
        mode == CommandMode.Default ? defaultMode : mode;

    // ------------------------------------------------------------
    // Reactive helper
    // ------------------------------------------------------------

    protected IObservable<string?> Observe(string name)
    {
        return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                static h => (_, e) => h(e),
                h => PropertyChanged += h,
                h => PropertyChanged -= h)
            .Where(x => x.PropertyName == name)
            .Select(x => x.PropertyName);
    }

    protected void Subscribe<T>(IObservable<T> observable, Action<T> action)
    {
        Disposables.Add(observable.Subscribe(action));
    }
}
