namespace Smart.Windows.Data;

using System.Globalization;

public sealed class AnyConverterTests
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
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, [], null, Culture));
    }
}
