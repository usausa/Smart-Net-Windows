namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

public sealed class ToLowerConverterTests
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
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("hello", typeof(string), null, Culture));
    }
}
