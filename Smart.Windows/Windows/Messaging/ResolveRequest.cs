namespace Smart.Windows.Messaging;

public sealed class ResolveRequest<T> : IEventRequest<ResolveEventArgs>
{
    public event EventHandler<ResolveEventArgs>? Requested;

    public T Resolve()
    {
        var args = new ResolveEventArgs();
        Requested?.Invoke(this, args);
        return (T)args.Result!;
    }
}
