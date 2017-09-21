namespace Smart.Windows.Input
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Windows.Internal;

    /// <summary>
    ///
    /// </summary>
    public static class ObserverCommandExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Ignore")]
        public static TCommand Observe<TCommand, TValue>(this TCommand command, NotificationValue<TValue> value)
            where TCommand : ObserveCommandBase<TCommand>
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return command.Observe(value, nameof(value.Value));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <param name="disposables"></param>
        /// <returns></returns>
        public static TCommand RemoveObserverBy<TCommand>(this TCommand command, ICollection<IDisposable> disposables)
            where TCommand : ObserveCommandBase<TCommand>
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (disposables == null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            disposables.Add(new DelegateDisposable(() => command.RemoveObserver()));

            return command;
        }
    }
}
