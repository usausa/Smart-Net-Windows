namespace Smart.Windows;

using System.Reactive.Linq;
using System.Windows;

public static class UIElementExtensions
{
    public static IObservable<RoutedEventArgs> RoutedEventAsObservable(
        this UIElement source,
        RoutedEvent routedEvent,
        bool handledEventsToo = false)
    {
        return Observable.Create<RoutedEventArgs>(observer =>
        {
            RoutedEventHandler handler = (_, e) => observer.OnNext(e);
            source.AddHandler(routedEvent, handler, handledEventsToo);
            return () => source.RemoveHandler(routedEvent, handler);
        });
    }

    public static IObservable<TEventArgs> RoutedEventAsObservable<TEventArgs>(
        this UIElement source,
        RoutedEvent routedEvent,
        bool handledEventsToo = false)
        where TEventArgs : RoutedEventArgs
    {
        return Observable.Create<TEventArgs>(observer =>
        {
            RoutedEventHandler handler = (_, e) =>
            {
                if (e is TEventArgs typed)
                {
                    observer.OnNext(typed);
                }
            };
            source.AddHandler(routedEvent, handler, handledEventsToo);
            return () => source.RemoveHandler(routedEvent, handler);
        });
    }
}
