namespace Smart.Windows.Messaging;

public interface IReactiveMessenger
{
    IObservable<TMessage> Observe<TMessage>();

    void Send<TMessage>(TMessage message);
}
