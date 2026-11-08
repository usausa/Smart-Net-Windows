namespace Smart.Windows.Interactivity;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

public sealed class MaskedTextBehaviorTests
{
    private sealed class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Value
        {
            get;
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }

    private static readonly Func<string, string> NarrowDigits = static x => String.Concat(x.Select(static c => c is >= '０' and <= '９' ? (char)(c - '０' + '0') : c));

    private static TextBox Attach(MaskedTextBehavior behavior)
    {
        var textBox = new TextBox();
        Interaction.GetBehaviors(textBox).Add(behavior);
        return textBox;
    }

    private static bool Type(TextBox textBox, string text)
    {
        var handled = true;
        for (var i = 0; i < text.Length; i++)
        {
            var args = new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, textBox, text[i..(i + 1)]))
            {
                RoutedEvent = UIElement.PreviewTextInputEvent
            };
            textBox.RaiseEvent(args);
            handled &= args.Handled;
        }

        return handled;
    }

    private static DataObjectPastingEventArgs Paste(TextBox textBox, string text)
    {
        var args = new DataObjectPastingEventArgs(new DataObject(DataFormats.UnicodeText, text), false, DataFormats.UnicodeText);
        textBox.RaiseEvent(args);
        return args;
    }

    //------------------------------------------------------------------
    // Display
    //------------------------------------------------------------------

    [Fact]
    public void ShowsPromptAndDisablesInputMethodWhenAttached()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000" });

            // Assert
            Assert.Equal("___-____", textBox.Text);
            Assert.False(InputMethod.GetIsInputMethodEnabled(textBox));
        });
    }

    [Fact]
    public void ChangesPromptChar()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "12" };
            var textBox = Attach(behavior);

            // Act
            behavior.PromptChar = '*';

            // Assert
            Assert.Equal("12*-****", textBox.Text);
        });
    }

    [Fact]
    public void ReappliesValueWhenMaskChanged()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);

            // Act
            behavior.Mask = "000 0000";

            // Assert
            Assert.Equal("123 4567", textBox.Text);
        });
    }

    //------------------------------------------------------------------
    // Typing
    //------------------------------------------------------------------

    [Fact]
    public void FillsPositionsAndSkipsLiteral()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            var handled = Type(textBox, "1234");

            // Assert
            Assert.True(handled);
            Assert.Equal("123-4___", textBox.Text);
            Assert.Equal(5, textBox.CaretIndex);
            Assert.Equal("123-4", behavior.Value);
        });
    }

    [Fact]
    public void IgnoresCharacterNotMatchingMask()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000" });

            // Act
            var handled = Type(textBox, "1a");

            // Assert
            Assert.True(handled);
            Assert.Equal("1__-____", textBox.Text);
            Assert.Equal(1, textBox.CaretIndex);
        });
    }

    [Fact]
    public void FillsFromFirstEmptyPositionWhenCaretIsAfterInput()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1" });
            textBox.Select(8, 0);

            // Act
            Type(textBox, "2");

            // Assert
            Assert.Equal("12_-____", textBox.Text);
        });
    }

    [Fact]
    public void ReplacesSelection()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" });
            textBox.Select(4, 2);

            // Act
            Type(textBox, "9");

            // Assert
            Assert.Equal("123-967_", textBox.Text);
            Assert.Equal(5, textBox.CaretIndex);
        });
    }

    [Fact]
    public void IgnoresTypingWhenFull()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" });
            textBox.Select(1, 0);

            // Act
            Type(textBox, "9");

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    //------------------------------------------------------------------
    // Delete
    //------------------------------------------------------------------

    [Fact]
    public void BackspaceRemovesPreviousCharacterAndShiftsRest()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" });
            textBox.Select(5, 0);

            // Act
            EditingCommands.Backspace.Execute(null, textBox);

            // Assert
            Assert.Equal("123-567_", textBox.Text);
            Assert.Equal(4, textBox.CaretIndex);
        });
    }

    [Fact]
    public void BackspaceSkipsLiteral()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" });
            textBox.Select(4, 0);

            // Act
            EditingCommands.Backspace.Execute(null, textBox);

            // Assert
            Assert.Equal("124-567_", textBox.Text);
            Assert.Equal(2, textBox.CaretIndex);
        });
    }

    [Fact]
    public void DeleteRemovesNextCharacter()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" });
            textBox.Select(3, 0);

            // Act
            EditingCommands.Delete.Execute(null, textBox);

            // Assert
            Assert.Equal("123-567_", textBox.Text);
            Assert.Equal(3, textBox.CaretIndex);
        });
    }

    [Fact]
    public void DeleteRemovesSelection()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" });
            textBox.Select(2, 3);

            // Act
            EditingCommands.Delete.Execute(null, textBox);

            // Assert
            Assert.Equal("125-67__", textBox.Text);
            Assert.Equal(2, textBox.CaretIndex);
        });
    }

    [Fact]
    public void ValueIsEmptyWhenAllRemoved()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);
            Type(textBox, "1");

            // Act
            EditingCommands.Backspace.Execute(null, textBox);

            // Assert
            Assert.Equal("___-____", textBox.Text);
            Assert.Equal(string.Empty, behavior.Value);
        });
    }

    //------------------------------------------------------------------
    // Paste
    //------------------------------------------------------------------

    [Fact]
    public void PastesTextWithLiteral()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            var args = Paste(textBox, "123-4567");

            // Assert
            Assert.True(args.CommandCancelled);
            Assert.Equal("123-4567", textBox.Text);
            Assert.Equal("123-4567", behavior.Value);
        });
    }

    [Fact]
    public void IgnoresPasteNotMatchingMask()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "12" });

            // Act
            var args = Paste(textBox, "abc");

            // Assert
            Assert.True(args.CommandCancelled);
            Assert.Equal("12_-____", textBox.Text);
        });
    }

    //------------------------------------------------------------------
    // Value
    //------------------------------------------------------------------

    [Fact]
    public void UpdatesTextFromSource()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var model = new Model();
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            BindingOperations.SetBinding(behavior, MaskedTextBehavior.ValueProperty, new Binding(nameof(Model.Value)) { Source = model });
            var textBox = Attach(behavior);

            // Act
            model.Value = "1234567";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void UpdatesSourceWithoutLiteral()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var model = new Model();
            var behavior = new MaskedTextBehavior { Mask = "000-0000", IncludeLiterals = false };
            BindingOperations.SetBinding(behavior, MaskedTextBehavior.ValueProperty, new Binding(nameof(Model.Value)) { Source = model });
            var textBox = Attach(behavior);

            // Act
            Type(textBox, "1234567");

            // Assert
            Assert.Equal("1234567", model.Value);
            Assert.NotNull(BindingOperations.GetBindingExpression(behavior, MaskedTextBehavior.ValueProperty));
        });
    }

    [Fact]
    public void ClearsWhenValueNotMatchingMask()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);

            // Act
            behavior.Value = "abc";

            // Assert
            Assert.Equal("___-____", textBox.Text);
        });
    }

    //------------------------------------------------------------------
    // Normalizer
    //------------------------------------------------------------------

    [Fact]
    public void AcceptsFullWidthDigitsWithoutNormalizer()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000" });

            // Act
            Paste(textBox, "１２３４５６７");

            // Assert
            Assert.Equal("１２３-４５６７", textBox.Text);
        });
    }

    [Fact]
    public void NormalizesTypedAndPastedText()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Normalizer = NarrowDigits };
            var textBox = Attach(behavior);

            // Act
            Type(textBox, "１２３");
            Paste(textBox, "４５６７");

            // Assert
            Assert.Equal("123-4567", textBox.Text);
            Assert.Equal("123-4567", behavior.Value);
        });
    }

    [Fact]
    public void NormalizesValueFromSource()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Normalizer = NarrowDigits };
            var textBox = Attach(behavior);

            // Act
            behavior.Value = "１２３４５６７";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void NormalizesTextSetDirectly()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Normalizer = NarrowDigits });

            // Act
            textBox.Text = "１２３４５６７";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void ReappliesValueWhenNormalizerChanged()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "１２３４５６７" };
            var textBox = Attach(behavior);

            // Act
            behavior.Normalizer = NarrowDigits;

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    //------------------------------------------------------------------
    // Text
    //------------------------------------------------------------------

    [Fact]
    public void FormatsTextSetDirectly()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            textBox.Text = "1234567";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
            Assert.Equal("123-4567", behavior.Value);
        });
    }

    [Fact]
    public void RevertsTextNotMatchingMask()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" });

            // Act
            textBox.Text = "abc";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    //------------------------------------------------------------------
    // Disabled
    //------------------------------------------------------------------

    [Fact]
    public void DoesNothingWithoutMask()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior());

            // Act
            var handled = Type(textBox, "a");

            // Assert
            Assert.False(handled);
            Assert.Equal(string.Empty, textBox.Text);
            Assert.True(InputMethod.GetIsInputMethodEnabled(textBox));
        });
    }

    [Fact]
    public void StopsMaskingWhenDetached()
    {
        TestDispatcher.RunSta(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            Interaction.GetBehaviors(textBox).Remove(behavior);
            var handled = Type(textBox, "a");

            // Assert
            Assert.False(handled);
            Assert.True(InputMethod.GetIsInputMethodEnabled(textBox));
        });
    }
}
