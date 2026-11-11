namespace Smart.Windows.Interactivity;

using System.ComponentModel;
using System.Reflection;

public sealed class CancelEventActionTests
{
    private static readonly MethodInfo InvokeMethod =
        typeof(CancelEventAction).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic, null, [typeof(object)], null)!;

    private static void Invoke(CancelEventAction action, object? parameter) =>
        InvokeMethod.Invoke(action, [parameter]);

    [Fact]
    public void SetsCancelOfCancelEventArgs()
    {
        // Arrange
        var args = new CancelEventArgs();
        var action = new CancelEventAction { Cancel = true };

        // Act
        Invoke(action, args);

        // Assert
        Assert.True(args.Cancel);
    }

    [Fact]
    public void IgnoresOtherParameter()
    {
        // Arrange
        var action = new CancelEventAction { Cancel = true };

        // Act
        var exception = Record.Exception(() => Invoke(action, EventArgs.Empty));

        // Assert
        Assert.Null(exception);
    }
}
