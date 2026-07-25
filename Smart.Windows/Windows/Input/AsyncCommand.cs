namespace Smart.Windows.Input;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Smart.Windows.Internal;

public sealed class AsyncCommand : IObserveCommand
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

    private readonly Func<Task> execute;

    private readonly Func<bool> canExecute;

    private bool executing;

    public AsyncCommand(Func<Task> execute)
        : this(execute, Functions.True)
    {
    }

    public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    bool ICommand.CanExecute(object? parameter) => !executing && canExecute();

    // ReSharper disable once AsyncVoidMethod
    async void ICommand.Execute(object? parameter)
    {
        if (executing)
        {
            return;
        }

        executing = true;
        RaiseCanExecuteChanged();

#pragma warning disable CA1031
        try
        {
            await execute().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Execute failed. type=[{GetType()}], message=[{ex.Message}]");
        }
        finally
        {
            executing = false;
            RaiseCanExecuteChanged();
        }
#pragma warning restore CA1031
    }

#pragma warning disable CA1030
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RaiseCanExecuteChanged()
    {
        canExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
#pragma warning restore CA1030
}

public sealed class AsyncCommand<T> : IObserveCommand
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

    private readonly Func<T, Task> execute;

    private readonly Func<T, bool> canExecute;

    private bool executing;

    public AsyncCommand(Func<T, Task> execute)
        : this(execute, Functions<T>.True)
    {
    }

    public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    bool ICommand.CanExecute(object? parameter) => !executing && canExecute(Cast(parameter));

    // ReSharper disable once AsyncVoidMethod
    async void ICommand.Execute(object? parameter)
    {
        if (executing)
        {
            return;
        }

        executing = true;
        RaiseCanExecuteChanged();

#pragma warning disable CA1031
        try
        {
            await execute(Cast(parameter)).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Execute failed. type=[{GetType()}], message=[{ex.Message}]");
        }
        finally
        {
            executing = false;
            RaiseCanExecuteChanged();
        }
#pragma warning restore CA1031
    }

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
