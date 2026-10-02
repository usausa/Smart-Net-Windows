namespace Smart.Windows.ViewModels;

using Smart.Mvvm.ViewModels;

public class ExtendViewModelOptions : ViewModelOptions, IExtendViewModelOptions
{
    public CommandMode CommandMode { get; init; } = CommandMode.Standard;

    public bool AutoUpdateCommandState { get; init; } = true;
}
