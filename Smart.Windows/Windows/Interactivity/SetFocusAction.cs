namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class SetFocusAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            AssociatedObject.Focus();
        }
    }
}
