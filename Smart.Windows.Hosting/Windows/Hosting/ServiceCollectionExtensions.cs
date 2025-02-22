namespace Smart.Windows.Hosting;

using System.Windows;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWpf<TApp>(this IServiceCollection services)
        where TApp : Application
    {
        services.AddSingleton<TApp>();
        services.AddHostedService<ApplicationHostingService<TApp>>();

        return services;
    }
}
