namespace Smart.Windows.Resolver
{
    using Smart.Resolver;

    public static class ResolveProviderExtensions
    {
        public static void UseSmartResolver(this ResolveProvider provider, SmartResolver resolver)
        {
            provider.Provider = new SmartResolveProvider(resolver);
        }
    }
}
