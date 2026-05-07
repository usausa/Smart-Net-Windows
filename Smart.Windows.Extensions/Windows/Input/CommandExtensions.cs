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

    public static IObservable<bool> CanExecuteObservable(
        this ICommand command,
        object? parameter = null)
    {
        return Observable.Create<bool>(observer =>
        {
            observer.OnNext(command.CanExecute(parameter));

            void OnCanExecuteChanged(object? sender, EventArgs e) => observer.OnNext(command.CanExecute(parameter));

            command.CanExecuteChanged += OnCanExecuteChanged;
            return () => command.CanExecuteChanged -= OnCanExecuteChanged;
        })
        .DistinctUntilChanged();
    }
}
