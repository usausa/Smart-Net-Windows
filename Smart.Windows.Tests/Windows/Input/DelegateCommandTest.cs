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
        var count = 0;
        var command = new DelegateCommand(() => count++);

        ((ICommand)command).Execute(null);

        Assert.Equal(1, count);
    }

    [Fact]
    public void CanExecuteReturnsTrueByDefault()
    {
        var command = new DelegateCommand(() => { });

        Assert.True(((ICommand)command).CanExecute(null));
    }

    [Fact]
    public void CanExecuteRespectsUserPredicate()
    {
        var allow = false;
        // ReSharper disable once AccessToModifiedClosure
        var command = new DelegateCommand(() => { }, () => allow);

        Assert.False(((ICommand)command).CanExecute(null));

        allow = true;
        Assert.True(((ICommand)command).CanExecute(null));
    }

    [Fact]
    public void CanExecuteChangedEventFiredByRaiseMethod()
    {
        var command = new DelegateCommand(() => { });
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        command.RaiseCanExecuteChanged();

        Assert.Equal(1, raised);
    }

    //------------------------------------------------------------------
    // DelegateCommand<T>
    //------------------------------------------------------------------

    [Fact]
    public void GenericExecuteInvokesDelegate()
    {
        var received = string.Empty;
        var command = new DelegateCommand<string?>(v => received = v);

        ((ICommand)command).Execute("hello");

        Assert.Equal("hello", received);
    }

    [Fact]
    public void GenericExecutePassesParameter()
    {
        var received = 0;
        var command = new DelegateCommand<int>(v => received = v);

        ((ICommand)command).Execute(42);

        Assert.Equal(42, received);
    }

    [Fact]
    public void GenericCanExecuteReturnsTrueByDefault()
    {
        var command = new DelegateCommand<int>(_ => { });

        Assert.True(((ICommand)command).CanExecute(0));
    }

    [Fact]
    public void GenericCanExecutePassesParameter()
    {
        var received = -1;
        var command = new DelegateCommand<int>(
            _ => { },
            v =>
            {
                received = v;
                return true;
            });

        ((ICommand)command).CanExecute(7);

        Assert.Equal(7, received);
    }

    [Fact]
    public void GenericCanExecuteRespectsUserPredicate()
    {
        var allow = false;
        // ReSharper disable once AccessToModifiedClosure
        var command = new DelegateCommand<int>(_ => { }, _ => allow);

        Assert.False(((ICommand)command).CanExecute(0));

        allow = true;
        Assert.True(((ICommand)command).CanExecute(0));
    }

    [Fact]
    public void GenericCastNullToDefaultForValueType()
    {
        var received = -1;
        var command = new DelegateCommand<int>(v => received = v);

        // null parameter → value type T should receive default(int) == 0
        ((ICommand)command).Execute(null);

        Assert.Equal(0, received);
    }

    [Fact]
    public void GenericCastNullForReferenceType()
    {
        var called = false;
        var command = new DelegateCommand<string?>(v =>
        {
            called = true;
            Assert.Null(v);
        });

        ((ICommand)command).Execute(null);

        Assert.True(called);
    }

    [Fact]
    public void GenericCanExecuteChangedEventFiredByRaiseMethod()
    {
        var command = new DelegateCommand<int>(_ => { });
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        command.RaiseCanExecuteChanged();

        Assert.Equal(1, raised);
    }
}
