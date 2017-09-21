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

        private Messenger messenger;

        private bool isBusy;

        // ------------------------------------------------------------
        // Disposables
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        protected ICollection<IDisposable> Disposables => disposables ?? (disposables = new ListDisposable());

        // ------------------------------------------------------------
        // Messenger
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        public Messenger Messenger => messenger ?? (messenger = new Messenger());

        // ------------------------------------------------------------
        // Busy
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

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
        /// <param name="messenger"></param>
        protected ViewModelBase(Messenger messenger)
        {
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
        public async Task ExecuteBusyAsync(Func<Task> execute)
        {
            try
            {
                IsBusy = true;

                await execute();
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="execute"></param>
        /// <returns></returns>
        public async Task<TResult> ExecuteBusyAsync<TResult>(Func<Task<TResult>> execute)
        {
            try
            {
                IsBusy = true;

                return await execute();
            }
            finally
            {
                IsBusy = false;
            }
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
            return new DelegateCommand(execute, canExecute)
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
            return new DelegateCommand<TParameter>(execute, canExecute)
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
                    IsBusy = true;
                    try
                    {
                        await execute();
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, () => !IsBusy && canExecute())
                .Observe(this, nameof(IsBusy))
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
                    IsBusy = true;
                    try
                    {
                        await execute(parameter);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, parameter => !IsBusy && canExecute(parameter))
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeAsyncCommand(Action execute)
        {
            return MakeAsyncCommand(execute, Actions.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeAsyncCommand(Action execute, Func<bool> canExecute)
        {
            return new AsyncCommand(
                () =>
                {
                    IsBusy = true;
                    try
                    {
                        execute();
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, () => !IsBusy && canExecute())
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Action<TParameter> execute)
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
        protected AsyncCommand<TParameter> MakeAsyncCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            return new AsyncCommand<TParameter>(
                parameter =>
                {
                    IsBusy = true;
                    try
                    {
                        execute(parameter);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }, parameter => !IsBusy && canExecute(parameter))
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeBusyCommand(Action execute)
        {
            return MakeBusyCommand(execute, Actions.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeBusyCommand(Action execute, Func<bool> canExecute)
        {
            return new AsyncCommand(
                    () =>
                    {
                        IsBusy = true;
                        try
                        {
                            execute();
                        }
                        finally
                        {
                            IsBusy = false;
                        }
                    }, () => !IsBusy && canExecute())
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Action<TParameter> execute)
        {
            return MakeBusyCommand(execute, Actions<TParameter>.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            return new AsyncCommand<TParameter>(
                    parameter =>
                    {
                        IsBusy = true;
                        try
                        {
                            execute(parameter);
                        }
                        finally
                        {
                            IsBusy = false;
                        }
                    }, parameter => !IsBusy && canExecute(parameter))
                .Observe(this, nameof(IsBusy))
                .RemoveObserverBy(Disposables);
        }
    }
}
