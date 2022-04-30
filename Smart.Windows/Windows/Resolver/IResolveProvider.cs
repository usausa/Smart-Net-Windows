namespace Smart.Windows.Resolver;

public interface IResolveProvider
{
    object? Resolve(Type type);
}
