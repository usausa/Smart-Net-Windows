namespace Smart.Windows.Input
{
    using System;
    using System.Windows.Input;

    using Smart.Windows.Internal;

    public sealed class DelegateCommand : ObserveCommandBase<DelegateCommand>, ICommand
    {
        private readonly Action execute;

        private readonly Func<bool> canExecute;

        public DelegateCommand(Action execute)
            : this(execute, Actions.True)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        void ICommand.Execute(object parameter)
        {
            execute();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return canExecute();
        }
    }

    public sealed class DelegateCommand<T> : ObserveCommandBase<DelegateCommand<T>>, ICommand
    {
        private static readonly bool IsValueType = typeof(T).IsValueType;

        private readonly Action<T> execute;

        private readonly Func<T, bool> canExecute;

        public DelegateCommand(Action<T> execute)
            : this(execute, Actions<T>.True)
        {
        }

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        void ICommand.Execute(object parameter)
        {
            execute(Cast(parameter));
        }

        bool ICommand.CanExecute(object parameter)
        {
            return canExecute(Cast(parameter));
        }

        private static T Cast(object parameter)
        {
            if ((parameter is null) && IsValueType)
            {
                return default;
            }

            return (T)parameter;
        }
    }
}
