namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(UIElement))]
    public sealed class SetFocusOnLoadBehavior : Behavior<UIElement>
    {
        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            var fs = FocusManager.GetFocusScope(AssociatedObject);
            FocusManager.SetFocusedElement(fs, AssociatedObject);
        }
    }
}