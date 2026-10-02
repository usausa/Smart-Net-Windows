namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

using Smart.Windows.Input;

public sealed class LongPressBehaviorTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    private static readonly TimeSpan Wait = TimeSpan.FromMilliseconds(200);

    private static void RaiseMouseButton(UIElement element, RoutedEvent routedEvent) =>
        element.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) { RoutedEvent = routedEvent });

    private static void RaiseMouse(UIElement element, RoutedEvent routedEvent) =>
        element.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0) { RoutedEvent = routedEvent });

    [Fact]
    public void ExecutesCommandWhenHeldForDuration()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var button = new Button();
            object? received = null;
            Interaction.GetBehaviors(button).Add(new LongPressBehavior
            {
                Command = new DelegateCommand<object?>(x => received = x),
                CommandParameter = "parameter",
                Duration = TimeSpan.FromMilliseconds(10)
            });

            // Act
            RaiseMouseButton(button, UIElement.PreviewMouseLeftButtonDownEvent);
            TestDispatcher.ProcessUntil(() => received is not null, Timeout);

            // Assert
            Assert.Equal("parameter", received);
        });
    }

    [Fact]
    public void DoesNotExecuteCommandWhenReleasedBeforeDuration()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var button = new Button();
            var count = 0;
            Interaction.GetBehaviors(button).Add(new LongPressBehavior
            {
                Command = new DelegateCommand(() => count++),
                Duration = TimeSpan.FromMilliseconds(50)
            });

            // Act
            RaiseMouseButton(button, UIElement.PreviewMouseLeftButtonDownEvent);
            RaiseMouseButton(button, UIElement.PreviewMouseLeftButtonUpEvent);
            TestDispatcher.ProcessUntil(static () => false, Wait);

            // Assert
            Assert.Equal(0, count);
        });
    }

    [Fact]
    public void DoesNotExecuteCommandWhenMouseLeaves()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var button = new Button();
            var count = 0;
            Interaction.GetBehaviors(button).Add(new LongPressBehavior
            {
                Command = new DelegateCommand(() => count++),
                Duration = TimeSpan.FromMilliseconds(50)
            });

            // Act
            RaiseMouseButton(button, UIElement.PreviewMouseLeftButtonDownEvent);
            RaiseMouse(button, UIElement.MouseLeaveEvent);
            TestDispatcher.ProcessUntil(static () => false, Wait);

            // Assert
            Assert.Equal(0, count);
        });
    }

    [Fact]
    public void ExecutesCommandForElement()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var border = new Border();
            var count = 0;
            Interaction.GetBehaviors(border).Add(new LongPressBehavior
            {
                Command = new DelegateCommand(() => count++),
                Duration = TimeSpan.FromMilliseconds(10)
            });

            // Act
            RaiseMouseButton(border, UIElement.PreviewMouseLeftButtonDownEvent);
            TestDispatcher.ProcessUntil(() => count > 0, Timeout);

            // Assert
            Assert.Equal(1, count);
        });
    }

    [Fact]
    public void DoesNotExecuteCommandAfterDetached()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var button = new Button();
            var count = 0;
            var behavior = new LongPressBehavior
            {
                Command = new DelegateCommand(() => count++),
                Duration = TimeSpan.FromMilliseconds(50)
            };
            Interaction.GetBehaviors(button).Add(behavior);
            RaiseMouseButton(button, UIElement.PreviewMouseLeftButtonDownEvent);

            // Act
            Interaction.GetBehaviors(button).Remove(behavior);
            TestDispatcher.ProcessUntil(static () => false, Wait);

            // Assert
            Assert.Equal(0, count);
        });
    }
}
