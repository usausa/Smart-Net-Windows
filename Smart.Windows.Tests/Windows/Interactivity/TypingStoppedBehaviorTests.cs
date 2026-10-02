namespace Smart.Windows.Interactivity;

using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

using Smart.Windows.Input;

public sealed class TypingStoppedBehaviorTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    private static readonly TimeSpan Wait = TimeSpan.FromMilliseconds(200);

    [Fact]
    public void ExecutesCommandWithTextAfterDelay()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            object? received = null;
            Interaction.GetBehaviors(textBox).Add(new TypingStoppedBehavior
            {
                Command = new DelegateCommand<object?>(x => received = x),
                Delay = TimeSpan.FromMilliseconds(10)
            });

            // Act
            textBox.Text = "abc";
            TestDispatcher.ProcessUntil(() => received is not null, Timeout);

            // Assert
            Assert.Equal("abc", received);
        });
    }

    [Fact]
    public void ExecutesCommandOnceWithLatestText()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            var received = new List<object?>();
            Interaction.GetBehaviors(textBox).Add(new TypingStoppedBehavior
            {
                Command = new DelegateCommand<object?>(received.Add),
                Delay = TimeSpan.FromMilliseconds(50)
            });

            // Act
            textBox.Text = "a";
            textBox.Text = "ab";
            TestDispatcher.ProcessUntil(() => received.Count > 0, Timeout);
            TestDispatcher.ProcessUntil(static () => false, Wait);

            // Assert
            Assert.Equal(["ab"], received);
        });
    }

    [Fact]
    public void UsesCommandParameterWhenSet()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            object? received = null;
            Interaction.GetBehaviors(textBox).Add(new TypingStoppedBehavior
            {
                Command = new DelegateCommand<object?>(x => received = x),
                CommandParameter = "parameter",
                Delay = TimeSpan.FromMilliseconds(10)
            });

            // Act
            textBox.Text = "abc";
            TestDispatcher.ProcessUntil(() => received is not null, Timeout);

            // Assert
            Assert.Equal("parameter", received);
        });
    }
}
