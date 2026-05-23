namespace Smart.Windows.Hosting;

using System.Diagnostics.CodeAnalysis;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWpf<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this IServiceCollection services)
        where TApp : Application
    {
        services.AddSingleton<TApp>();
        services.AddHostedService<ApplicationHostingService<TApp>>();

        return services;
    }
}
