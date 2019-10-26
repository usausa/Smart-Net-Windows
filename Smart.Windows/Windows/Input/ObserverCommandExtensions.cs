namespace Smart.Windows.Input
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Smart.Windows.Internal;

    public static class ObserverCommandExtensions
    {
        public static TCommand Observe<TCommand>(this TCommand command, params INotifyPropertyChanged[] values)
            where TCommand : ObserveCommandBase<TCommand>
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                command.Observe(value);
            }

            return command;
        }

        public static TCommand Observe<TCommand>(this TCommand command, IEnumerable<INotifyPropertyChanged> values)
            where TCommand : ObserveCommandBase<TCommand>
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                command.Observe(value);
            }

            return command;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Ignore")]
        public static TCommand RemoveObserverBy<TCommand>(this TCommand command, ICollection<IDisposable> disposables)
            where TCommand : ObserveCommandBase<TCommand>
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (disposables is null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            disposables.Add(new DelegateDisposable(() => command.RemoveObserver()));

            return command;
        }
    }
}
