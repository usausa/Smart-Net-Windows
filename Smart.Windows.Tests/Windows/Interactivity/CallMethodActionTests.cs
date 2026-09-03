namespace Smart.Windows.Interactivity;

using System.Reflection;

public sealed class CallMethodActionTests
{
    private static readonly MethodInfo InvokeMethod =
        typeof(CallMethodAction).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic, null, [typeof(object)], null)!;

    private static void Invoke(CallMethodAction action, object? parameter) =>
        InvokeMethod.Invoke(action, [parameter]);

    private interface IArgument;

    private sealed class Argument : IArgument;

    private sealed class MethodTarget
    {
        public bool InterfaceCalled { get; private set; }

        public object? InterfaceReceived { get; private set; }

        public bool ObjectCalled { get; private set; }

        public object? ObjectReceived { get; private set; }

        public void InvokeInterface(IArgument argument)
        {
            InterfaceCalled = true;
            InterfaceReceived = argument;
        }

        public void InvokeObject(object argument)
        {
            ObjectCalled = true;
            ObjectReceived = argument;
        }
    }

    //------------------------------------------------------------------
    // Action
    //------------------------------------------------------------------

    [Fact]
    public void ResolvesMethodWhenArgumentIsAssignableToParameterType()
    {
        // Arrange
        var target = new MethodTarget();
        var argument = new Argument();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeInterface),
            MethodParameter = argument
        };

        // Act
        Invoke(action, null);

        // Assert
        Assert.True(target.InterfaceCalled);
        Assert.Same(argument, target.InterfaceReceived);
    }

    [Fact]
    public void PassesEventParameterWhenMethodParameterIsNotSet()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeObject)
        };

        // Act
        Invoke(action, "eventArgument");

        // Assert
        Assert.True(target.ObjectCalled);
        Assert.Equal("eventArgument", target.ObjectReceived);
    }

    [Fact]
    public void PassesMethodParameterWhenSet()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeObject),
            MethodParameter = "methodParameter"
        };

        // Act
        Invoke(action, "eventArgument");

        // Assert
        Assert.True(target.ObjectCalled);
        Assert.Equal("methodParameter", target.ObjectReceived);
    }
}
