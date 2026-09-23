namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

public sealed class ArrayIndexConverterTests
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
    public void ConvertNullIndexReturnsUnsetValue()
    {
        // Arrange
        var converter = new ArrayIndexConverter();

        // Act
        var result = converter.Convert(null, typeof(object), null, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertNegativeIndexReturnsUnsetValue()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(-1, typeof(object), array, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertIndexOutOfRangeReturnsUnsetValue()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new object[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(3, typeof(object), array, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
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
