namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(FrameworkElement))]
    public sealed class ChangeCursorAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty CursorProperty = DependencyProperty.Register(
            nameof(Cursor),
            typeof(Cursor),
            typeof(ChangeCursorAction),
            new PropertyMetadata(null));

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty ApplicationWideProperty = DependencyProperty.Register(
            nameof(ApplicationWide),
            typeof(bool),
            typeof(ChangeCursorAction),
            new PropertyMetadata(false));

        /// <summary>
        ///
        /// </summary>
        public Cursor Cursor
        {
            get => (Cursor)GetValue(CursorProperty);
            set => SetValue(CursorProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        public bool ApplicationWide
        {
            get => (bool)GetValue(ApplicationWideProperty);
            set => SetValue(ApplicationWideProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (ApplicationWide)
            {
                Mouse.OverrideCursor = Cursor;
            }
            else
            {
                AssociatedObject.Cursor = Cursor;
            }
        }
    }
}
