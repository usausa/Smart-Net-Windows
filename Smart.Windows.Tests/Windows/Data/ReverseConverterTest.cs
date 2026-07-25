namespace Smart.Windows.Data;

using System.Globalization;

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
