namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class DragMoveBehavior : Behavior<Window>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "DependencyProperty")]
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register("Enabled", typeof(bool), typeof(DragMoveBehavior), new PropertyMetadata(true));

        /// <summary>
        ///
        /// </summary>
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
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
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (Enabled)
            {
                AssociatedObject.DragMove();
            }
        }
    }
}