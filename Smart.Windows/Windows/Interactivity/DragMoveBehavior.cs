namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(Window))]
    public class DragMoveBehavior : Behavior<Window>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
            nameof(Enabled),
            typeof(bool),
            typeof(DragMoveBehavior),
            new PropertyMetadata(true));

        /// <summary>
        ///
        /// </summary>
        public bool Enabled
        {
            get => (bool)GetValue(EnabledProperty);
            set => SetValue(EnabledProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.MouseDown += OnMouseDown;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.MouseDown -= OnMouseDown;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (Enabled && (args.LeftButton == MouseButtonState.Pressed))
            {
                AssociatedObject.DragMove();
            }
        }
    }
}