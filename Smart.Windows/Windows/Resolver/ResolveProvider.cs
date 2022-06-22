namespace Smart.Windows.Resolver;

public sealed class ResolveProvider : IServiceProvider
{
    public static ResolveProvider Default { get; } = new();

    public IServiceProvider Provider { get; set; } = DefaultResolveProvider.Default;

    public object? GetService(Type serviceType) => Provider.GetService(serviceType);
}
