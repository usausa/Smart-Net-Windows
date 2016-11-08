namespace Smart.Windows.Interactivity
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class CancelEventHandleAction : TriggerAction<Window>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty CancelProperty = DependencyProperty.Register("Cancel", typeof(bool), typeof(CancelEventHandleAction), new PropertyMetadata(null));

        /// <summary>
        ///
        /// </summary>
        public bool Cancel
        {
            get { return (bool)GetValue(CancelProperty); }
            set { SetValue(CancelProperty, value); }
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
