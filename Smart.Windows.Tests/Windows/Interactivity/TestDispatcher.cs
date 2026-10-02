namespace Smart.Windows.Interactivity;

using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

internal static class TestDispatcher
{
    public static void RunSta(Action action)
    {
        var source = new TaskCompletionSource<Dispatcher>();
        var thread = new Thread(() =>
        {
            source.SetResult(Dispatcher.CurrentDispatcher);
            Dispatcher.Run();
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        var dispatcher = source.Task.GetAwaiter().GetResult();

        try
        {
            dispatcher.Invoke(action);
        }
        finally
        {
            dispatcher.InvokeShutdown();
            thread.Join();
        }
    }

    public static void ProcessUntil(Func<bool> condition, TimeSpan timeout)
    {
        var frame = new DispatcherFrame();
        var watch = Stopwatch.StartNew();
        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
        timer.Tick += (_, _) =>
        {
            if (condition() || (watch.Elapsed >= timeout))
            {
                timer.Stop();
                frame.Continue = false;
            }
        };
        timer.Start();
        Dispatcher.PushFrame(frame);
    }
}
