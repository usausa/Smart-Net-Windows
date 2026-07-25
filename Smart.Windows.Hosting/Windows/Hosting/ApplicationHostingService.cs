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

    private readonly TaskCompletionSource tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public ApplicationHostingService(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.serviceProvider = serviceProvider;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var thread = new Thread(() =>
        {
#pragma warning disable CA1031
            try
            {
                var app = serviceProvider.GetRequiredService<TApp>();
                using var registration = stoppingToken.Register(() => app.Dispatcher.InvokeAsync(app.Shutdown));

                app.Run();
                tcs.TrySetResult();
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            finally
            {
                hostApplicationLifetime.StopApplication();
            }
#pragma warning restore CA1031
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }
}
#pragma warning restore CA1812
