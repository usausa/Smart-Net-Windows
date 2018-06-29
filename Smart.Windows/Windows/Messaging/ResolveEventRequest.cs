namespace Smart.Windows.Messaging
{
    using System;

    public sealed class ResolveEventRequest<T> : IEventRequest<ValueHolderEventArgs>
    {
        public event EventHandler<ValueHolderEventArgs> Requested;

        public T Resolve()
        {
            var args = new ValueHolderEventArgs();
            Requested?.Invoke(this, args);
            return (T)args.Value;
        }
    }
}
