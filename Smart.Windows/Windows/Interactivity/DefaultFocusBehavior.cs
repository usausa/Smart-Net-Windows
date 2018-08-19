namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(UIElement))]
    public sealed class DefaultFocusBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var fs = FocusManager.GetFocusScope(AssociatedObject);
            FocusManager.SetFocusedElement(fs, AssociatedObject);
        }
    }
}