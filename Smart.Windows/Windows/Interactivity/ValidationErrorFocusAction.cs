namespace Smart.Windows.Interactivity
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    using Smart.Windows.Messaging;

    /// <summary>
    ///
    /// </summary>
    public class ValidationErrorFocusAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var element = AssociatedObject.FindChildrens<UIElement>().FirstOrDefault(Validation.GetHasError);
            if (element != null)
            {
                element.Focus();

                var message = parameter as CancelMessage;
                if (message != null)
                {
                    message.Cancel = true;
                }
            }
        }
    }
}
