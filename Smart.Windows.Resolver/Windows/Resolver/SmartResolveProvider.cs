namespace Smart.Windows.Resolver
{
    using System;

    using Smart.Resolver;

    /// <summary>
    ///
    /// </summary>
    public class SmartResolveProvider : IResolveProvider
    {
        private readonly SmartResolver resolver;

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
        public SmartResolveProvider(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return resolver.Get(type);
        }
    }
}
