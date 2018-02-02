namespace Smart.Windows.Interactivity
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(Window))]
    public class WindowCloseToHideAction : TriggerAction<Window>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty ClosableProperty = DependencyProperty.Register(
            nameof(Closable),
            typeof(bool),
            typeof(WindowCloseToHideAction),
            new PropertyMetadata(default(bool)));

        /// <summary>
        ///
        /// </summary>
        public bool Closable
        {
            get => (bool)GetValue(ClosableProperty);
            set => SetValue(ClosableProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
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
