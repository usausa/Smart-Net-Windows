namespace Smart.Windows.Messaging;

public sealed class EventRequest : IEventRequest<ParameterEventArgs>
{
    private static readonly ParameterEventArgs EmptyArgs = new(null);

    public event EventHandler<ParameterEventArgs>? Requested;

    public void Request()
    {
        Requested?.Invoke(this, EmptyArgs);
    }
}

public sealed class EventRequest<T> : IEventRequest<ParameterEventArgs>
{
    public event EventHandler<ParameterEventArgs>? Requested;

    public void Request(T value)
    {
        Requested?.Invoke(this, new ParameterEventArgs(value));
    }
}
