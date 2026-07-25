namespace Smart.Windows.Resolver;

using Smart.Mvvm.Resolver;

internal static class ResolveHelper
{
    public static object Resolve(Type type)
    {
        var service = ResolveProvider.Default.GetService(type);
        if (service is null)
        {
            throw new InvalidOperationException($"Failed to resolve service. type=[{type}]");
        }

        return service;
    }
}
