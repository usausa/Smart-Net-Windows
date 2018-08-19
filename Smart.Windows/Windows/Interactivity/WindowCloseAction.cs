namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(Window))]
    public sealed class WindowCloseAction : TriggerAction<Window>
    {
        protected override void Invoke(object parameter)
        {
            AssociatedObject.Close();
        }
    }
}
