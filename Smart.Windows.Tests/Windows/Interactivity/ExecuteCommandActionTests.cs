namespace Smart.Windows.Interactivity;

using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;

using Smart.Windows.Input;

public sealed class ExecuteCommandActionTests
{
    private static readonly MethodInfo InvokeMethod =
        typeof(ExecuteCommandAction).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic, null, [typeof(object)], null)!;

    private static void Invoke(ExecuteCommandAction action, object? parameter) =>
        InvokeMethod.Invoke(action, [parameter]);

    private sealed class NullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => null;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
    }

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

    //------------------------------------------------------------------
    // Action
    //------------------------------------------------------------------

    [Fact]
    public void PassesNullWhenConverterReturnsNull()
    {
        // Arrange
        var executed = false;
        object? received = "initial";
        var action = new ExecuteCommandAction
        {
            Command = new DelegateCommand<object?>(x =>
            {
                executed = true;
                received = x;
            }),
            Converter = new NullConverter()
        };

        // Act
        Invoke(action, "eventArgument");

        // Assert
        Assert.True(executed);
        Assert.Null(received);
    }
}
