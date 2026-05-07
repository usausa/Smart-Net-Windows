namespace Smart.Windows.Input;

using System.Reactive.Linq;
using System.Windows.Input;

public static class CommandExtensions
{
    public static IObservable<EventArgs> AsObservable(this ICommand command)
    {
        return Observable.FromEvent<EventHandler, EventArgs>(
            h => (_, e) => h(e),
            h => command.CanExecuteChanged += h,
            h => command.CanExecuteChanged -= h);
    }
}
