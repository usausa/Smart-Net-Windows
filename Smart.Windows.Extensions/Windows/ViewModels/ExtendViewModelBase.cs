namespace Smart.Windows.ViewModels;

using System.ComponentModel;
using System.Reactive.Linq;

using Smart.Mvvm.Messaging;
using Smart.Mvvm.ViewModels;
using Smart.Windows.Input;

#pragma warning disable IDE0032
// ReSharper disable ReplaceWithFieldKeyword
public abstract class ExtendViewModelBase : ViewModelBase
{
    private static class Functions
    {
        public static Func<bool> True { get; } = static () => true;
    }

    private static class Functions<T>
    {
        public static Func<T, bool> True { get; } = static _ => true;
    }

    // ------------------------------------------------------------
    // Member
    // ------------------------------------------------------------

    private List<IObserveCommand>? commands;

    // ------------------------------------------------------------
    // Constructor
    // ------------------------------------------------------------

    protected ExtendViewModelBase()
    {
    }

    protected ExtendViewModelBase(IBusyState busyState)
        : base(busyState)
    {
    }

    protected ExtendViewModelBase(IMessenger messenger)
        : base(messenger)
    {
    }

    protected ExtendViewModelBase(IBusyState busyState, IMessenger messenger)
        : base(busyState, messenger)
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (commands is not null)
        {
            PropertyChanged -= UpdateCommandState;
            commands = null;
        }

        base.Dispose(disposing);
    }

    // ------------------------------------------------------------
    // Command helper
    // ------------------------------------------------------------

    private void AddCommandObserver(IObserveCommand command)
    {
        if (commands is null)
        {
            commands = new List<IObserveCommand>();
            PropertyChanged += UpdateCommandState;
        }
        commands.Add(command);
    }

    private void UpdateCommandState(object? sender, PropertyChangedEventArgs e) =>
        UpdateCommandState();

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

    protected DelegateCommand MakeDelegateCommand(Action execute) =>
        MakeDelegateCommand(execute, Functions.True);

    protected DelegateCommand MakeDelegateCommand(Action execute, Func<bool> canExecute)
    {
        var command = new DelegateCommand(() =>
        {
            execute();
            UpdateCommandState();
        }, canExecute);
        AddCommandObserver(command);
        return command;
    }

    protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute) =>
        MakeDelegateCommand(execute, Functions<TParameter>.True);

    protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
    {
        var command = new DelegateCommand<TParameter>(x =>
        {
            execute(x);
            UpdateCommandState();
        }, canExecute);
        AddCommandObserver(command);
        return command;
    }

    protected AsyncCommand MakeAsyncCommand(Func<Task> execute) =>
        MakeAsyncCommand(execute, Functions.True);

    protected AsyncCommand MakeAsyncCommand(Func<Task> execute, Func<bool> canExecute)
    {
        var command = new AsyncCommand(async () =>
        {
            using (BusyState.Begin())
            {
                await execute().ConfigureAwait(true);
            }
            UpdateCommandState();
        }, canExecute);
        AddCommandObserver(command);
        return command;
    }

    protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute) =>
        MakeAsyncCommand(execute, Functions<TParameter>.True);

    protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute)
    {
        var command = new AsyncCommand<TParameter>(async x =>
        {
            using (BusyState.Begin())
            {
                await execute(x).ConfigureAwait(true);
            }
            UpdateCommandState();
        }, canExecute);
        AddCommandObserver(command);
        return command;
    }

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
