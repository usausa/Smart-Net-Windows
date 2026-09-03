namespace Smart.Windows.Data;

using System.Globalization;

public sealed class NullToParameterConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void NullValueReturnsParameter()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act & Assert
        Assert.Equal("fallback", converter.Convert(null, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void NonNullValueReturnsValue()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act & Assert
        Assert.Equal("value", converter.Convert("value", typeof(string), "fallback", Culture));
    }

    [Fact]
    public void InvertNullValueReturnsValue()
    {
        // Arrange
        var converter = new NullToParameterConverter { Invert = true };

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void HandleEmptyStringTreatsEmptyAsNull()
    {
        // Arrange
        var converter = new NullToParameterConverter { HandleEmptyString = true };

        // Act & Assert
        Assert.Equal("fallback", converter.Convert(string.Empty, typeof(string), "fallback", Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("test", typeof(string), "fallback", Culture));
    }
}
