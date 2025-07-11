namespace Smart.Windows.ViewModels;

using Smart.Mvvm.ViewModels;

public class ExtendViewModelOptions : ViewModelOptions, IExtendViewModelOptions
{
    public CommandBehavior CommandBehavior { get; } = CommandBehavior.None;
}
