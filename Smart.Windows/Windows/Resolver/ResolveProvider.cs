namespace Smart.Windows.Resolver;

public sealed class ResolveProvider : IResolveProvider
{
    public static ResolveProvider Default { get; } = new();

    public IResolveProvider Provider { get; set; } = DefaultResolveProvider.Default;

    public object? Resolve(Type type) => Provider.Resolve(type);
}
