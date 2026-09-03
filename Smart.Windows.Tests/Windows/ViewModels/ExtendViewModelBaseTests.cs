namespace Smart.Windows.ViewModels;

using Smart.Windows.Input;

public sealed class ExtendViewModelBaseTests
{
    private sealed class TestViewModel : ExtendViewModelBase
    {
        public IObserveCommand Command { get; }

        public TestViewModel(IExtendViewModelOptions? options = null)
            : base(options)
        {
            Command = MakeDelegateCommand(static () => { });
        }

        public void RaiseChanged(string name) => RaisePropertyChanged(name);
    }

    //------------------------------------------------------------------
    // ViewModel
    //------------------------------------------------------------------

    [Fact]
    public void UpdatesCommandStateOnPropertyChangedByDefault()
    {
        // Arrange
        using var viewModel = new TestViewModel();
        var count = 0;
        viewModel.Command.CanExecuteChanged += (_, _) => count++;

        // Act
        viewModel.RaiseChanged("Any");

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DoesNotUpdateCommandStateWhenAutoUpdateIsDisabled()
    {
        // Arrange
        using var viewModel = new TestViewModel(new ExtendViewModelOptions { AutoUpdateCommandState = false });
        var count = 0;
        viewModel.Command.CanExecuteChanged += (_, _) => count++;

        // Act
        viewModel.RaiseChanged("Any");

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void DefaultOptionsEnableAutoUpdate()
    {
        // Assert
        Assert.True(new ExtendViewModelOptions().AutoUpdateCommandState);
    }
}
