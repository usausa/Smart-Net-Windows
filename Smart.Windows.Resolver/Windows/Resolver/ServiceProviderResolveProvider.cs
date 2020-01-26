namespace Smart.Windows.Resolver
{
    using System;

    public sealed class ServiceProviderResolveProvider : IResolveProvider
    {
        private readonly IServiceProvider provider;

        public ServiceProviderResolveProvider(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object Resolve(Type type) => provider.GetService(type);
    }
}
