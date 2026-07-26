namespace Smart.Windows.Messaging;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public sealed class ReactiveMessenger : IReactiveMessenger, IDisposable
{
    public static ReactiveMessenger Default { get; } = new();

    private sealed class SubjectHolder<TMessage> : IDisposable
    {
        public Subject<TMessage> Subject { get; } = new();

        public void Dispose()
        {
            Subject.OnCompleted();
            Subject.Dispose();
        }
    }

    private readonly ConcurrentDictionary<Type, IDisposable> holders = new();

    private bool disposed;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;

        foreach (var holder in holders.Values)
        {
#pragma warning disable CA1031
            try
            {
                holder.Dispose();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Dispose failed. type=[{holder.GetType()}], message=[{ex.Message}]");
            }
#pragma warning restore CA1031
        }

        holders.Clear();
    }

    public IObservable<TMessage> Observe<TMessage>()
    {
        return GetHolder<TMessage>().Subject.AsObservable();
    }

    public void Send<TMessage>(TMessage message)
    {
        GetHolder<TMessage>().Subject.OnNext(message);
    }

    public bool HasObservers<TMessage>()
    {
        return GetHolder<TMessage>().Subject.HasObservers;
    }

    private SubjectHolder<TMessage> GetHolder<TMessage>()
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        return (SubjectHolder<TMessage>)holders.GetOrAdd(typeof(TMessage), static _ => new SubjectHolder<TMessage>());
    }
}
