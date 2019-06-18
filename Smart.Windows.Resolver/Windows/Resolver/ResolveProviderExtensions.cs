namespace Smart.Windows.Resolver
{
    using Smart.Resolver;

    public static class ResolveProviderExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static void UseSmartResolver(this ResolveProvider provider, SmartResolver resolver)
        {
            provider.Provider = new SmartResolveProvider(resolver);
        }
    }
}
