namespace Smart.Windows.Data;

using System.Globalization;

public sealed class ObjectToBoolConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void MatchingValueReturnsTrue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(true, converter.Convert("yes", typeof(bool), null, Culture));
    }

    [Fact]
    public void NonMatchingValueReturnsFalse()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(false, converter.Convert("other", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueReturnsTrueValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("yes", converter.ConvertBack(true, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackFalseReturnsFalseValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.ConvertBack(false, typeof(string), null, Culture));
    }

    [Fact]
    public void IntToBoolConverter()
    {
        // Arrange
        var converter = new IntToBoolConverter { TrueValue = 1, FalseValue = 0 };

        // Act & Assert
        Assert.Equal(true, converter.Convert(1, typeof(bool), null, Culture));
        Assert.Equal(false, converter.Convert(0, typeof(bool), null, Culture));
        Assert.Equal(false, converter.Convert(99, typeof(bool), null, Culture));
    }
}
