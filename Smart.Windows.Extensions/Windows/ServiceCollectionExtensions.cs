namespace Smart.Windows;

using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWindowsServices(this IServiceCollection services)
    {
        services.AddSingleton<Dispatcher>(_ => Application.Current.Dispatcher);
        return services;
    }
}
