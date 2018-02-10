namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(Button))]
    public sealed class DialogResultBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.Register(
            nameof(DialogResult),
            typeof(bool),
            typeof(DialogResultBehavior),
            new FrameworkPropertyMetadata(true));

        /// <summary>
        ///
        /// </summary>
        public bool DialogResult
        {
            get => (bool)GetValue(DialogResultProperty);
            set => SetValue(DialogResultProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Click += OnButtonClick;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnButtonClick;

            base.OnDetaching();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var window = AssociatedObject.FindParent<Window>();
            if (window != null)
            {
                window.DialogResult = DialogResult;
            }
        }
    }
}
