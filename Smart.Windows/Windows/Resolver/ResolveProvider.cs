namespace Smart.Windows.Resolver
{
    using System;

    public sealed class ResolveProvider : IResolveProvider
    {
        public static ResolveProvider Default { get; } = new ResolveProvider();

        public IResolveProvider Provider { get; set; } = DefaultResolveProvider.Default;

        public object Resolve(Type type)
        {
            return Provider.Resolve(type);
        }
    }
}
