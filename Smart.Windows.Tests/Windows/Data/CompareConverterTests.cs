namespace Smart.Windows.Data;

using System.Globalization;

public sealed class CompareConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EqualReturnsTrue()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert(42, typeof(bool), 42, Culture));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(42, typeof(bool), 99, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, typeof(object), null, Culture));
    }
}
