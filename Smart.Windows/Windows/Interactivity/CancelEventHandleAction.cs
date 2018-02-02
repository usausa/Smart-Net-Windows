namespace Smart.Windows.Interactivity
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(DependencyObject))]
    public class CancelEventHandleAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty CancelProperty = DependencyProperty.Register(
            nameof(Cancel),
            typeof(bool),
            typeof(CancelEventHandleAction),
            new PropertyMetadata(default(bool)));

        /// <summary>
        ///
        /// </summary>
        public bool Cancel
        {
            get => (bool)GetValue(CancelProperty);
            set => SetValue(CancelProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var args = (CancelEventArgs)parameter;
            args.Cancel = Cancel;
        }
    }
}
