namespace Smart.Windows.Input;

using System.Windows.Input;

public sealed class DelegateCommandTest
{
    //------------------------------------------------------------------
    // DelegateCommand
    //------------------------------------------------------------------

    [Fact]
    public void ExecuteInvokesDelegate()
    {
        // Arrange
        var count = 0;
        var command = new DelegateCommand(() => count++);

        // Act
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void CanExecuteReturnsTrueByDefault()
    {
        // Arrange
        var command = new DelegateCommand(() => { });

        // Act & Assert
        Assert.True(((ICommand)command).CanExecute(null));
    }

    [Fact]
    public void CanExecuteRespectsUserPredicate()
    {
        // Arrange
        var allow = false;
        // ReSharper disable once AccessToModifiedClosure
        var command = new DelegateCommand(() => { }, () => allow);

        // Act & Assert
        Assert.False(((ICommand)command).CanExecute(null));

        allow = true;
        Assert.True(((ICommand)command).CanExecute(null));
    }

    [Fact]
    public void CanExecuteChangedEventFiredByRaiseMethod()
    {
        // Arrange
        var command = new DelegateCommand(() => { });
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        // Act
        command.RaiseCanExecuteChanged();

        // Assert
        Assert.Equal(1, raised);
    }

    //------------------------------------------------------------------
    // DelegateCommand<T>
    //------------------------------------------------------------------

    [Fact]
    public void GenericExecuteInvokesDelegate()
    {
        // Arrange
        var received = string.Empty;
        var command = new DelegateCommand<string?>(v => received = v);

        // Act
        ((ICommand)command).Execute("hello");

        // Assert
        Assert.Equal("hello", received);
    }

    [Fact]
    public void GenericExecutePassesParameter()
    {
        // Arrange
        var received = 0;
        var command = new DelegateCommand<int>(v => received = v);

        // Act
        ((ICommand)command).Execute(42);

        // Assert
        Assert.Equal(42, received);
    }

    [Fact]
    public void GenericCanExecuteReturnsTrueByDefault()
    {
        // Arrange
        var command = new DelegateCommand<int>(_ => { });

        // Act & Assert
        Assert.True(((ICommand)command).CanExecute(0));
    }

    [Fact]
    public void GenericCanExecutePassesParameter()
    {
        // Arrange
        var received = -1;
        var command = new DelegateCommand<int>(
            _ => { },
            v =>
            {
                received = v;
                return true;
            });

        // Act
        ((ICommand)command).CanExecute(7);

        // Assert
        Assert.Equal(7, received);
    }

    [Fact]
    public void GenericCanExecuteRespectsUserPredicate()
    {
        // Arrange
        var allow = false;
        // ReSharper disable once AccessToModifiedClosure
        var command = new DelegateCommand<int>(_ => { }, _ => allow);

        // Act & Assert
        Assert.False(((ICommand)command).CanExecute(0));

        allow = true;
        Assert.True(((ICommand)command).CanExecute(0));
    }

    [Fact]
    public void GenericCastNullToDefaultForValueType()
    {
        // Arrange
        var received = -1;
        var command = new DelegateCommand<int>(v => received = v);

        // Act
        // null parameter → value type T should receive default(int) == 0
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(0, received);
    }

    [Fact]
    public void GenericCastNullForReferenceType()
    {
        // Arrange
        var called = false;
        var command = new DelegateCommand<string?>(v =>
        {
            called = true;
            Assert.Null(v);
        });

        // Act
        ((ICommand)command).Execute(null);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void GenericCanExecuteChangedEventFiredByRaiseMethod()
    {
        // Arrange
        var command = new DelegateCommand<int>(_ => { });
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        // Act
        command.RaiseCanExecuteChanged();

        // Assert
        Assert.Equal(1, raised);
    }
}
