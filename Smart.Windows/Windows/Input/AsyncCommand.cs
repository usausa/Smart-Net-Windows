namespace Smart.Windows.Input
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Smart.Windows.Internal;

    /// <summary>
    ///
    /// </summary>
    public sealed class AsyncCommand : ObserveCommandBase<AsyncCommand>, ICommand
    {
        private readonly Func<Task> execute;

        private readonly Func<bool> canExecute;

        private bool executing;

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Func<Task> execute)
            : this(execute, Actions.True)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Action execute)
            : this(execute, Actions.True)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = () =>
            {
                execute();
                return Task.CompletedTask;
            };
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return !executing && canExecute();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
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

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AsyncCommand<T> : ObserveCommandBase<AsyncCommand<T>>, ICommand
    {
        private static readonly bool IsValueType = typeof(T).GetTypeInfo().IsValueType;

        private readonly Func<T, Task> execute;

        private readonly Func<T, bool> canExecute;

        private bool executing;

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Func<T, Task> execute)
            : this(execute, Actions<T>.True)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        public AsyncCommand(Action<T> execute)
            : this(execute, Actions<T>.True)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public AsyncCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = arg =>
            {
                execute(arg);
                return Task.CompletedTask;
            };
            this.canExecute = canExecute;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return !executing && canExecute(Cast(parameter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            Execute(Cast(parameter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static T Cast(object parameter)
        {
            if ((parameter == null) && IsValueType)
            {
                return default;
            }

            return (T)parameter;
        }
    }
}
