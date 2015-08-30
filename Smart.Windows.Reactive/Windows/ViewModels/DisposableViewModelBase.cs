namespace Smart.Windows.ViewModels
{
    using System;
    using System.Reactive.Disposables;

    /// <summary>
    ///
    /// </summary>
    public abstract class DisposableViewModelBase : ViewModelBase, IDisposable
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        /// <summary>
        ///
        /// </summary>
        protected CompositeDisposable Disposables
        {
            get { return disposables; }
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
                disposables.Dispose();
            }
        }
    }
}
