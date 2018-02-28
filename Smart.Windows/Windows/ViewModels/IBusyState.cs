namespace Smart.Windows.ViewModels
{
    using System.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    public interface IBusyState : INotifyPropertyChanged
    {
        /// <summary>
        ///
        /// </summary>
        bool IsBusy { get; set; }
    }
}
