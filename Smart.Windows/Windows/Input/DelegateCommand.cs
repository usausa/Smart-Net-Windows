namespace Smart.Windows.Input
{
    using System;
    using System.Windows.Input;

    /// <summary>
    ///
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        ///
        /// </summary>
        public Action CommandAction { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Func<bool> CanExecuteFunc { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            CommandAction();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
