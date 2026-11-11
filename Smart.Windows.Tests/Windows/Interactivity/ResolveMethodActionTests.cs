namespace Smart.Windows.Interactivity;

using System.Reflection;

using Smart.Mvvm.Messaging;

public sealed class ResolveMethodActionTests
{
    private static readonly MethodInfo InvokeMethod =
        typeof(ResolveMethodAction).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic, null, [typeof(object)], null)!;

    private sealed class MethodTarget
    {
        private readonly string text = "described";

        public string Describe() => text;
    }

    private static void Invoke(ResolveMethodAction action, object? parameter) =>
        InvokeMethod.Invoke(action, [parameter]);

    [Fact]
    public void ResolvesMethodOfTargetObject()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };
        var args = new ResolveEventArgs();

        // Act
        Invoke(action, args);

        // Assert
        Assert.Equal("described", args.Result);
    }

    [Fact]
    public void IgnoresOtherParameter()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };

        // Act
        var exception = Record.Exception(() => Invoke(action, EventArgs.Empty));

        // Assert
        Assert.Null(exception);
    }
}
