namespace Smart.Windows.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;

    using Smart.Windows.Messaging;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    [TypeConstraint(typeof(FrameworkElement))]
    public abstract class EventRequestTriggerBase<TEventArgs> : TriggerBase<FrameworkElement>
        where TEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty EventRequestProperty = DependencyProperty.Register(
            nameof(EventRequest),
            typeof(IEventRequest<TEventArgs>),
            typeof(EventRequestTriggerBase<TEventArgs>),
            new PropertyMetadata(RequestChanged));

        /// <summary>
        ///
        /// </summary>
        public IEventRequest<TEventArgs> EventRequest
        {
            get => (IEventRequest<TEventArgs>)GetValue(EventRequestProperty);
            set => SetValue(EventRequestProperty, value);
        }

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
            if (EventRequest != null)
            {
                EventRequest.Requested -= EventRequestOnRequested;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void RequestChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var trigger = (EventRequestTriggerBase<TEventArgs>)obj;

            if ((e.OldValue != null) && (trigger.EventRequest != null))
            {
                trigger.EventRequest.Requested -= trigger.EventRequestOnRequested;
            }

            if ((e.NewValue != null) && (trigger.EventRequest != null))
            {
                trigger.EventRequest.Requested += trigger.EventRequestOnRequested;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventRequestOnRequested(object sender, TEventArgs e)
        {
            OnEventRequest(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void OnEventRequest(object sender, TEventArgs e);
    }
}
