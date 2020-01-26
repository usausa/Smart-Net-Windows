namespace Smart.Windows.Resolver
{
    using System;

    public sealed class ServiceContainerResolverProvider : IResolveProvider
    {
        private readonly IServiceProvider provider;

        public ServiceContainerResolverProvider(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object Resolve(Type type) => provider.GetService(type);
    }
}
