namespace Smart.Windows.ViewModels
{
    using Smart.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    public class BusyState : NotificationObject, IBusyState
    {
        private bool isBusy;

        /// <summary>
        ///
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }
    }
}
