namespace Smart.Windows.ViewModels;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Smart.ComponentModel;
using Smart.Windows.Input;
using Smart.Windows.Internal;
using Smart.Windows.Messaging;

#pragma warning disable IDE0032
// ReSharper disable ReplaceWithFieldKeyword
public abstract class ViewModelBase : NotificationObject, IDataErrorInfo, IDisposable
{
    private ListDisposable? disposables;

    private IBusyState? busyState;

    private IMessenger? messenger;

    // ------------------------------------------------------------
    // Disposables
    // ------------------------------------------------------------

    protected ICollection<IDisposable> Disposables => disposables ??= [];

    // ------------------------------------------------------------
    // Busy
    // ------------------------------------------------------------

    public IBusyState BusyState => busyState ??= new BusyState();

    // ------------------------------------------------------------
    // Messenger
    // ------------------------------------------------------------

    public IMessenger Messenger => messenger ??= new Messenger();

    // ------------------------------------------------------------
    // Validation
    // ------------------------------------------------------------

    public string this[string columnName]
    {
        get
        {
            var pi = GetType().GetProperty(columnName);
            if (pi is null)
            {
                return string.Empty;
            }

            var results = new List<ValidationResult>();
            if (Validator.TryValidateProperty(
                pi.GetValue(this, null),
                new ValidationContext(this, null, null) { MemberName = columnName },
                results))
            {
                return string.Empty;
            }

            return results.First().ErrorMessage ?? string.Empty;
        }
    }

    public string Error
    {
        get
        {
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(
                this,
                new ValidationContext(this, null, null),
                results))
            {
                return string.Empty;
            }

            return String.Join(Environment.NewLine, results.Select(static r => r.ErrorMessage));
        }
    }

    // ------------------------------------------------------------
    // Constructor
    // ------------------------------------------------------------

    protected ViewModelBase()
    {
    }

    protected ViewModelBase(IBusyState busyState)
    {
        this.busyState = busyState;
    }

    protected ViewModelBase(IMessenger messenger)
    {
        this.messenger = messenger;
    }

    protected ViewModelBase(IBusyState busyState, IMessenger messenger)
    {
        this.busyState = busyState;
        this.messenger = messenger;
    }

    ~ViewModelBase()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            disposables?.Dispose();
        }
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
