namespace Smart.Windows.Resolver
{
    using System;

    public sealed class DefaultResolveProvider : IResolveProvider
    {
        private Func<Type, object> defaultResolver = Activator.CreateInstance;

        public static DefaultResolveProvider Default { get; } = new DefaultResolveProvider();

        private DefaultResolveProvider()
        {
        }

        public void SetResolver(Func<Type, object> resolver)
        {
            defaultResolver = resolver;
        }

        public object Resolve(Type type)
        {
            return defaultResolver(type);
        }
    }
}
