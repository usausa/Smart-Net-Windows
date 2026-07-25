namespace Smart.Windows.Interactivity;

using System.Windows.Input;

using Smart.Windows.Input;

public sealed class ExecuteCommandActionTest
{
    //------------------------------------------------------------------
    // DependencyProperty
    //------------------------------------------------------------------

    [Fact]
    public void CommandPropertyIsRegisteredAsICommand()
    {
        Assert.Equal(typeof(ICommand), ExecuteCommandAction.CommandProperty.PropertyType);
    }

    [Fact]
    public void CommandParameterPropertyIsRegisteredAsObject()
    {
        Assert.Equal(typeof(object), ExecuteCommandAction.CommandParameterProperty.PropertyType);
    }

    //------------------------------------------------------------------
    // Assignment
    //------------------------------------------------------------------

    [Fact]
    public void CommandAcceptsICommandValue()
    {
        // Arrange
        var action = new ExecuteCommandAction();
        var command = new DelegateCommand(static () => { });

        // Act
        action.Command = command;

        // Assert
        Assert.Same(command, action.Command);
    }

    [Fact]
    public void CommandParameterAcceptsArbitraryObject()
    {
        // Arrange & Act
        var action = new ExecuteCommandAction { CommandParameter = "parameter" };

        // Assert
        Assert.Equal("parameter", action.CommandParameter as string);
    }
}
