namespace Smart.Windows.Resolver;

public sealed class DefaultResolveProvider : IResolveProvider
{
    private Func<Type, object?> defaultResolver = Activator.CreateInstance;

    public static DefaultResolveProvider Default { get; } = new();

    private DefaultResolveProvider()
    {
    }

    public void SetResolver(Func<Type, object?> resolver)
    {
        defaultResolver = resolver;
    }

    public object? Resolve(Type type) => defaultResolver(type);
}
