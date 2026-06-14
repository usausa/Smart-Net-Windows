namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public sealed class AllConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void AllTrueReturnsTrue()
    {
        // Arrange
        var converter = new AllConverter();

        // Act
        var result = converter.Convert([true, true, true], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void AnyFalseReturnsFalse()
    {
        // Arrange
        var converter = new AllConverter();

        // Act
        var result = converter.Convert([true, false, true], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAllTrueReturnsFalse()
    {
        // Arrange
        var converter = new AllConverter { Invert = true };

        // Act
        var result = converter.Convert([true, true], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAnyFalseReturnsTrue()
    {
        // Arrange
        var converter = new AllConverter { Invert = true };

        // Act
        var result = converter.Convert([true, false], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new AllConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, [], null, Culture));
    }
}

public sealed class AnyConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void AnyTrueReturnsTrue()
    {
        // Arrange
        var converter = new AnyConverter();

        // Act
        var result = converter.Convert([false, true, false], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void AllFalseReturnsFalse()
    {
        // Arrange
        var converter = new AnyConverter();

        // Act
        var result = converter.Convert([false, false], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAnyTrueReturnsFalse()
    {
        // Arrange
        var converter = new AnyConverter { Invert = true };

        // Act
        var result = converter.Convert([false, true], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new AnyConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(true, [], null, Culture));
    }
}

public sealed class ArrayIndexConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertReturnsElementAtIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(1, typeof(object), array, Culture);

        // Assert
        Assert.Equal("b", result);
    }

    [Fact]
    public void ConvertNullIndexReturnsNull()
    {
        // Arrange
        var converter = new ArrayIndexConverter();

        // Act
        var result = converter.Convert(null, typeof(object), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackReturnsIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y", "z" };

        // Act
        var result = converter.ConvertBack("y", typeof(int), array, Culture);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ConvertBackNotFoundReturnsMinusOne()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y" };

        // Act
        var result = converter.ConvertBack("z", typeof(int), array, Culture);

        // Assert
        Assert.Equal(-1, result);
    }
}

public sealed class BoolToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void TrueReturnsConfiguredTrueValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("yes", converter.Convert(true, typeof(string), null, Culture));
    }

    [Fact]
    public void FalseReturnsConfiguredFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.Convert(false, typeof(string), null, Culture));
    }

    [Fact]
    public void NullReturnsFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(true, converter.ConvertBack("yes", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(false, converter.ConvertBack("no", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackUnknownReturnsDoNothing()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(Binding.DoNothing, converter.ConvertBack("maybe", typeof(bool), null, Culture));
    }

    [Fact]
    public void BoolToVisibilityConverter()
    {
        // Arrange
        var converter = new BoolToVisibilityConverter
        {
            TrueValue = Visibility.Visible,
            FalseValue = Visibility.Collapsed
        };

        // Act & Assert
        Assert.Equal(Visibility.Visible, converter.Convert(true, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(false, typeof(Visibility), null, Culture));
    }

    [Fact]
    public void BoolToColorConverter()
    {
        // Arrange
        var converter = new BoolToColorConverter
        {
            TrueValue = Colors.Red,
            FalseValue = Colors.Blue
        };

        // Act & Assert
        Assert.Equal(Colors.Red, converter.Convert(true, typeof(Color), null, Culture));
        Assert.Equal(Colors.Blue, converter.Convert(false, typeof(Color), null, Culture));
    }
}

public sealed class ChainConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EmptyChainReturnsOriginalValue()
    {
        // Arrange
        var converter = new ChainConverter();

        // Act & Assert
        Assert.Equal("hello", converter.Convert("hello", typeof(string), null, Culture));
    }

    [Fact]
    public void ChainAppliesConvertersInOrder()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());
        converter.Converters.Add(new ReverseConverter()); // ReverseConverter passes non-bool through

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("HELLO", result);
    }
}

public sealed class ColorBlendConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void BlendAtZeroReturnsOriginalColor()
    {
        // Arrange
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.0 };

        // Act
        var result = converter.Convert(Colors.Blue, typeof(Color), null, Culture);

        // Assert
        Assert.Equal(Colors.Blue, result);
    }

    [Fact]
    public void BlendAtOneReturnsTargetColor()
    {
        // Arrange
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 1.0 };

        // Act
        var result = converter.Convert(Colors.Blue, typeof(Color), null, Culture);

        // Assert
        var color = Assert.IsType<Color>(result);
        Assert.Equal(255, color.R);
        Assert.Equal(0, color.G);
        Assert.Equal(0, color.B);
    }

    [Fact]
    public void BlendNonColorReturnsUnsetValue()
    {
        // Arrange
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.5 };

        // Act
        var result = converter.Convert("not a color", typeof(Color), null, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.5 };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(Colors.Red, typeof(Color), null, Culture));
    }

    [Fact]
    public void InvalidRaitoThrows()
    {
        // Arrange
        var converter = new ColorBlendConverter();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => converter.Raito = 2.0);
    }
}

public sealed class ColorToBrushConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertColorToSolidColorBrush()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act
        var result = converter.Convert(Colors.Red, typeof(SolidColorBrush), null, Culture);

        // Assert
        var brush = Assert.IsType<SolidColorBrush>(result);
        Assert.Equal(Colors.Red, brush.Color);
    }

    [Fact]
    public void ConvertNullReturnsNull()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(SolidColorBrush), null, Culture));
    }

    [Fact]
    public void ConvertBackBrushToColor()
    {
        // Arrange
        var converter = new ColorToBrushConverter();
        var brush = new SolidColorBrush(Colors.Green);

        // Act
        var result = converter.ConvertBack(brush, typeof(Color), null, Culture);

        // Assert
        Assert.Equal(Colors.Green, result);
    }

    [Fact]
    public void ConvertBackNullReturnsNull()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act & Assert
        Assert.Null(converter.ConvertBack(null, typeof(Color), null, Culture));
    }
}

public sealed class CompareConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EqualReturnsTrue()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert(42, typeof(bool), 42, Culture));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(42, typeof(bool), 99, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(true, typeof(object), null, Culture));
    }
}

public sealed class ContainsConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ContainedValueReturnsTrue()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b", "c" };

        // Act & Assert
        Assert.Equal(true, converter.Convert("b", typeof(bool), list, Culture));
    }

    [Fact]
    public void NotContainedValueReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b" };

        // Act & Assert
        Assert.Equal(false, converter.Convert("z", typeof(bool), list, Culture));
    }

    [Fact]
    public void NullParameterReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert("a", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(true, typeof(object), null, Culture));
    }
}

public sealed class EnumDescriptionConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private enum TestEnum
    {
        [System.ComponentModel.Description("First Item")]
        First,
        Second
    }

    [Fact]
    public void ReturnsDescriptionWhenPresent()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal("First Item", converter.Convert(TestEnum.First, typeof(string), null, Culture));
    }

    [Fact]
    public void ReturnsToStringWhenNoDescription()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal("Second", converter.Convert(TestEnum.Second, typeof(string), null, Culture));
    }

    [Fact]
    public void NullReturnsUnsetValue()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("test", typeof(TestEnum), null, Culture));
    }
}

public sealed class NullToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void NullReturnsNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };

        // Act & Assert
        Assert.Equal("null", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void NonNullReturnsNonNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };

        // Act & Assert
        Assert.Equal("not-null", converter.Convert("something", typeof(string), null, Culture));
    }

    [Fact]
    public void HandleEmptyStringTreatsEmptyAsNull()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = true
        };

        // Act & Assert
        Assert.Equal("null", converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void HandleEmptyStringFalseDoesNotTreatEmptyAsNull()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = false
        };

        // Act & Assert
        Assert.Equal("not-null", converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void NullToBoolDefaultValues()
    {
        // Arrange
        var converter = new NullToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(null, typeof(bool), null, Culture));
        Assert.Equal(true, converter.Convert("x", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new NullToTextConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("test", typeof(string), null, Culture));
    }
}

public sealed class NullToParameterConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void NullValueReturnsParameter()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act & Assert
        Assert.Equal("fallback", converter.Convert(null, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void NonNullValueReturnsValue()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act & Assert
        Assert.Equal("value", converter.Convert("value", typeof(string), "fallback", Culture));
    }

    [Fact]
    public void InvertNullValueReturnsValue()
    {
        // Arrange
        var converter = new NullToParameterConverter { Invert = true };

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void HandleEmptyStringTreatsEmptyAsNull()
    {
        // Arrange
        var converter = new NullToParameterConverter { HandleEmptyString = true };

        // Act & Assert
        Assert.Equal("fallback", converter.Convert(string.Empty, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("test", typeof(string), "fallback", Culture));
    }
}

public sealed class ParameterEqualsConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EqualReturnsTrue()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert("a", typeof(bool), "a", Culture));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert("a", typeof(bool), "b", Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsParameter()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal("param", converter.ConvertBack(true, typeof(string), "param", Culture));
    }

    [Fact]
    public void ConvertBackFalseReturnsDoNothing()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(Binding.DoNothing, converter.ConvertBack(false, typeof(string), "param", Culture));
    }
}

public sealed class ReverseConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void TrueReturnsFalse()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(true, typeof(bool), null, Culture));
    }

    [Fact]
    public void FalseReturnsTrue()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert(false, typeof(bool), null, Culture));
    }

    [Fact]
    public void NonBoolPassesThrough()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal("hello", converter.Convert("hello", typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsFalse()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act & Assert
        Assert.Equal(false, converter.ConvertBack(true, typeof(bool), null, Culture));
    }
}

public sealed class TextReplaceConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ReplacesPattern()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+", Replacement = "#" };

        // Act & Assert
        Assert.Equal("abc#def#", converter.Convert("abc123def456", typeof(string), null, Culture));
    }

    [Fact]
    public void ReplaceAllFalseReplacesFirstOnly()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+", Replacement = "#", ReplaceAll = false };

        // Act & Assert
        Assert.Equal("abc#def456", converter.Convert("abc123def456", typeof(string), null, Culture));
    }

    [Fact]
    public void NullInputReturnsNull()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+" };

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void EmptyStringReturnsEmptyString()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+" };

        // Act & Assert
        Assert.Equal(string.Empty, converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = "x" };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("test", typeof(string), null, Culture));
    }
}

public sealed class ToLowerConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertsToLowerCase()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act & Assert
        Assert.Equal("hello world", converter.Convert("Hello World", typeof(string), null, Culture));
    }

    [Fact]
    public void NonStringReturnsUnsetValue()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act & Assert
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(42, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("hello", typeof(string), null, Culture));
    }
}

public sealed class ToUpperConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertsToUpperCase()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act & Assert
        Assert.Equal("HELLO WORLD", converter.Convert("Hello World", typeof(string), null, Culture));
    }

    [Fact]
    public void NonStringReturnsUnsetValue()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act & Assert
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(42, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("HELLO", typeof(string), null, Culture));
    }
}

public sealed class ObjectToBoolConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void MatchingValueReturnsTrue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(true, converter.Convert("yes", typeof(bool), null, Culture));
    }

    [Fact]
    public void NonMatchingValueReturnsFalse()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(false, converter.Convert("other", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsTrueValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("yes", converter.ConvertBack(true, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackFalseReturnsFalseValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.ConvertBack(false, typeof(string), null, Culture));
    }

    [Fact]
    public void IntToBoolConverter()
    {
        // Arrange
        var converter = new IntToBoolConverter { TrueValue = 1, FalseValue = 0 };

        // Act & Assert
        Assert.Equal(true, converter.Convert(1, typeof(bool), null, Culture));
        Assert.Equal(false, converter.Convert(0, typeof(bool), null, Culture));
        Assert.Equal(false, converter.Convert(99, typeof(bool), null, Culture));
    }
}

public sealed class BinaryConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EvalDelegatesToExpression()
    {
        // Arrange
        var called = false;
        object? leftReceived = null;
        object? rightReceived = null;
        var expr = new DelegateExpression((l, r) =>
        {
            called = true;
            leftReceived = l;
            rightReceived = r;
            return "result";
        });
        var converter = new BinaryConverter { Expression = expr };

        // Act
        var result = converter.Convert("left", typeof(object), "right", Culture);

        // Assert
        Assert.True(called);
        Assert.Equal("left", leftReceived);
        Assert.Equal("right", rightReceived);
        Assert.Equal("result", result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new BinaryConverter { Expression = new DelegateExpression((_, _) => null) };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("test", typeof(object), null, Culture));
    }

    private sealed class DelegateExpression : Expressions.IBinaryExpression
    {
        private readonly Func<object?, object?, object?> func;

        public DelegateExpression(Func<object?, object?, object?> func)
        {
            this.func = func;
        }

        public object? Eval(object? left, object? right) => func(left, right);
    }
}

public sealed class MultiBinaryConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void FoldsValuesWithExpression()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = Expressions.BinaryExpressions.Add };

        // Act
        var result = converter.Convert([1, 2, 3], typeof(int), null, Culture);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = Expressions.BinaryExpressions.Add };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(1, [], null, Culture));
    }
}

public sealed class MapToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void UnmappedKeyReturnsDefaultValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };

        // Act & Assert
        Assert.Equal("default", converter.Convert("unknown", typeof(string), null, Culture));
    }

    [Fact]
    public void NullValueReturnsDefault()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "fallback" };

        // Act & Assert
        Assert.Equal("fallback", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new MapToTextConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("x", typeof(object), null, Culture));
    }
}
