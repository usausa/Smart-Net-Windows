namespace Smart.Windows.Interactivity;

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

public sealed class UpdateTargetActionTests
{
    private static readonly MethodInfo InvokeMethod =
        typeof(UpdateTargetAction).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic, null, [typeof(object)], null)!;

    private static void Invoke(UpdateTargetAction action) =>
        InvokeMethod.Invoke(action, [null]);

    [Fact]
    public void UpdatesTargetOfChangedPropertyName()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var source = new Source { Text = "a", Tag = "b" };
            var textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty, new Binding(nameof(Source.Text)) { Source = source });
            textBox.SetBinding(FrameworkElement.TagProperty, new Binding(nameof(Source.Tag)) { Source = source });
            var action = new UpdateTargetAction { PropertyName = nameof(TextBox.Text) };
            action.Attach(textBox);
            source.Text = "c";
            source.Tag = "d";

            // Act
            action.PropertyName = nameof(FrameworkElement.Tag);
            Invoke(action);

            // Assert
            Assert.Equal("d", textBox.Tag);
            Assert.Equal("a", textBox.Text);
        });
    }

    [Fact]
    public void ChangingPropertyNameRaisesChanged()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var action = new UpdateTargetAction();
            var changed = 0;
            action.Changed += (_, _) => changed++;

            // Act
            action.PropertyName = nameof(TextBox.Text);

            // Assert
            Assert.Equal(1, changed);
        });
    }

    private sealed class Source
    {
        public string Text { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;
    }
}
