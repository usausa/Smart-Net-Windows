namespace Smart.Windows.Interactivity
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class WindowCloseToHideAction : TriggerAction<Window>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty ClosableProperty = DependencyProperty.Register("Closable", typeof(bool), typeof(WindowCloseToHideAction), new PropertyMetadata(null));

        /// <summary>
        ///
        /// </summary>
        public bool Closable
        {
            get { return (bool)GetValue(ClosableProperty); }
            set { SetValue(ClosableProperty, value); }
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
