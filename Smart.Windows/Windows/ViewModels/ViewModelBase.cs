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

    /// <summary>
    ///
    /// </summary>
    public abstract class ViewModelBase : NotificationObject, IDataErrorInfo, IDisposable
    {
        private ListDisposable disposables;

        private IBusyState busyState;

        private IMessenger messenger;

        // ------------------------------------------------------------
        // Disposables
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        protected ICollection<IDisposable> Disposables => disposables ?? (disposables = new ListDisposable());

        // ------------------------------------------------------------
        // Busy
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        public IBusyState BusyState => busyState ?? (busyState = new BusyState());

        // ------------------------------------------------------------
        // Messenger
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        public IMessenger Messenger => messenger ?? (messenger = new Messenger());

        // ------------------------------------------------------------
        // Validation
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        ///
        /// </summary>
        protected ViewModelBase()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="busyState"></param>
        protected ViewModelBase(IBusyState busyState)
        {
            this.busyState = busyState;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messenger"></param>
        protected ViewModelBase(Messenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="busyState"></param>
        /// <param name="messenger"></param>
        protected ViewModelBase(IBusyState busyState, IMessenger messenger)
        {
            this.busyState = busyState;
            this.messenger = messenger;
        }

        /// <summary>
        ///
        /// </summary>
        ~ViewModelBase()
        {
            Dispose(false);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                disposables?.Dispose();
            }
        }

        // ------------------------------------------------------------
        // Execute helper
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        public Task ExecuteBusyAsync(Func<Task> execute)
        {
            return BusyHelper.ExecuteBusyAsync(busyState, execute);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="execute"></param>
        /// <returns></returns>
        public Task<TResult> ExecuteBusyAsync<TResult>(Func<Task<TResult>> execute)
        {
            return BusyHelper.ExecuteBusyAsync(busyState, execute);
        }

        // ------------------------------------------------------------
        // DelegateCommand helper
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected DelegateCommand MakeDelegateCommand(Action execute)
        {
            return MakeDelegateCommand(execute, Actions.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected DelegateCommand MakeDelegateCommand(Action execute, Func<bool> canExecute)
        {
            return new DelegateCommand(execute, () => !BusyState.IsBusy && canExecute())
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute)
        {
            return MakeDelegateCommand(execute, Actions<TParameter>.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected DelegateCommand<TParameter> MakeDelegateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            return new DelegateCommand<TParameter>(execute, x => !BusyState.IsBusy && canExecute(x))
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }

        // ------------------------------------------------------------
        // AsyncCommand helper
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeAsyncCommand(Func<Task> execute)
        {
            return MakeAsyncCommand(execute, Actions.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeAsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            return new AsyncCommand(
                async () =>
                {
                    BusyState.IsBusy = true;
                    try
                    {
                        await execute();
                    }
                    finally
                    {
                        BusyState.IsBusy = false;
                    }
                }, () => !BusyState.IsBusy && canExecute())
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute)
        {
            return MakeAsyncCommand(execute, Actions<TParameter>.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute)
        {
            return new AsyncCommand<TParameter>(
                async parameter =>
                {
                    BusyState.IsBusy = true;
                    try
                    {
                        await execute(parameter);
                    }
                    finally
                    {
                        BusyState.IsBusy = false;
                    }
                }, parameter => !BusyState.IsBusy && canExecute(parameter))
                .Observe(BusyState, nameof(IBusyState.IsBusy))
                .RemoveObserverBy(Disposables);
        }
    }
}
