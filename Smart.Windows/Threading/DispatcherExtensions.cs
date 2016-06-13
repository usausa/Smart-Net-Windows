namespace Smart.Threading
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    ///
    /// </summary>
    public static class DispatcherExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        public static void Invoke(this DispatcherObject dispatcher, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (dispatcher == null)
            {
                return;
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

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        /// <param name="arg"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore.")]
        public static void Invoke<T>(this DispatcherObject dispatcher, Action<T> action, T arg)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (dispatcher == null)
            {
                return;
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

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore.")]
        public static TResult Invoke<TResult>(this DispatcherObject dispatcher, Func<TResult> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (dispatcher == null)
            {
                return default(TResult);
            }

            if (dispatcher.CheckAccess())
            {
                return action();
            }

            return (TResult)dispatcher.Dispatcher.Invoke(DispatcherPriority.Normal, action);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        public static void AsyncInvoke(this DispatcherObject dispatcher, Action action)
        {
            dispatcher?.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        /// <param name="arg"></param>
        public static void AsyncInvoke<T>(this DispatcherObject dispatcher, Action<T> action, T arg)
        {
            dispatcher?.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action, arg);
        }
    }
}
