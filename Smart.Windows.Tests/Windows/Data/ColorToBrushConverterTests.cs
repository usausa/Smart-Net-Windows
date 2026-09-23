namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public sealed class ColorToBrushConverterTests
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
    public void ConvertNullReturnsUnsetValue()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act & Assert
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(null, typeof(SolidColorBrush), null, Culture));
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
    public void ConvertBackNullReturnsDoNothing()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act & Assert
        Assert.Equal(Binding.DoNothing, converter.ConvertBack(null, typeof(Color), null, Culture));
    }
}
