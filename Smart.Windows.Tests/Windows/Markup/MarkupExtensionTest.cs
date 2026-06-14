namespace Smart.Windows.Markup;

using System.Windows;
using System.Windows.Media;

internal sealed class NullServiceProvider : IServiceProvider
{
    public static readonly NullServiceProvider Instance = new();

    public object? GetService(Type serviceType) => null;
}

public sealed class BoolExtensionTest
{
    [Fact]
    public void ProvideValueReturnsTrue()
    {
        var ext = new BoolExtension(true);
        Assert.Equal(true, ext.ProvideValue(NullServiceProvider.Instance));
    }

    [Fact]
    public void ProvideValueReturnsFalse()
    {
        var ext = new BoolExtension(false);
        Assert.Equal(false, ext.ProvideValue(NullServiceProvider.Instance));
    }
}

public sealed class DoubleExtensionTest
{
    [Fact]
    public void ProvideValueReturnsDouble()
    {
        var ext = new DoubleExtension(3.14);
        Assert.Equal(3.14, ext.ProvideValue(NullServiceProvider.Instance));
    }
}

public sealed class FloatExtensionTest
{
    [Fact]
    public void ProvideValueReturnsFloat()
    {
        var ext = new FloatExtension(1.5f);
        Assert.Equal(1.5f, ext.ProvideValue(NullServiceProvider.Instance));
    }
}

public sealed class Int16ExtensionTest
{
    [Fact]
    public void ProvideValueReturnsShort()
    {
        var ext = new Int16Extension(100);
        Assert.Equal((short)100, ext.ProvideValue(NullServiceProvider.Instance));
    }
}

public sealed class Int32ExtensionTest
{
    [Fact]
    public void ProvideValueReturnsInt()
    {
        var ext = new Int32Extension(42);
        Assert.Equal(42, ext.ProvideValue(NullServiceProvider.Instance));
    }
}

public sealed class Int64ExtensionTest
{
    [Fact]
    public void ProvideValueReturnsLong()
    {
        var ext = new Int64Extension(999L);
        Assert.Equal(999L, ext.ProvideValue(NullServiceProvider.Instance));
    }
}

public sealed class EnumValuesExtensionTest
{
    // ReSharper disable UnusedMember.Local
    private enum SampleEnum
    {
        A,
        B,
        C
    }
    // ReSharper restore UnusedMember.Local

    [Fact]
    public void ProvideValueReturnsEnumValues()
    {
        var ext = new EnumValuesExtension(typeof(SampleEnum));
        var result = ext.ProvideValue(NullServiceProvider.Instance);
        var values = Assert.IsAssignableFrom<System.Collections.IEnumerable>(result);
        var list = values.Cast<object>().ToList();
        Assert.Equal(3, list.Count);
    }
}

public sealed class BoolToExtensionTest
{
    [Fact]
    public void BoolToTextExtensionProvidesBoolToTextConverter()
    {
        var ext = new BoolToTextExtension { True = "yes", False = "no" };
        var converter = Assert.IsType<Data.BoolToTextConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal("yes", converter.TrueValue);
        Assert.Equal("no", converter.FalseValue);
    }

    [Fact]
    public void BoolToVisibilityExtensionProvidesConverter()
    {
        var ext = new BoolToVisibilityExtension { True = Visibility.Visible, False = Visibility.Collapsed };
        var converter = Assert.IsType<Data.BoolToVisibilityConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal(Visibility.Visible, converter.TrueValue);
        Assert.Equal(Visibility.Collapsed, converter.FalseValue);
    }

    [Fact]
    public void BoolToColorExtensionProvidesConverter()
    {
        var ext = new BoolToColorExtension { True = Colors.Red, False = Colors.Blue };
        var converter = Assert.IsType<Data.BoolToColorConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal(Colors.Red, converter.TrueValue);
        Assert.Equal(Colors.Blue, converter.FalseValue);
    }
}

public sealed class ColorBlendExtensionTest
{
    [Fact]
    public void ProvideValueProvidesColorBlendConverter()
    {
        var ext = new ColorBlendExtension { Color = Colors.Red, Raito = 0.5 };
        var converter = Assert.IsType<Data.ColorBlendConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal(Colors.Red, converter.Color);
        Assert.Equal(0.5, converter.Raito);
    }
}

public sealed class CompareToExtensionTest
{
    [Fact]
    public void CompareToBoolExtensionProvidesConverter()
    {
        var ext = new CompareToBoolExtension();
        var converter = Assert.IsType<Data.CompareToBoolConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.NotNull(converter.Expression);
    }

    [Fact]
    public void CompareToTextExtensionProvidesConverter()
    {
        var ext = new CompareToTextExtension { True = "equal", False = "not equal" };
        var converter = Assert.IsType<Data.CompareToTextConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal("equal", converter.TrueValue);
        Assert.Equal("not equal", converter.FalseValue);
    }
}

public sealed class ContainsToExtensionTest
{
    [Fact]
    public void ContainsToBoolExtensionProvidesConverter()
    {
        var ext = new ContainsToBoolExtension();
        var converter = Assert.IsType<Data.ContainsToBoolConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.True(converter.TrueValue);
        Assert.False(converter.FalseValue);
    }

    [Fact]
    public void ContainsToBoolExtensionInvertedProvidesConverter()
    {
        var ext = new ContainsToBoolExtension { Invert = true };
        var converter = Assert.IsType<Data.ContainsToBoolConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.False(converter.TrueValue);
        Assert.True(converter.FalseValue);
    }
}

public sealed class NullToExtensionTest
{
    [Fact]
    public void NullToBoolExtensionProvidesConverter()
    {
        var ext = new NullToBoolExtension();
        var converter = Assert.IsType<Data.NullToBoolConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.False(converter.HandleEmptyString);
    }

    [Fact]
    public void NullToTextExtensionProvidesConverter()
    {
        var ext = new NullToTextExtension { Null = "(null)", NonNull = "value" };
        var converter = Assert.IsType<Data.NullToTextConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal("(null)", converter.NullValue);
        Assert.Equal("value", converter.NonNullValue);
    }
}

public sealed class TextReplaceExtensionTest
{
    [Fact]
    public void ProvideValueProvidesTextReplaceConverter()
    {
        var ext = new TextReplaceExtension { Pattern = @"\d+", Replacement = "#" };
        var converter = Assert.IsType<Data.TextReplaceConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal(@"\d+", converter.Pattern);
        Assert.Equal("#", converter.Replacement);
    }
}

public sealed class ToBoolExtensionTest
{
    [Fact]
    public void TextToBoolExtensionProvidesConverter()
    {
        var ext = new TextToBoolExtension { True = "yes", False = "no" };
        var converter = Assert.IsType<Data.TextToBoolConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal("yes", converter.TrueValue);
        Assert.Equal("no", converter.FalseValue);
    }

    [Fact]
    public void IntToBoolExtensionProvidesConverter()
    {
        var ext = new IntToBoolExtension { True = 1, False = 0 };
        var converter = Assert.IsType<Data.IntToBoolConverter>(ext.ProvideValue(NullServiceProvider.Instance));
        Assert.Equal(1, converter.TrueValue);
        Assert.Equal(0, converter.FalseValue);
    }
}
