namespace Smart.Windows.Internal
{
    using System;

    /// <summary>
    ///
    /// </summary>
    internal sealed class DelegateDisposable : IDisposable
    {
        private readonly Action action;

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public DelegateDisposable(Action action)
        {
            this.action = action;
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            action();
        }
    }
}
