namespace Smart.Windows.Resolver;

public sealed class ServiceProviderResolveProvider : IResolveProvider
{
    private readonly IServiceProvider provider;

    public ServiceProviderResolveProvider(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public object? Resolve(Type type) => provider.GetService(type);
}
