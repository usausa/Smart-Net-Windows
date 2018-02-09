namespace Smart.Windows.Resolver
{
    using System;

    public sealed class DefaultResolveProvider : IResolveProvider
    {
        private Func<Type, object> defaultResolver = Activator.CreateInstance;

        /// <summary>
        ///
        /// </summary>
        public static DefaultResolveProvider Default { get; } = new DefaultResolveProvider();

        /// <summary>
        ///
        /// </summary>
        private DefaultResolveProvider()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
        public void SetResolver(Func<Type, object> resolver)
        {
            defaultResolver = resolver;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return defaultResolver(type);
        }
    }
}
