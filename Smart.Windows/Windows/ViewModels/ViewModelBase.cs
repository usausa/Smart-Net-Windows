namespace Smart.Windows.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Smart.ComponentModel;
    using Smart.Windows.Input;
    using Smart.Windows.Internal;
    using Smart.Windows.Messaging;

    public abstract class ViewModelBase : NotificationObject, IDataErrorInfo, IDisposable
    {
        private ListDisposable disposables;

        private IBusyState busyState;

        private IMessenger messenger;

        // ------------------------------------------------------------
        // Disposables
        // ------------------------------------------------------------

        protected ICollection<IDisposable> Disposables => disposables ??= new ListDisposable();

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
                var results = new List<ValidationResult>();
                if (Validator.TryValidateProperty(
                    GetType().GetProperty(columnName).GetValue(this, null),
                    new ValidationContext(this, null, null) { MemberName = columnName },
                    results))
                {
                    return null;
                }

                return results.First().ErrorMessage;
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

                return String.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
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

        protected ViewModelBase(Messenger messenger)
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
            return new DelegateCommand(execute, () => !BusyState.IsBusy && canExecute())
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }

        protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute)
        {
            return MakeDelegateCommand(execute, Functions<TParameter>.True);
        }

        protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            return new DelegateCommand<TParameter>(execute, x => !BusyState.IsBusy && canExecute(x))
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }

        // ------------------------------------------------------------
        // AsyncCommand helper
        // ------------------------------------------------------------

        protected AsyncCommand MakeAsyncCommand(Func<Task> execute)
        {
            return MakeAsyncCommand(execute, Functions.True);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2007:DoNotDirectlyAwaitATask", Justification = "Ignore")]
        protected AsyncCommand MakeAsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            return new AsyncCommand(
                async () =>
                {
                    using (BusyState.Begin())
                    {
                        await execute();
                    }
                }, () => !BusyState.IsBusy && canExecute())
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }

        protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute)
        {
            return MakeAsyncCommand(execute, Functions<TParameter>.True);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2007:DoNotDirectlyAwaitATask", Justification = "Ignore")]
        protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute)
        {
            return new AsyncCommand<TParameter>(
                    async parameter =>
                    {
                        using (BusyState.Begin())
                        {
                            await execute(parameter);
                        }
                    }, parameter => !BusyState.IsBusy && canExecute(parameter))
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }
    }
}
