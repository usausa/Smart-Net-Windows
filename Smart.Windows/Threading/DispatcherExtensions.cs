namespace Smart.Threading;

using System.Windows.Threading;

public static class DispatcherExtensions
{
    public static void Invoke(this DispatcherObject dispatcher, Action action)
    {
        if (dispatcher.CheckAccess())
        {
            action();
        }
        else
        {
            dispatcher.Dispatcher.Invoke(DispatcherPriority.Normal, action);
        }
    }

    public static void Invoke<T>(this DispatcherObject dispatcher, Action<T> action, T arg)
    {
        if (dispatcher.CheckAccess())
        {
            action(arg);
        }
        else
        {
            dispatcher.Dispatcher.Invoke(DispatcherPriority.Normal, action, arg);
        }
    }

    public static TResult Invoke<TResult>(this DispatcherObject dispatcher, Func<TResult> action)
    {
        if (dispatcher.CheckAccess())
        {
            return action();
        }

        return (TResult)dispatcher.Dispatcher.Invoke(DispatcherPriority.Normal, action);
    }

    public static DispatcherOperation AsyncInvoke(this DispatcherObject dispatcher, Action action, CancellationToken cancellationToken = default)
    {
        return dispatcher.Dispatcher.InvokeAsync(action, DispatcherPriority.Normal, cancellationToken);
    }

    public static DispatcherOperation AsyncInvoke<T>(this DispatcherObject dispatcher, Action<T> action, T arg, CancellationToken cancellationToken = default)
    {
        return dispatcher.Dispatcher.InvokeAsync(() => action(arg), DispatcherPriority.Normal, cancellationToken);
    }
}
