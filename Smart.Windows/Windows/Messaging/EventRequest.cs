namespace Smart.Windows.Messaging
{
    using System;

    public sealed class EventRequest : IEventRequest<EventArgs>
    {
        public event EventHandler<EventArgs> Requested;

        public void Request(EventArgs args)
        {
            Requested?.Invoke(this, args);
        }
    }

    public sealed class EventRequest<TEventAgrs> : IEventRequest<TEventAgrs>
        where TEventAgrs : EventArgs
    {
        public event EventHandler<TEventAgrs> Requested;

        public void Request(TEventAgrs args)
        {
            Requested?.Invoke(this, args);
        }
    }
}