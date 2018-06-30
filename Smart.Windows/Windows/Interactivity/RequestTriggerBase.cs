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
    public abstract class RequestTriggerBase<TEventArgs> : TriggerBase<FrameworkElement>
        where TEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty RequestProperty = DependencyProperty.Register(
            nameof(Request),
            typeof(IEventRequest<TEventArgs>),
            typeof(RequestTriggerBase<TEventArgs>),
            new PropertyMetadata(RequestChanged));

        /// <summary>
        ///
        /// </summary>
        public IEventRequest<TEventArgs> Request
        {
            get => (IEventRequest<TEventArgs>)GetValue(RequestProperty);
            set => SetValue(RequestProperty, value);
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
            if (Request != null)
            {
                Request.Requested -= EventRequestOnRequested;
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

            var trigger = (RequestTriggerBase<TEventArgs>)obj;

            if ((e.OldValue != null) && (trigger.Request != null))
            {
                trigger.Request.Requested -= trigger.EventRequestOnRequested;
            }

            if ((e.NewValue != null) && (trigger.Request != null))
            {
                trigger.Request.Requested += trigger.EventRequestOnRequested;
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
