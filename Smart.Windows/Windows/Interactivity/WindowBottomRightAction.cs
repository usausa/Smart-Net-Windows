namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(Window))]
    public sealed class WindowBottomRightAction : TriggerAction<Window>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var workArea = SystemParameters.WorkArea;
            AssociatedObject.Left = workArea.Right - AssociatedObject.Width;
            AssociatedObject.Top = workArea.Bottom - AssociatedObject.Height;
        }
    }
}
