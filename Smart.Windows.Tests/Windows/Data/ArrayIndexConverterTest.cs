namespace Smart.Windows.Data;

using System.Globalization;

public sealed class ArrayIndexConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertReturnsElementAtIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(1, typeof(object), array, Culture);

        // Assert
        Assert.Equal("b", result);
    }

    [Fact]
    public void ConvertNullIndexReturnsNull()
    {
        // Arrange
        var converter = new ArrayIndexConverter();

        // Act
        var result = converter.Convert(null, typeof(object), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackReturnsIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y", "z" };

        // Act
        var result = converter.ConvertBack("y", typeof(int), array, Culture);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ConvertBackNotFoundReturnsMinusOne()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "x", "y" };

        // Act
        var result = converter.ConvertBack("z", typeof(int), array, Culture);

        // Assert
        Assert.Equal(-1, result);
    }
}
