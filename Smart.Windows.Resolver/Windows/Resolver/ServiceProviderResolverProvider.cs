namespace Smart.Windows.Resolver
{
    using System;

    public sealed class ServiceProviderResolverProvider : IResolveProvider
    {
        private readonly IServiceProvider provider;

        public ServiceProviderResolverProvider(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object Resolve(Type type) => provider.GetService(type);
    }
}
