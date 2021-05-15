namespace Smart.Windows.Resolver
{
    using System;

    using Smart.Resolver;

    public static class ResolveProviderExtensions
    {
        public static void UseSmartResolver(this ResolveProvider provider, SmartResolver resolver)
        {
            provider.Provider = new SmartResolveProvider(resolver);
        }

        public static void UseServiceProvider(this ResolveProvider provider, IServiceProvider services)
        {
            provider.Provider = new ServiceProviderResolveProvider(services);
        }
    }
}
