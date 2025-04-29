namespace Smart.Windows.Messaging;

using System.ComponentModel;

public sealed class ResolveRequest<T> : IEventRequest<ResolveEventArgs>
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler<ResolveEventArgs>? Requested;

    public T Resolve()
    {
        var args = new ResolveEventArgs();
        Requested?.Invoke(this, args);
        return (T)args.Result!;
    }
}
