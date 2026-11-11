namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

using Smart.Windows.Expressions;

public sealed class CompareTriggerTests
{
    private sealed class RecordAction : TriggerAction<DependencyObject>
    {
        public List<(object? Element, object? Parameter)> Calls { get; } = [];

        protected override void Invoke(object parameter) => Calls.Add((AssociatedObject, parameter));
    }

    private static (Border Element, CompareTrigger Trigger, RecordAction Action) Create()
    {
        var element = new Border();
        var trigger = new CompareTrigger();
        var action = new RecordAction();
        trigger.Actions.Add(action);
        Interaction.GetTriggers(element).Add(trigger);
        return (element, trigger, action);
    }

    [Fact]
    public void ExecutesActionsWhenBindingEqualsParameter()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (element, trigger, action) = Create();
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 5;

            // Assert
            var (target, parameter) = Assert.Single(action.Calls);
            Assert.Same(element, target);
            Assert.Null(parameter);
        });
    }

    [Fact]
    public void DoesNotExecuteActionsWhenBindingDiffersFromParameter()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (_, trigger, action) = Create();
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 6;

            // Assert
            Assert.Empty(action.Calls);
        });
    }

    [Fact]
    public void UsesExpression()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (_, trigger, action) = Create();
            trigger.Expression = CompareExpressions.GreaterThan;
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 6;

            // Assert
            Assert.Single(action.Calls);
        });
    }

    [Fact]
    public void ReevaluatesWhenExpressionChanges()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (_, trigger, action) = Create();
            trigger.Parameter = 5;
            trigger.Binding = 6;

            // Act
            trigger.Expression = CompareExpressions.GreaterThan;

            // Assert
            Assert.Single(action.Calls);
        });
    }

    [Fact]
    public void DoesNotExecuteActionsWhenDetached()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var (element, trigger, action) = Create();
            Interaction.GetTriggers(element).Remove(trigger);
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 5;

            // Assert
            Assert.Empty(action.Calls);
        });
    }

    [Fact]
    public void ExpressionIsEqualByDefault()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange & Act
            var trigger = new CompareTrigger();

            // Assert
            Assert.Same(CompareExpressions.Equal, trigger.Expression);
        });
    }
}
