namespace Smart.Windows.Input
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Smart.Windows.Internal;

    public sealed class AsyncCommand : ObserveCommandBase<AsyncCommand>, ICommand
    {
        private readonly Func<Task> execute;

        private readonly Func<bool> canExecute;

        private bool executing;

        public AsyncCommand(Func<Task> execute)
            : this(execute, Actions.True)
        {
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return !executing && canExecute();
        }

        public async void Execute(object parameter)
        {
            executing = true;
            RaiseCanExecuteChanged();

            try
            {
                var task = execute();
                await task;
            }
            finally
            {
                executing = false;
                RaiseCanExecuteChanged();
            }
        }
    }

    public sealed class AsyncCommand<T> : ObserveCommandBase<AsyncCommand<T>>, ICommand
    {
        private static readonly bool IsValueType = typeof(T).GetTypeInfo().IsValueType;

        private readonly Func<T, Task> execute;

        private readonly Func<T, bool> canExecute;

        private bool executing;

        public AsyncCommand(Func<T, Task> execute)
            : this(execute, Actions<T>.True)
        {
        }

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return !executing && canExecute(Cast(parameter));
        }

        public void Execute(object parameter)
        {
            Execute(Cast(parameter));
        }

        private async void Execute(T parameter)
        {
            executing = true;
            RaiseCanExecuteChanged();

            try
            {
                var task = execute(parameter);
                await task;
            }
            finally
            {
                executing = false;
                RaiseCanExecuteChanged();
            }
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
