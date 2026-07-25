namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Mvvm.Messaging;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class MessageTrigger : TriggerBase<FrameworkElement>
{
    public static readonly DependencyProperty MessengerProperty = DependencyProperty.Register(
        nameof(Messenger),
        typeof(IMessenger),
        typeof(MessageTrigger),
        new PropertyMetadata(HandleMessengerPropertyChanged));

    public IMessenger? Messenger
    {
        get => (IMessenger)GetValue(MessengerProperty);
        set => SetValue(MessengerProperty, value);
    }

    public string? Label { get; set; }

    public Type? MessageType { get; set; }

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
        AddReceived(Messenger);
    }

    private void Unsubscribe()
    {
        if (!subscribed)
        {
            return;
        }

        subscribed = false;
        RemoveReceived(Messenger);
    }

    private void AddReceived(IMessenger? messenger)
    {
        if (messenger is not null)
        {
            messenger.Received += MessengerOnReceived;
        }
    }

    private void RemoveReceived(IMessenger? messenger)
    {
        if (messenger is not null)
        {
            messenger.Received -= MessengerOnReceived;
        }
    }

    private static void HandleMessengerPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue)
        {
            return;
        }

        var trigger = (MessageTrigger)obj;
        if (!trigger.subscribed)
        {
            return;
        }

        trigger.RemoveReceived(e.OldValue as IMessenger);
        trigger.AddReceived(e.NewValue as IMessenger);
    }

    private void MessengerOnReceived(object? sender, MessengerEventArgs e)
    {
        if (((Label is null) || Label.Equals(e.Label, StringComparison.Ordinal)) &&
            ((MessageType is null) || ((e.MessageType is not null) && MessageType.IsAssignableFrom(e.MessageType))))
        {
            InvokeActions(e.Message);
        }
    }
}
