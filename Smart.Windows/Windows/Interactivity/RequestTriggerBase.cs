namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Windows.Messaging;

[TypeConstraint(typeof(FrameworkElement))]
public abstract class RequestTriggerBase<TEventArgs> : TriggerBase<FrameworkElement>
    where TEventArgs : EventArgs
{
    public static readonly DependencyProperty RequestProperty = DependencyProperty.Register(
        nameof(Request),
        typeof(IEventRequest<TEventArgs>),
        typeof(RequestTriggerBase<TEventArgs>),
        new PropertyMetadata(HandleRequestPropertyChanged));

    public IEventRequest<TEventArgs>? Request
    {
        get => (IEventRequest<TEventArgs>)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

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
        if (Request is not null)
        {
            Request.Requested -= EventRequestOnRequested;
        }
    }

    private static void HandleRequestPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue)
        {
            return;
        }

        var trigger = (RequestTriggerBase<TEventArgs>)obj;

        if ((e.OldValue is not null) && (trigger.Request is not null))
        {
            trigger.Request.Requested -= trigger.EventRequestOnRequested;
        }

        if ((e.NewValue is not null) && (trigger.Request is not null))
        {
            trigger.Request.Requested += trigger.EventRequestOnRequested;
        }
    }

    private void EventRequestOnRequested(object? sender, TEventArgs e)
    {
        OnEventRequest(sender, e);
    }

    protected abstract void OnEventRequest(object? sender, TEventArgs e);
}
