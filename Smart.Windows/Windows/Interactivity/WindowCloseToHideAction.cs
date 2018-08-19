namespace Smart.Windows.Interactivity
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(Window))]
    public sealed class WindowCloseToHideAction : TriggerAction<Window>
    {
        public static readonly DependencyProperty ClosableProperty = DependencyProperty.Register(
            nameof(Closable),
            typeof(bool),
            typeof(WindowCloseToHideAction),
            new PropertyMetadata(default(bool)));

        public bool Closable
        {
            get => (bool)GetValue(ClosableProperty);
            set => SetValue(ClosableProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (!Closable)
            {
                var args = (CancelEventArgs)parameter;
                args.Cancel = true;

                AssociatedObject.Hide();
            }
        }
    }
}
