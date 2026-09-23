namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

public sealed class AllConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void AllTrueReturnsTrue()
    {
        // Arrange
        var converter = new AllConverter();

        // Act
        var result = converter.Convert([true, true, true], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void AnyFalseReturnsFalse()
    {
        // Arrange
        var converter = new AllConverter();

        // Act
        var result = converter.Convert([true, false, true], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAllTrueReturnsFalse()
    {
        // Arrange
        var converter = new AllConverter { Invert = true };

        // Act
        var result = converter.Convert([true, true], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void InvertAnyFalseReturnsTrue()
    {
        // Arrange
        var converter = new AllConverter { Invert = true };

        // Act
        var result = converter.Convert([true, false], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void UnsetValueReturnsUnsetValue()
    {
        // Arrange
        var converter = new AllConverter();

        // Act
        var result = converter.Convert([true, DependencyProperty.UnsetValue], typeof(bool), null, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new AllConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, [], null, Culture));
    }
}
