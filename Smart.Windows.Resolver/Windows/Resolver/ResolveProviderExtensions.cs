namespace Smart.Windows.Resolver
{
    using Smart.Resolver;

    /// <summary>
    ///
    /// </summary>
    public static class ResolveProviderExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="resolver"></param>
        public static void UseSmartResolver(this ResolveProvider provider, SmartResolver resolver)
        {
            provider.Provider = new SmartResolveProvider(resolver);
        }
    }
}
