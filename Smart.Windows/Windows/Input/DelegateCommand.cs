namespace Smart.Windows.Input;

using System.Runtime.CompilerServices;
using System.Windows.Input;

public sealed class DelegateCommand : IObserveCommand
{
    private EventHandler? canExecuteChanged;

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            canExecuteChanged += value;
            CommandManager.RequerySuggested += value;
        }
        remove
        {
            canExecuteChanged -= value;
            CommandManager.RequerySuggested -= value;
        }
    }

    private readonly Action execute;

    private readonly Func<bool> canExecute;

    public DelegateCommand(Action execute)
        : this(execute, Functions.True)
    {
    }

    public DelegateCommand(Action execute, Func<bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    bool ICommand.CanExecute(object? parameter) => canExecute();

    void ICommand.Execute(object? parameter) => execute();

#pragma warning disable CA1030
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RaiseCanExecuteChanged()
    {
        canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
#pragma warning restore CA1030
}

public sealed class DelegateCommand<T> : IObserveCommand
{
    private EventHandler? canExecuteChanged;

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            canExecuteChanged += value;
            CommandManager.RequerySuggested += value;
        }
        remove
        {
            canExecuteChanged -= value;
            CommandManager.RequerySuggested -= value;
        }
    }

    private readonly Action<T> execute;

    private readonly Func<T, bool> canExecute;

    public DelegateCommand(Action<T> execute)
        : this(execute, Functions<T>.True)
    {
    }

    public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    bool ICommand.CanExecute(object? parameter) => canExecute(Cast(parameter));

    void ICommand.Execute(object? parameter) => execute(Cast(parameter));

    private static T Cast(object? parameter)
    {
        if (typeof(T).IsValueType && (parameter is null))
        {
            return default!;
        }

        return (T)parameter!;
    }

#pragma warning disable CA1030
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RaiseCanExecuteChanged()
    {
        canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
#pragma warning restore CA1030
}
