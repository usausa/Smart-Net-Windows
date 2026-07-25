namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

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
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("HELLO", typeof(string), null, Culture));
    }
}
