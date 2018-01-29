namespace Smart.Windows.Resolver
{
    using System;

    public static class ResolveProvider
    {
        private static Func<Type, object> defaultResolver = Activator.CreateInstance;

        public static void SetResolver(Func<Type, object> resolver)
        {
            defaultResolver = resolver;
        }

        public static object Resolve(Type type)
        {
            return defaultResolver(type);
        }
    }
}
