namespace Smart.Windows.ViewModels;

using Smart.Mvvm.ViewModels;

public interface IExtendViewModelOptions : IViewModelOptions
{
    CommandMode CommandMode { get; }

    bool AutoUpdateCommandState => true;
}
