namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Media;

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
