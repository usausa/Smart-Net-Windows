namespace Smart.Windows.Resolver
{
    using System;

    public interface IResolveProvider
    {
        object Resolve(Type type);
    }
}
