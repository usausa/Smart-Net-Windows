namespace Smart.Windows.Data;

using System.Globalization;

public sealed class NullToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void NullReturnsNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };

        // Act & Assert
        Assert.Equal("null", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void NonNullReturnsNonNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "not-null" };

        // Act & Assert
        Assert.Equal("not-null", converter.Convert("something", typeof(string), null, Culture));
    }

    [Fact]
    public void HandleEmptyStringTreatsEmptyAsNull()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = true
        };

        // Act & Assert
        Assert.Equal("null", converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void HandleEmptyStringFalseDoesNotTreatEmptyAsNull()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "null",
            NonNullValue = "not-null",
            HandleEmptyString = false
        };

        // Act & Assert
        Assert.Equal("not-null", converter.Convert(string.Empty, typeof(string), null, Culture));
    }

    [Fact]
    public void NullToBoolDefaultValues()
    {
        // Arrange
        var converter = new NullToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(null, typeof(bool), null, Culture));
        Assert.Equal(true, converter.Convert("x", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new NullToTextConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("test", typeof(string), null, Culture));
    }
}
