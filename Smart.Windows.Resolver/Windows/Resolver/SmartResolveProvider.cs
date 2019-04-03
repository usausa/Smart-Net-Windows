namespace Smart.Windows.Resolver
{
    using System;

    using Smart.Resolver;

    public sealed class SmartResolveProvider : IResolveProvider
    {
        private readonly SmartResolver resolver;

        public SmartResolveProvider(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public object Resolve(Type type)
        {
            return resolver.Get(type);
        }
    }
}
