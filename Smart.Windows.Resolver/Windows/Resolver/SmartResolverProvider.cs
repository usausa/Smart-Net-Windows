namespace Smart.Windows.Resolver
{
    using System;

    using Smart.Resolver;

    public sealed class SmartResolverProvider : IResolveProvider
    {
        private readonly SmartResolver resolver;

        public SmartResolverProvider(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public object Resolve(Type type) => resolver.Get(type);
    }
}
