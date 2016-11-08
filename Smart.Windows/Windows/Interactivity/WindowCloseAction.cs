namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class WindowCloseAction : TriggerAction<Window>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            AssociatedObject.Close();
        }
    }
}
