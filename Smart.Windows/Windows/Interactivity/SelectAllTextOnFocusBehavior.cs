namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(TextBox))]
    public sealed class SelectAllTextOnFocusBehavior : Behavior<TextBox>
    {
        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.GotKeyboardFocus += SelectAllText;
            AssociatedObject.GotMouseCapture += SelectAllText;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= SelectAllText;
            AssociatedObject.GotMouseCapture -= SelectAllText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            AssociatedObject.SelectAll();
        }
    }
}