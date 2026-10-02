namespace Smart.Windows.ViewModels;

using Smart.Mvvm.ViewModels;
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

    private sealed class ModeViewModel : ExtendViewModelBase
    {
        public ModeViewModel(CommandMode mode = CommandMode.Standard)
            : base(new ExtendViewModelOptions { BusyState = new BusyState(), CommandMode = mode })
        {
        }

        public bool Accepts
        {
            get => AcceptsCommand;
            set => AcceptsCommand = value;
        }

        public IObserveCommand MakeDelegate(Action execute) =>
            MakeDelegateCommand(execute);

        public IObserveCommand MakeDelegate(CommandMode mode, Action execute) =>
            MakeDelegateCommand(mode, execute);

        public IObserveCommand MakeDelegateWithParameter(Action<int> execute) =>
            MakeDelegateCommand(execute);

        public IObserveCommand MakeDelegateWithParameter(CommandMode mode, Action<int> execute) =>
            MakeDelegateCommand(mode, execute);

        public IObserveCommand MakeAsync(Func<Task> execute) =>
            MakeAsyncCommand(execute);

        public IObserveCommand MakeAsync(CommandMode mode, Func<Task> execute) =>
            MakeAsyncCommand(mode, execute);

        public IObserveCommand MakeAsyncWithParameter(Func<int, Task> execute) =>
            MakeAsyncCommand(execute);

        public IObserveCommand MakeAsyncWithParameter(CommandMode mode, Func<int, Task> execute) =>
            MakeAsyncCommand(mode, execute);

        public void RaiseChanged(string name) => RaisePropertyChanged(name);
    }

    private static async Task WaitUntilAsync(Func<bool> condition)
    {
        for (var i = 0; (i < 100) && !condition(); i++)
        {
            await Task.Delay(10, TestContext.Current.CancellationToken).ConfigureAwait(true);
        }
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

    //------------------------------------------------------------------
    // Standard
    //------------------------------------------------------------------

    [Fact]
    public void StandardExecutesWithBusyState()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var busy = false;
        var command = viewModel.MakeDelegate(() =>
        {
            count++;
            // ReSharper disable once AccessToDisposedClosure
            busy = viewModel.BusyState.IsBusy;
        });

        // Act
        command.Execute(null);

        // Assert
        Assert.Equal(1, count);
        Assert.True(busy);
        Assert.False(viewModel.BusyState.IsBusy);
    }

    [Fact]
    public void StandardSkipsWhenCommandNotAccepted()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++);
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeDelegateWithParameter(_ => parameterCount++);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);

        // Assert
        Assert.Equal(0, count);
        Assert.Equal(0, parameterCount);
    }

    [Fact]
    public void StandardSkipsWhenBusy()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++);

        // Act
        using (viewModel.BusyState.Begin())
        {
            command.Execute(null);
        }

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task StandardAsyncHoldsBusyStateWhileExecuting()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var completion = new TaskCompletionSource();
        var command = viewModel.MakeAsync(() => completion.Task);

        // Act
        command.Execute(null);

        // Assert
        Assert.True(viewModel.BusyState.IsBusy);

        // Act
        completion.SetResult();
        await WaitUntilAsync(() => !viewModel.BusyState.IsBusy);

        // Assert
        Assert.False(viewModel.BusyState.IsBusy);
    }

    //------------------------------------------------------------------
    // ControlByBusyState
    //------------------------------------------------------------------

    [Fact]
    public void ControlByBusyStateDisablesWhileBusy()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var command = viewModel.MakeDelegate(CommandMode.ControlByBusyState, static () => { });

        // Assert
        Assert.True(command.CanExecute(null));
        using (viewModel.BusyState.Begin())
        {
            Assert.False(command.CanExecute(null));
        }
    }

    [Fact]
    public void ControlByBusyStateSkipsWhenCommandNotAccepted()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(CommandMode.ControlByBusyState, () => count++);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);

        // Assert
        Assert.Equal(0, count);
    }

    //------------------------------------------------------------------
    // Simple
    //------------------------------------------------------------------

    [Fact]
    public void SimpleExecutesWithoutBusyState()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var busy = true;
        var command = viewModel.MakeDelegate(CommandMode.Simple, () =>
        {
            count++;
            // ReSharper disable once AccessToDisposedClosure
            busy = viewModel.BusyState.IsBusy;
        });
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeDelegateWithParameter(CommandMode.Simple, _ => parameterCount++);

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);

        // Assert
        Assert.Equal(1, count);
        Assert.False(busy);
        Assert.Equal(1, parameterCount);
    }

    [Fact]
    public void SimpleDoesNotExecuteWhenNotAccepted()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(CommandMode.Simple, () => count++);
        var parameterCommand = viewModel.MakeDelegateWithParameter(CommandMode.Simple, _ => count++);
        var asyncCommand = viewModel.MakeAsync(CommandMode.Simple, () =>
        {
            count++;
            return Task.CompletedTask;
        });
        var asyncParameterCommand = viewModel.MakeAsyncWithParameter(CommandMode.Simple, _ =>
        {
            count++;
            return Task.CompletedTask;
        });
        viewModel.Accepts = false;

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);
        asyncCommand.Execute(null);
        asyncParameterCommand.Execute(1);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void SimpleExecutesWhileBusy()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(CommandMode.Simple, () => count++);

        // Act
        using (viewModel.BusyState.Begin())
        {
            command.Execute(null);
        }

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void SimpleAsyncDoesNotHoldBusyState()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var completion = new TaskCompletionSource();
        var command = viewModel.MakeAsync(CommandMode.Simple, () => completion.Task);
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeAsyncWithParameter(CommandMode.Simple, _ =>
        {
            parameterCount++;
            return Task.CompletedTask;
        });

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);

        // Assert
        Assert.False(viewModel.BusyState.IsBusy);
        Assert.Equal(1, parameterCount);

        completion.SetResult();
    }

    [Fact]
    public void SimpleUpdatesCommandState()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var command = viewModel.MakeDelegate(CommandMode.Simple, static () => { });
        var count = 0;
        command.CanExecuteChanged += (_, _) => count++;

        // Act
        viewModel.RaiseChanged("Any");

        // Assert
        Assert.Equal(1, count);
    }

    //------------------------------------------------------------------
    // Nested
    //------------------------------------------------------------------

    [Fact]
    public void StandardSkipsCommandRaisedInsideCommand()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var inner = viewModel.MakeDelegate(() => count++);
        var outer = viewModel.MakeDelegate(() => inner.Execute(null));

        // Act
        outer.Execute(null);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void SimpleExecutesCommandRaisedInsideCommand()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var inner = viewModel.MakeDelegate(CommandMode.Simple, () => count++);
        var outer = viewModel.MakeDelegate(() => inner.Execute(null));

        // Act
        outer.Execute(null);

        // Assert
        Assert.Equal(1, count);
    }

    //------------------------------------------------------------------
    // Default
    //------------------------------------------------------------------

    [Fact]
    public void DefaultUsesModeOfOptions()
    {
        // Arrange
        using var viewModel = new ModeViewModel(CommandMode.Simple);
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++);

        // Act
        using (viewModel.BusyState.Begin())
        {
            command.Execute(null);
        }

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DefaultOptionsUseStandard()
    {
        // Assert
        Assert.Equal(CommandMode.Standard, new ExtendViewModelOptions().CommandMode);
    }
}
