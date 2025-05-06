namespace Smart.Windows.ViewModels;

using Smart.Mvvm.State;
using Smart.Mvvm.ViewModels;
using Smart.Windows.Input;

#pragma warning disable IDE0032
// ReSharper disable ReplaceWithFieldKeyword
public abstract class WindowsViewModelBase : ViewModelBase
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
    // DelegateCommand helper
    // ------------------------------------------------------------

    protected DelegateCommand MakeDelegateCommand(Action execute)
    {
        return MakeDelegateCommand(execute, Functions.True);
    }

    protected DelegateCommand MakeDelegateCommand(Action execute, Func<bool> canExecute)
    {
        var command = new DelegateCommand(execute, () => !BusyState.IsBusy && canExecute());
        command.Observe(BusyState);
        Disposables.Add(command);
        return command;
    }

    protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute)
    {
        return MakeDelegateCommand(execute, Functions<TParameter>.True);
    }

    protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
    {
        var command = new DelegateCommand<TParameter>(execute, x => !BusyState.IsBusy && canExecute(x));
        command.Observe(BusyState);
        Disposables.Add(command);
        return command;
    }

    // ------------------------------------------------------------
    // AsyncCommand helper
    // ------------------------------------------------------------

    protected AsyncCommand MakeAsyncCommand(Func<Task> execute)
    {
        return MakeAsyncCommand(execute, Functions.True);
    }

    protected AsyncCommand MakeAsyncCommand(Func<Task> execute, Func<bool> canExecute)
    {
        var command = new AsyncCommand(async () =>
        {
            using (BusyState.Begin())
            {
                await execute().ConfigureAwait(true);
            }
        }, () => !BusyState.IsBusy && canExecute());
        command.Observe(BusyState);
        Disposables.Add(command);
        return command;
    }

    protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute)
    {
        return MakeAsyncCommand(execute, Functions<TParameter>.True);
    }

    protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute)
    {
        var command = new AsyncCommand<TParameter>(async parameter =>
        {
            using (BusyState.Begin())
            {
                await execute(parameter).ConfigureAwait(true);
            }
        }, x => !BusyState.IsBusy && canExecute(x));
        command.Observe(BusyState);
        Disposables.Add(command);
        return command;
    }
}
