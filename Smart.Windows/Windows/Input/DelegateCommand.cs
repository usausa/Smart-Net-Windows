namespace Smart.Windows.Input
{
    using System;
    using System.Windows.Input;

    using Smart.Windows.Internal;

    public sealed class DelegateCommand : ObserveCommandBase<DelegateCommand>, ICommand, IDisposable
    {
        private readonly Action execute;

        private readonly Func<bool> canExecute;

        public DelegateCommand(Action execute)
            : this(execute, Functions.True)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Dispose() => RemoveObservers();

        void ICommand.Execute(object? parameter)
        {
            execute();
        }

        bool ICommand.CanExecute(object? parameter) => canExecute();
    }

    public sealed class DelegateCommand<T> : ObserveCommandBase<DelegateCommand<T>>, ICommand, IDisposable
    {
        private static readonly bool IsValueType = typeof(T).IsValueType;

        private readonly Action<T> execute;

        private readonly Func<T, bool> canExecute;

        public DelegateCommand(Action<T> execute)
            : this(execute, Functions<T>.True)
        {
        }

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Dispose() => RemoveObservers();

        void ICommand.Execute(object? parameter)
        {
            execute(Cast(parameter));
        }

        bool ICommand.CanExecute(object? parameter) => canExecute(Cast(parameter));

        private static T Cast(object? parameter)
        {
            if ((parameter is null) && IsValueType)
            {
                return default!;
            }

            return (T)parameter!;
        }
    }
}
