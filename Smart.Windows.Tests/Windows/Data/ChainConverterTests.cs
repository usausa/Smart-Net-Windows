namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

public sealed class ChainConverterTests
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

    [Fact]
    public void ConvertStopsAtUnsetValue()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());
        converter.Converters.Add(new NullToTextConverter { NullValue = "null", NonNullValue = "set" });

        // Act
        // ToUpperConverter returns UnsetValue for a non-string, which must not reach NullToTextConverter
        var result = converter.Convert(1, typeof(string), null, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertBackStopsAtDoNothing()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ObjectConvertConverter());
        converter.Converters.Add(new BoolToTextConverter { TrueValue = "ON", FalseValue = "OFF" });

        // Act
        // BoolToTextConverter.ConvertBack returns DoNothing, which must not reach ObjectConvertConverter
        var result = converter.ConvertBack("unknown", typeof(int), null, Culture);

        // Assert
        Assert.Equal(Binding.DoNothing, result);
    }
}
