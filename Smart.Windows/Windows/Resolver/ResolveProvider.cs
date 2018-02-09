namespace Smart.Windows.Resolver
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public sealed class ResolveProvider : IResolveProvider
    {
        /// <summary>
        ///
        /// </summary>
        public static ResolveProvider Default { get; } = new ResolveProvider();

        /// <summary>
        ///
        /// </summary>
        public IResolveProvider Provider { get; set; } = DefaultResolveProvider.Default;

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return Provider.Resolve(type);
        }
    }
}
