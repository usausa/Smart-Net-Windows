namespace Smart.Windows.Data;

using System.Globalization;

public sealed class TextReplaceConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ReplacesPattern()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+", Replacement = "#" };

        // Act & Assert
        Assert.Equal("abc#def#", converter.Convert("abc123def456", typeof(string), null, Culture));
    }

    [Fact]
    public void ReplaceAllFalseReplacesFirstOnly()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+", Replacement = "#", ReplaceAll = false };

        // Act & Assert
        Assert.Equal("abc#def456", converter.Convert("abc123def456", typeof(string), null, Culture));
    }

    [Fact]
    public void NullInputReturnsNull()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+" };

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void EmptyStringReturnsEmptyString()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = @"\d+" };

        // Act & Assert
        Assert.Equal(string.Empty, converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = "x" };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("test", typeof(string), null, Culture));
    }
}
