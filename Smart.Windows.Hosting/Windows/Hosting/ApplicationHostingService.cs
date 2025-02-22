namespace Smart.Windows.Hosting;

using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#pragma warning disable CA1812
internal sealed class ApplicationHostingService<TApp> : BackgroundService
    where TApp : Application
{
    private readonly IServiceProvider serviceProvider;

    private readonly IHostApplicationLifetime hostApplicationLifetime;

    private readonly TaskCompletionSource tcs = new();

    public ApplicationHostingService(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.serviceProvider = serviceProvider;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var thread = new Thread(() =>
        {
            var app = serviceProvider.GetRequiredService<TApp>();
            app.Run();
            tcs.SetResult();
            hostApplicationLifetime.StopApplication();
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }
}
#pragma warning restore CA1812
