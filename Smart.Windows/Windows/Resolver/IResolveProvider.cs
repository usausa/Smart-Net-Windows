namespace Smart.Windows.Resolver
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IResolveProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);
    }
}
