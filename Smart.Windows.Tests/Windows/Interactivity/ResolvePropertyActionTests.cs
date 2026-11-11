namespace Smart.Windows.Interactivity;

using System.Reflection;

using Smart.Mvvm.Messaging;

public sealed class ResolvePropertyActionTests
{
    private static readonly MethodInfo InvokeMethod =
        typeof(ResolvePropertyAction).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic, null, [typeof(object)], null)!;

    private sealed class PropertyTarget
    {
        public string Name { get; } = "target";
    }

    private static void Invoke(ResolvePropertyAction action, object? parameter) =>
        InvokeMethod.Invoke(action, [parameter]);

    [Fact]
    public void ResolvesPropertyOfTargetObject()
    {
        // Arrange
        var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Name) };
        var args = new ResolveEventArgs();

        // Act
        Invoke(action, args);

        // Assert
        Assert.Equal("target", args.Result);
    }

    [Fact]
    public void IgnoresOtherParameter()
    {
        // Arrange
        var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Name) };

        // Act
        var exception = Record.Exception(() => Invoke(action, EventArgs.Empty));

        // Assert
        Assert.Null(exception);
    }
}
