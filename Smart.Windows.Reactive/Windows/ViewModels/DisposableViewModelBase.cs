namespace Smart.Windows.ViewModels
{
    using System;
    using System.Reactive.Disposables;
    using System.Threading.Tasks;

    using Smart.Windows.Input;
    using Smart.Windows.Internal;
    using Smart.Windows.Messaging;

    /// <summary>
    ///
    /// </summary>
    public abstract class DisposableViewModelBase : ViewModelBase, IDisposable
    {
        private CompositeDisposable disposables;

        /// <summary>
        ///
        /// </summary>
        protected CompositeDisposable Disposables => disposables ?? (disposables = new CompositeDisposable());

        /// <summary>
        ///
        /// </summary>
        protected DisposableViewModelBase()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messenger"></param>
        protected DisposableViewModelBase(Messenger messenger)
            : base(messenger)
        {
        }

        /// <summary>
        ///
        /// </summary>
        ~DisposableViewModelBase()
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
        // Command helper
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeBusyCommand(Func<Task> execute)
        {
            return MakeBusyCommand(execute, Actions.True);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        protected AsyncCommand MakeBusyCommand(Func<Task> execute, Func<bool> canExecute)
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
        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Func<TParameter, Task> execute)
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
        protected AsyncCommand<TParameter> MakeBusyCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute)
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
