namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

public sealed class DefaultFocusBehaviorTests
{
    private static TextBox Target(bool isTarget = true)
    {
        var textBox = new TextBox();
        DefaultFocus.SetIsTarget(textBox, isTarget);
        return textBox;
    }

    private static void RaiseLoaded(FrameworkElement element) =>
        element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, element));

    private static IInputElement? Focused(DependencyObject element) =>
        FocusManager.GetFocusedElement(FocusManager.GetFocusScope(element));

    [Fact]
    public void FocusesAssociatedElement()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            var panel = new StackPanel { Children = { textBox } };

            // Act
            Interaction.GetBehaviors(textBox).Add(new DefaultFocusBehavior());

            // Assert
            Assert.Same(textBox, Focused(panel));
        });
    }

    [Fact]
    public void FocusesFirstTargetWhenLoaded()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var first = Target();
            var panel = new StackPanel();
            Interaction.GetBehaviors(panel).Add(new DefaultFocusBehavior());
            panel.Children.Add(new TextBox());
            panel.Children.Add(Target(false));
            panel.Children.Add(first);
            panel.Children.Add(Target());

            // Act
            RaiseLoaded(panel);

            // Assert
            Assert.Same(first, Focused(panel));
        });
    }

    [Fact]
    public void SkipsDisabledAndHiddenTargets()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var disabled = Target();
            disabled.IsEnabled = false;
            var hidden = Target();
            hidden.Visibility = Visibility.Collapsed;
            var expected = Target();
            var panel = new StackPanel { Children = { disabled, hidden, expected } };
            Interaction.GetBehaviors(panel).Add(new DefaultFocusBehavior());

            // Act
            RaiseLoaded(panel);

            // Assert
            Assert.Same(expected, Focused(panel));
        });
    }

    [Fact]
    public void SkipsTargetsInsideHiddenContainer()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var hidden = new StackPanel { Visibility = Visibility.Collapsed, Children = { Target() } };
            var disabled = new StackPanel { IsEnabled = false, Children = { Target() } };
            var expected = Target();
            var panel = new StackPanel { Children = { hidden, disabled, new StackPanel { Children = { expected } } } };
            Interaction.GetBehaviors(panel).Add(new DefaultFocusBehavior());

            // Act
            RaiseLoaded(panel);

            // Assert
            Assert.Same(expected, Focused(panel));
        });
    }

    [Fact]
    public void UsesBoundTarget()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var bound = new TextBox();
            BindingOperations.SetBinding(bound, DefaultFocus.IsTargetProperty, new Binding(nameof(Model.IsNew)) { Source = new Model(false) });
            var expected = Target();
            var panel = new StackPanel { Children = { bound, expected } };
            Interaction.GetBehaviors(panel).Add(new DefaultFocusBehavior());

            // Act
            RaiseLoaded(panel);

            // Assert
            Assert.Same(expected, Focused(panel));
        });
    }

    [Fact]
    public void KeepsAssociatedElementWithoutTarget()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var panel = new StackPanel { Children = { new TextBox(), Target(false) } };
            Interaction.GetBehaviors(panel).Add(new DefaultFocusBehavior());

            // Act
            RaiseLoaded(panel);

            // Assert
            Assert.Same(panel, Focused(panel));
        });
    }

    [Fact]
    public void IgnoresLoadedAfterDetached()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var panel = new StackPanel { Children = { Target() } };
            var behavior = new DefaultFocusBehavior();
            Interaction.GetBehaviors(panel).Add(behavior);
            Interaction.GetBehaviors(panel).Remove(behavior);

            // Act
            RaiseLoaded(panel);

            // Assert
            Assert.Same(panel, Focused(panel));
        });
    }

    private sealed class Model(bool isNew)
    {
        public bool IsNew { get; } = isNew;
    }
}
