namespace Smart.Windows.ViewModels;

using Smart.Mvvm.ViewModels;

public interface IExtendViewModelOptions : IViewModelOptions
{
    CommandBehavior CommandBehavior { get; }
}
