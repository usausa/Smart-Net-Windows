namespace Smart.Windows.Data;

using System.Globalization;

public sealed class ContainsConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ContainedValueReturnsTrue()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b", "c" };

        // Act & Assert
        Assert.Equal(true, converter.Convert("b", typeof(bool), list, Culture));
    }

    [Fact]
    public void NotContainedValueReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b" };

        // Act & Assert
        Assert.Equal(false, converter.Convert("z", typeof(bool), list, Culture));
    }

    [Fact]
    public void NullParameterReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert("a", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(true, typeof(object), null, Culture));
    }
}
