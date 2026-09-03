namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

public sealed class ParameterEqualsConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EqualReturnsTrue()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert("a", typeof(bool), "a", Culture));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert("a", typeof(bool), "b", Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsParameter()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal("param", converter.ConvertBack(true, typeof(string), "param", Culture));
    }

    [Fact]
    public void ConvertBackFalseReturnsDoNothing()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(Binding.DoNothing, converter.ConvertBack(false, typeof(string), "param", Culture));
    }
}
