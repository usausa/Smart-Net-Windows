namespace Smart.Windows.Messaging;

using System.ComponentModel;

public sealed class Messenger : IMessenger
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler<MessengerEventArgs>? Received;

    public void Send(string label)
    {
        Received?.Invoke(this, new MessengerEventArgs(label, typeof(object), null));
    }

    public void Send<T>(T message)
    {
        Received?.Invoke(this, new MessengerEventArgs(string.Empty, typeof(T), message));
    }

    public void Send<T>(string label, T parameter)
    {
        Received?.Invoke(this, new MessengerEventArgs(label, typeof(T), parameter));
    }
}
