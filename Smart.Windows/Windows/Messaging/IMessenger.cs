namespace Smart.Windows.Messaging;

using System;

public interface IMessenger
{
    event EventHandler<MessengerEventArgs> Received;

    void Send(string label);

    void Send<T>(T message);

    void Send<T>(string label, T parameter);
}
