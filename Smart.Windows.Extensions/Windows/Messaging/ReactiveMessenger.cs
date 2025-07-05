namespace Smart.Windows.Messaging;

using System.Reactive.Linq;
using System.Reactive.Subjects;

public class ReactiveMessenger : IReactiveMessenger
{
    public static ReactiveMessenger Default { get; } = new();

    private static class SubjectHolder<T>
    {
        public static readonly Subject<T> Subject = new();
    }

    private ReactiveMessenger()
    {
    }

    public IObservable<TMessage> Observe<TMessage>()
    {
        var subject = SubjectHolder<TMessage>.Subject;
        return subject.AsObservable();
    }

    public void Send<TMessage>(TMessage message)
    {
        var subject = SubjectHolder<TMessage>.Subject;
        subject.OnNext(message);
    }

#pragma warning disable CA1822
    public bool HasObservers<TMessage>()
    {
        var subject = SubjectHolder<TMessage>.Subject;
        return subject.HasObservers;
    }
#pragma warning restore CA1822
}
