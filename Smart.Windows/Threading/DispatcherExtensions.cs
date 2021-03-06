namespace Smart.Threading
{
    using System;
    using System.Windows.Threading;

    public static class DispatcherExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static void Invoke(this DispatcherObject dispatcher, Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Dispatcher.Invoke(DispatcherPriority.Normal, action);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static void Invoke<T>(this DispatcherObject dispatcher, Action<T> action, T arg)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (dispatcher.CheckAccess())
            {
                action(arg);
            }
            else
            {
                dispatcher.Dispatcher.Invoke(DispatcherPriority.Normal, action, arg);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public static TResult Invoke<TResult>(this DispatcherObject dispatcher, Func<TResult> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (dispatcher.CheckAccess())
            {
                return action();
            }

            return (TResult)dispatcher.Dispatcher.Invoke(DispatcherPriority.Normal, action);
        }

        public static void AsyncInvoke(this DispatcherObject dispatcher, Action action)
        {
            dispatcher?.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }

        public static void AsyncInvoke<T>(this DispatcherObject dispatcher, Action<T> action, T arg)
        {
            dispatcher?.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action, arg);
        }
    }
}
