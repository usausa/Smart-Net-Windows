namespace Smart.Windows.Input;

using System.Windows.Input;

public sealed class AsyncCommandTests : IDisposable
{
    private readonly SynchronizationContext? original = SynchronizationContext.Current;

    public void Dispose() => SynchronizationContext.SetSynchronizationContext(original);

    private sealed class RecordingSynchronizationContext : SynchronizationContext
    {
        private readonly List<(SendOrPostCallback Callback, object? State)> posted = [];

        public IReadOnlyList<(SendOrPostCallback Callback, object? State)> Posted => posted;

        public override void Post(SendOrPostCallback d, object? state) => posted.Add((d, state));
    }

    //------------------------------------------------------------------
    // AsyncCommand
    //------------------------------------------------------------------

    [Fact]
    public void ExecuteInvokesDelegate()
    {
        // Arrange
        var count = 0;
        var command = new AsyncCommand(() =>
        {
            count++;
            return Task.CompletedTask;
        });

        // Act
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void CanExecuteReturnsTrueByDefault()
    {
        // Arrange
        var command = new AsyncCommand(() => Task.CompletedTask);

        // Act & Assert
        Assert.True(((ICommand)command).CanExecute(null));
    }

    [Fact]
    public void CanExecuteRespectsUserPredicate()
    {
        // Arrange
        var count = 0;
        // CanExecute reflects the user predicate, but Execute does not check it and always runs
        var command = new AsyncCommand(
            () =>
            {
                count++;
                return Task.CompletedTask;
            },
            () => false);

        // Act & Assert
        Assert.False(((ICommand)command).CanExecute(null));

        ((ICommand)command).Execute(null);
        Assert.Equal(1, count);
    }

    [Fact]
    public void CanExecuteChangedEventFiredByRaiseMethod()
    {
        // Arrange
        var command = new AsyncCommand(() => Task.CompletedTask);
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        // Act
        command.RaiseCanExecuteChanged();

        // Assert
        Assert.Equal(1, raised);
    }

    //------------------------------------------------------------------
    // AsyncCommand<T>
    //------------------------------------------------------------------

    [Fact]
    public void GenericExecuteInvokesDelegate()
    {
        // Arrange
        var count = 0;
        var command = new AsyncCommand<int>(v =>
        {
            count += v;
            return Task.CompletedTask;
        });

        // Act
        ((ICommand)command).Execute(2);

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void GenericParameterPassedToExecute()
    {
        // Arrange
        var received = string.Empty;
        var command = new AsyncCommand<string?>(v =>
        {
            received = v;
            return Task.CompletedTask;
        });

        // Act
        ((ICommand)command).Execute("hello");

        // Assert
        Assert.Equal("hello", received);
    }

    [Fact]
    public void GenericCanExecuteReturnsTrueByDefault()
    {
        // Arrange
        var command = new AsyncCommand<int>(_ => Task.CompletedTask);

        // Act & Assert
        Assert.True(((ICommand)command).CanExecute(0));
    }

    [Fact]
    public void GenericParameterPassedToCanExecute()
    {
        // Arrange
        var received = -1;
        var command = new AsyncCommand<int>(
            _ => Task.CompletedTask,
            v =>
            {
                received = v;
                return true;
            });

        // Act
        ((ICommand)command).CanExecute(42);

        // Assert
        Assert.Equal(42, received);
    }

    [Fact]
    public void GenericCanExecuteRespectsUserPredicate()
    {
        // Arrange
        var count = 0;
        // CanExecute reflects the user predicate, but Execute does not check it and always runs
        var command = new AsyncCommand<int>(
            v =>
            {
                count += v;
                return Task.CompletedTask;
            },
            _ => false);

        // Act & Assert
        Assert.False(((ICommand)command).CanExecute(1));

        ((ICommand)command).Execute(1);
        Assert.Equal(1, count);
    }

    [Fact]
    public void GenericCastNullToDefaultForValueType()
    {
        // Arrange
        var received = -1;
        var command = new AsyncCommand<int>(v =>
        {
            received = v;
            return Task.CompletedTask;
        });

        // Act
        // null parameter → value type T should receive default(int) == 0
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(0, received);
    }

    [Fact]
    public void GenericCanExecuteChangedEventFiredByRaiseMethod()
    {
        // Arrange
        var command = new AsyncCommand<int>(_ => Task.CompletedTask);
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        // Act
        command.RaiseCanExecuteChanged();

        // Assert
        Assert.Equal(1, raised);
    }

    //------------------------------------------------------------------
    // Concurrency (the command does not serialize by itself)
    //------------------------------------------------------------------

    [Fact]
    public void ExecuteAllowsConcurrentInvocation()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var count = 0;
        var command = new AsyncCommand(() =>
        {
            count++;
            return tcs.Task;
        });

        // Act (AsyncCommand keeps no execution state; exclusion must be expressed through canExecute)
        ((ICommand)command).Execute(null);
        Assert.True(((ICommand)command).CanExecute(null));
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(2, count);

        tcs.SetResult();
    }

    //------------------------------------------------------------------
    // Exception
    //------------------------------------------------------------------

    [Fact]
    public void ExecuteDoesNotThrowToCallerWhenTaskFails()
    {
        // Arrange
        var context = new RecordingSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(context);
        var command = new AsyncCommand(static () => Task.FromException(new InvalidOperationException("test")));

        // Act
        ((ICommand)command).Execute(null);

        // Assert
        var (callback, state) = Assert.Single(context.Posted);
        Assert.Throws<InvalidOperationException>(() => callback(state));
    }

    [Fact]
    public void GenericExecuteDoesNotThrowToCallerWhenTaskFails()
    {
        // Arrange
        var context = new RecordingSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(context);
        var command = new AsyncCommand<int>(static _ => Task.FromException(new InvalidOperationException("test")));

        // Act
        ((ICommand)command).Execute(1);

        // Assert
        var (callback, state) = Assert.Single(context.Posted);
        Assert.Throws<InvalidOperationException>(() => callback(state));
    }
}
