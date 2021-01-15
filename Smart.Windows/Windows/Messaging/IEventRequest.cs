namespace Smart.Windows.Messaging
{
    using System;

    public interface IEventRequest<TEventArgs>
        where TEventArgs : EventArgs
    {
        event EventHandler<TEventArgs> Requested;
    }
}
