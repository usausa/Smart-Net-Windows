namespace Smart.Windows.Input;

using System.Windows.Input;

public sealed class AsyncCommandTest
{
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
    // Reentrancy
    //------------------------------------------------------------------

    private static void WaitUntilExecutable(ICommand command, object? parameter = null) =>
        Assert.True(SpinWait.SpinUntil(() => command.CanExecute(parameter), TimeSpan.FromSeconds(5)));

    [Fact]
    public void ExecuteIsIgnoredWhileRunning()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var count = 0;
        var command = new AsyncCommand(() =>
        {
            count++;
            return tcs.Task;
        });

        // Act
        ((ICommand)command).Execute(null);
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(1, count);

        // Completing the first execution makes the command executable again
        tcs.SetResult();
        WaitUntilExecutable(command);

        ((ICommand)command).Execute(null);
        Assert.Equal(2, count);
    }

    [Fact]
    public void CanExecuteIsFalseWhileRunning()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var command = new AsyncCommand(() => tcs.Task);

        // Act & Assert
        Assert.True(((ICommand)command).CanExecute(null));

        ((ICommand)command).Execute(null);
        Assert.False(((ICommand)command).CanExecute(null));

        tcs.SetResult();
        WaitUntilExecutable(command);
    }

    [Fact]
    public void GenericExecuteIsIgnoredWhileRunning()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var count = 0;
        var command = new AsyncCommand<int>(v =>
        {
            count += v;
            return tcs.Task;
        });

        // Act
        ((ICommand)command).Execute(1);
        ((ICommand)command).Execute(1);

        // Assert
        Assert.Equal(1, count);

        tcs.SetResult();
        WaitUntilExecutable(command, 1);

        ((ICommand)command).Execute(1);
        Assert.Equal(2, count);
    }

    //------------------------------------------------------------------
    // Exception
    //------------------------------------------------------------------

    [Fact]
    public void ExecuteDoesNotThrowWhenTaskFails()
    {
        // Arrange
        var command = new AsyncCommand(static () => Task.FromException(new InvalidOperationException("test")));

        // Act (the failure is traced, not propagated out of async void)
        ((ICommand)command).Execute(null);

        // Assert (state is restored so the command stays usable)
        WaitUntilExecutable(command);
    }

    [Fact]
    public void GenericExecuteDoesNotThrowWhenTaskFails()
    {
        // Arrange
        var command = new AsyncCommand<int>(static _ => Task.FromException(new InvalidOperationException("test")));

        // Act
        ((ICommand)command).Execute(1);

        // Assert
        WaitUntilExecutable(command, 1);
    }
}
