namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Interactivity;

    using Smart.Windows.Messaging;

    /// <summary>
    ///
    /// </summary>
    public class MessageTrigger : TriggerBase<FrameworkElement>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty MessengerProperty = DependencyProperty.Register(
            "Messenger",
            typeof(Messenger),
            typeof(MessageTrigger),
            new PropertyMetadata(MessengerChanged));

        /// <summary>
        ///
        /// </summary>
        public Messenger Messenger
        {
            get { return (Messenger)GetValue(MessengerProperty); }
            set { SetValue(MessengerProperty, value); }
        }

        /// <summary>
        ///
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Unloaded += OnUnloaded;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Unloaded -= OnUnloaded;

            base.OnDetaching();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Messenger != null)
            {
                Messenger.Recieved -= MessengerOnRecieved;
            }
        }

        private static void MessengerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var trigger = (MessageTrigger)obj;

            if ((e.OldValue != null) && (trigger.Messenger != null))
            {
                trigger.Messenger.Recieved -= trigger.MessengerOnRecieved;
            }

            if ((e.NewValue != null) && (trigger.Messenger != null))
            {
                trigger.Messenger.Recieved += trigger.MessengerOnRecieved;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessengerOnRecieved(object sender, MessengerEventArgs e)
        {
            if ((Message == null) || Message.Equals(e.Message))
            {
                InvokeActions(e.Parameter);
            }
        }
    }
}
