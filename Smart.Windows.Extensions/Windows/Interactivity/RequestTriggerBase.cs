namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Mvvm.Messaging;

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

    private bool subscribed;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.Unloaded += OnUnloaded;

        if (AssociatedObject.IsLoaded)
        {
            Subscribe();
        }
    }

    protected override void OnDetaching()
    {
        Unsubscribe();

        AssociatedObject.Loaded -= OnLoaded;
        AssociatedObject.Unloaded -= OnUnloaded;

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => Subscribe();

    private void OnUnloaded(object sender, RoutedEventArgs e) => Unsubscribe();

    private void Subscribe()
    {
        if (subscribed)
        {
            return;
        }

        subscribed = true;
        AddRequested(Request);
    }

    private void Unsubscribe()
    {
        if (!subscribed)
        {
            return;
        }

        subscribed = false;
        RemoveRequested(Request);
    }

    private void AddRequested(IEventRequest<TEventArgs>? request)
    {
        if (request is not null)
        {
            request.Requested += EventRequestOnRequested;
        }
    }

    private void RemoveRequested(IEventRequest<TEventArgs>? request)
    {
        if (request is not null)
        {
            request.Requested -= EventRequestOnRequested;
        }
    }

    private static void HandleRequestPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue)
        {
            return;
        }

        var trigger = (RequestTriggerBase<TEventArgs>)obj;
        if (!trigger.subscribed)
        {
            return;
        }

        trigger.RemoveRequested(e.OldValue as IEventRequest<TEventArgs>);
        trigger.AddRequested(e.NewValue as IEventRequest<TEventArgs>);
    }

    private void EventRequestOnRequested(object? sender, TEventArgs e)
    {
        OnEventRequest(sender, e);
    }

    protected abstract void OnEventRequest(object? sender, TEventArgs e);
}
