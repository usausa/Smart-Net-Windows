namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

using Smart.Converter;

public sealed class ObjectConvertConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertReturnsConvertedValue()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.Convert(12, typeof(string), null, Culture);

        // Assert
        Assert.Equal("12", result);
    }

    [Fact]
    public void ConvertBackReturnsParsedValue()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.ConvertBack("12", typeof(int), null, Culture);

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void ConvertBackInvalidReturnsUnsetValue()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.ConvertBack("abc", typeof(int), null, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertBackEmptyToValueTypeReturnsUnsetValue()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.ConvertBack(string.Empty, typeof(int), null, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertBackEmptyToNullableReturnsNull()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.ConvertBack(string.Empty, typeof(int?), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackWithoutConverterThrows()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act & Assert
        Assert.Throws<ObjectConverterException>(() => converter.ConvertBack(new object(), typeof(int), null, Culture));
    }
}
