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
        var converter = new AllConverter();
        var result = converter.Convert([true, true, true], typeof(bool), null, Culture);
        Assert.Equal(true, result);
    }

    [Fact]
    public void AnyFalseReturnsFalse()
    {
        var converter = new AllConverter();
        var result = converter.Convert([true, false, true], typeof(bool), null, Culture);
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAllTrueReturnsFalse()
    {
        var converter = new AllConverter { Invert = true };
        var result = converter.Convert([true, true], typeof(bool), null, Culture);
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAnyFalseReturnsTrue()
    {
        var converter = new AllConverter { Invert = true };
        var result = converter.Convert([true, false], typeof(bool), null, Culture);
        Assert.Equal(true, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new AllConverter();
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, [], null, Culture));
    }
}

public sealed class AnyConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void AnyTrueReturnsTrue()
    {
        var converter = new AnyConverter();
        var result = converter.Convert([false, true, false], typeof(bool), null, Culture);
        Assert.Equal(true, result);
    }

    [Fact]
    public void AllFalseReturnsFalse()
    {
        var converter = new AnyConverter();
        var result = converter.Convert([false, false], typeof(bool), null, Culture);
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAnyTrueReturnsFalse()
    {
        var converter = new AnyConverter { Invert = true };
        var result = converter.Convert([false, true], typeof(bool), null, Culture);
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new AnyConverter();
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
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };
        var result = converter.Convert(1, typeof(object), array, Culture);
        Assert.Equal("b", result);
    }

    [Fact]
    public void ConvertNullIndexReturnsNull()
    {
        var converter = new ArrayIndexConverter();
        var result = converter.Convert(null, typeof(object), null, Culture);
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackReturnsIndex()
    {
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y", "z" };
        var result = converter.ConvertBack("y", typeof(int), array, Culture);
        Assert.Equal(1, result);
    }

    [Fact]
    public void ConvertBackNotFoundReturnsMinusOne()
    {
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y" };
        var result = converter.ConvertBack("z", typeof(int), array, Culture);
        Assert.Equal(-1, result);
    }
}

public sealed class BoolToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void TrueReturnsConfiguredTrueValue()
    {
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal("yes", converter.Convert(true, typeof(string), null, Culture));
    }

    [Fact]
    public void FalseReturnsConfiguredFalseValue()
    {
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal("no", converter.Convert(false, typeof(string), null, Culture));
    }

    [Fact]
    public void NullReturnsFalseValue()
    {
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal("no", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueValue()
    {
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal(true, converter.ConvertBack("yes", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackFalseValue()
    {
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal(false, converter.ConvertBack("no", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackUnknownReturnsDoNothing()
    {
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal(Binding.DoNothing, converter.ConvertBack("maybe", typeof(bool), null, Culture));
    }

    [Fact]
    public void BoolToVisibilityConverter()
    {
        var converter = new BoolToVisibilityConverter
        {
            TrueValue = Visibility.Visible,
            FalseValue = Visibility.Collapsed
        };
        Assert.Equal(Visibility.Visible, converter.Convert(true, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(false, typeof(Visibility), null, Culture));
    }

    [Fact]
    public void BoolToColorConverter()
    {
        var converter = new BoolToColorConverter
        {
            TrueValue = Colors.Red,
            FalseValue = Colors.Blue
        };
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
        var converter = new ChainConverter();
        Assert.Equal("hello", converter.Convert("hello", typeof(string), null, Culture));
    }

    [Fact]
    public void ChainAppliesConvertersInOrder()
    {
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());
        converter.Converters.Add(new ReverseConverter()); // ReverseConverter passes non-bool through
        var result = converter.Convert("hello", typeof(string), null, Culture);
        Assert.Equal("HELLO", result);
    }
}

public sealed class ColorBlendConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void BlendAtZeroReturnsOriginalColor()
    {
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.0 };
        var result = converter.Convert(Colors.Blue, typeof(Color), null, Culture);
        Assert.Equal(Colors.Blue, result);
    }

    [Fact]
    public void BlendAtOneReturnsTargetColor()
    {
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 1.0 };
        var result = converter.Convert(Colors.Blue, typeof(Color), null, Culture);
        var color = Assert.IsType<Color>(result);
        Assert.Equal(255, color.R);
        Assert.Equal(0, color.G);
        Assert.Equal(0, color.B);
    }

    [Fact]
    public void BlendNonColorReturnsUnsetValue()
    {
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.5 };
        var result = converter.Convert("not a color", typeof(Color), null, Culture);
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.5 };
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(Colors.Red, typeof(Color), null, Culture));
    }

    [Fact]
    public void InvalidRaitoThrows()
    {
        var converter = new ColorBlendConverter();
        Assert.Throws<ArgumentOutOfRangeException>(() => converter.Raito = 2.0);
    }
}

public sealed class ColorToBrushConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertColorToSolidColorBrush()
    {
        var converter = new ColorToBrushConverter();
        var result = converter.Convert(Colors.Red, typeof(SolidColorBrush), null, Culture);
        var brush = Assert.IsType<SolidColorBrush>(result);
        Assert.Equal(Colors.Red, brush.Color);
    }

    [Fact]
    public void ConvertNullReturnsNull()
    {
        var converter = new ColorToBrushConverter();
        Assert.Null(converter.Convert(null, typeof(SolidColorBrush), null, Culture));
    }

    [Fact]
    public void ConvertBackBrushToColor()
    {
        var converter = new ColorToBrushConverter();
        var brush = new SolidColorBrush(Colors.Green);
        var result = converter.ConvertBack(brush, typeof(Color), null, Culture);
        Assert.Equal(Colors.Green, result);
    }

    [Fact]
    public void ConvertBackNullReturnsNull()
    {
        var converter = new ColorToBrushConverter();
        Assert.Null(converter.ConvertBack(null, typeof(Color), null, Culture));
    }
}

public sealed class CompareConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EqualReturnsTrue()
    {
        var converter = new CompareToBoolConverter();
        Assert.Equal(true, converter.Convert(42, typeof(bool), 42, Culture));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        var converter = new CompareToBoolConverter();
        Assert.Equal(false, converter.Convert(42, typeof(bool), 99, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new CompareToBoolConverter();
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
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b", "c" };
        Assert.Equal(true, converter.Convert("b", typeof(bool), list, Culture));
    }

    [Fact]
    public void NotContainedValueReturnsFalse()
    {
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b" };
        Assert.Equal(false, converter.Convert("z", typeof(bool), list, Culture));
    }

    [Fact]
    public void NullParameterReturnsFalse()
    {
        var converter = new ContainsToBoolConverter();
        Assert.Equal(false, converter.Convert("a", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new ContainsToBoolConverter();
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
        var converter = new EnumDescriptionConverter();
        Assert.Equal("First Item", converter.Convert(TestEnum.First, typeof(string), null, Culture));
    }

    [Fact]
    public void ReturnsToStringWhenNoDescription()
    {
        var converter = new EnumDescriptionConverter();
        Assert.Equal("Second", converter.Convert(TestEnum.Second, typeof(string), null, Culture));
    }

    [Fact]
    public void NullReturnsUnsetValue()
    {
        var converter = new EnumDescriptionConverter();
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new EnumDescriptionConverter();
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
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };
        Assert.Equal("null", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void NonNullReturnsNonNullValue()
    {
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };
        Assert.Equal("not-null", converter.Convert("something", typeof(string), null, Culture));
    }

    [Fact]
    public void HandleEmptyStringTreatsEmptyAsNull()
    {
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = true
        };
        Assert.Equal("null", converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void HandleEmptyStringFalseDoesNotTreatEmptyAsNull()
    {
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = false
        };
        Assert.Equal("not-null", converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void NullToBoolDefaultValues()
    {
        var converter = new NullToBoolConverter();
        Assert.Equal(false, converter.Convert(null, typeof(bool), null, Culture));
        Assert.Equal(true, converter.Convert("x", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new NullToTextConverter();
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
        var converter = new NullToParameterConverter();
        Assert.Equal("fallback", converter.Convert(null, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void NonNullValueReturnsValue()
    {
        var converter = new NullToParameterConverter();
        Assert.Equal("value", converter.Convert("value", typeof(string), "fallback", Culture));
    }

    [Fact]
    public void InvertNullValueReturnsValue()
    {
        var converter = new NullToParameterConverter { Invert = true };
        Assert.Null(converter.Convert(null, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void HandleEmptyStringTreatsEmptyAsNull()
    {
        var converter = new NullToParameterConverter { HandleEmptyString = true };
        Assert.Equal("fallback", converter.Convert(string.Empty, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new NullToParameterConverter();
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
        var converter = new ParameterEqualsConverter();
        Assert.Equal(true, converter.Convert("a", typeof(bool), "a", Culture));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        var converter = new ParameterEqualsConverter();
        Assert.Equal(false, converter.Convert("a", typeof(bool), "b", Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsParameter()
    {
        var converter = new ParameterEqualsConverter();
        Assert.Equal("param", converter.ConvertBack(true, typeof(string), "param", Culture));
    }

    [Fact]
    public void ConvertBackFalseReturnsDoNothing()
    {
        var converter = new ParameterEqualsConverter();
        Assert.Equal(Binding.DoNothing, converter.ConvertBack(false, typeof(string), "param", Culture));
    }
}

public sealed class ReverseConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void TrueReturnsFalse()
    {
        var converter = new ReverseConverter();
        Assert.Equal(false, converter.Convert(true, typeof(bool), null, Culture));
    }

    [Fact]
    public void FalseReturnsTrue()
    {
        var converter = new ReverseConverter();
        Assert.Equal(true, converter.Convert(false, typeof(bool), null, Culture));
    }

    [Fact]
    public void NonBoolPassesThrough()
    {
        var converter = new ReverseConverter();
        Assert.Equal("hello", converter.Convert("hello", typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsFalse()
    {
        var converter = new ReverseConverter();
        Assert.Equal(false, converter.ConvertBack(true, typeof(bool), null, Culture));
    }
}

public sealed class TextReplaceConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ReplacesPattern()
    {
        var converter = new TextReplaceConverter { Pattern = @"\d+", Replacement = "#" };
        Assert.Equal("abc#def#", converter.Convert("abc123def456", typeof(string), null, Culture));
    }

    [Fact]
    public void ReplaceAllFalseReplacesFirstOnly()
    {
        var converter = new TextReplaceConverter { Pattern = @"\d+", Replacement = "#", ReplaceAll = false };
        Assert.Equal("abc#def456", converter.Convert("abc123def456", typeof(string), null, Culture));
    }

    [Fact]
    public void NullInputReturnsNull()
    {
        var converter = new TextReplaceConverter { Pattern = @"\d+" };
        Assert.Null(converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void EmptyStringReturnsEmptyString()
    {
        var converter = new TextReplaceConverter { Pattern = @"\d+" };
        Assert.Equal(string.Empty, converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new TextReplaceConverter { Pattern = "x" };
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
        var converter = new ToLowerConverter();
        Assert.Equal("hello world", converter.Convert("Hello World", typeof(string), null, Culture));
    }

    [Fact]
    public void NonStringReturnsUnsetValue()
    {
        var converter = new ToLowerConverter();
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(42, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new ToLowerConverter();
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
        var converter = new ToUpperConverter();
        Assert.Equal("HELLO WORLD", converter.Convert("Hello World", typeof(string), null, Culture));
    }

    [Fact]
    public void NonStringReturnsUnsetValue()
    {
        var converter = new ToUpperConverter();
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(42, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new ToUpperConverter();
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
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal(true, converter.Convert("yes", typeof(bool), null, Culture));
    }

    [Fact]
    public void NonMatchingValueReturnsFalse()
    {
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal(false, converter.Convert("other", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsTrueValue()
    {
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal("yes", converter.ConvertBack(true, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackFalseReturnsFalseValue()
    {
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };
        Assert.Equal("no", converter.ConvertBack(false, typeof(string), null, Culture));
    }

    [Fact]
    public void IntToBoolConverter()
    {
        var converter = new IntToBoolConverter { TrueValue = 1, FalseValue = 0 };
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

        var result = converter.Convert("left", typeof(object), "right", Culture);

        Assert.True(called);
        Assert.Equal("left", leftReceived);
        Assert.Equal("right", rightReceived);
        Assert.Equal("result", result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new BinaryConverter { Expression = new DelegateExpression((_, _) => null) };
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
        var converter = new MultiBinaryConverter { Expression = Expressions.BinaryExpressions.Add };
        var result = converter.Convert([1, 2, 3], typeof(int), null, Culture);
        Assert.Equal(6, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new MultiBinaryConverter { Expression = Expressions.BinaryExpressions.Add };
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
        var converter = new MapToTextConverter { DefaultValue = "default" };
        Assert.Equal("default", converter.Convert("unknown", typeof(string), null, Culture));
    }

    [Fact]
    public void NullValueReturnsDefault()
    {
        var converter = new MapToTextConverter { DefaultValue = "fallback" };
        Assert.Equal("fallback", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        var converter = new MapToTextConverter();
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("x", typeof(object), null, Culture));
    }
}
