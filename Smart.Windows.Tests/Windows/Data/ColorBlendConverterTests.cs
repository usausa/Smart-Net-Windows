namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Media;

public sealed class ColorBlendConverterTests
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
