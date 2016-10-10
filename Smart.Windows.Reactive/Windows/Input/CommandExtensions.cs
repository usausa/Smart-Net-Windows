namespace Smart.Windows.Input
{
    using System;
    using System.Reactive.Linq;
    using System.Windows.Input;

    /// <summary>
    ///
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static IObservable<EventArgs> AsObservable(this ICommand command)
        {
            return Observable.FromEvent<EventHandler, EventArgs>(
                h => (sender, e) => h(e),
                h => command.CanExecuteChanged += h,
                h => command.CanExecuteChanged -= h);
        }
    }
}
