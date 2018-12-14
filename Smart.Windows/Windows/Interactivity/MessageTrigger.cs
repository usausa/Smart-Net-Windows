namespace Smart.Windows.Interactivity
{
    using System;
    using System.Windows;

    using Microsoft.Xaml.Behaviors;

    using Smart.Windows.Messaging;

    [TypeConstraint(typeof(FrameworkElement))]
    public sealed class MessageTrigger : TriggerBase<FrameworkElement>
    {
        public static readonly DependencyProperty MessengerProperty = DependencyProperty.Register(
            nameof(Messenger),
            typeof(IMessenger),
            typeof(MessageTrigger),
            new PropertyMetadata(HandleMessengerPropertyChanged));

        public IMessenger Messenger
        {
            get => (IMessenger)GetValue(MessengerProperty);
            set => SetValue(MessengerProperty, value);
        }

        public string Label { get; set; }

        public Type MessageType { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Unloaded -= OnUnloaded;

            base.OnDetaching();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Messenger != null)
            {
                Messenger.Received -= MessengerOnReceived;
            }
        }

        private static void HandleMessengerPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var trigger = (MessageTrigger)obj;

            if ((e.OldValue != null) && (trigger.Messenger != null))
            {
                trigger.Messenger.Received -= trigger.MessengerOnReceived;
            }

            if ((e.NewValue != null) && (trigger.Messenger != null))
            {
                trigger.Messenger.Received += trigger.MessengerOnReceived;
            }
        }

        private void MessengerOnReceived(object sender, MessengerEventArgs e)
        {
            if (((Label == null) || Label.Equals(e.Label)) &&
                ((MessageType == null) || ((e.MessageType != null) && MessageType.IsAssignableFrom(e.MessageType))))
            {
                InvokeActions(e.Message);
            }
        }
    }
}
