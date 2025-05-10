namespace Smart.Windows.Input;

using System.Windows.Input;

public interface IObserveCommand : ICommand
{
#pragma warning disable CA1030
    void RaiseCanExecuteChanged();
#pragma warning restore CA1030
}
