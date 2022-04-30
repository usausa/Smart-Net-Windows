namespace Smart.Windows.Messaging;

public interface IEventRequest<TEventArgs>
    where TEventArgs : EventArgs
{
    event EventHandler<TEventArgs> Requested;
}
