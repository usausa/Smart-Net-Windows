namespace Smart.Windows.Messaging
{
    using System;

    public sealed class ResolveRequest<T> : IEventRequest<ResolveEventArgs>
    {
        public event EventHandler<ResolveEventArgs> Requested;

        public T Resolve()
        {
            var args = new ResolveEventArgs();
            Requested?.Invoke(this, args);
            return (T)args.Value;
        }
    }
}
