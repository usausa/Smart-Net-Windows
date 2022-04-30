namespace Smart.Windows.Messaging;

public sealed class MessengerEventArgs : EventArgs
{
    public string Label { get; }

    public Type? MessageType { get; }

    public object? Message { get; }

    public MessengerEventArgs(string label, Type? messageType, object? message)
    {
        Label = label;
        MessageType = messageType;
        Message = message;
    }
}
