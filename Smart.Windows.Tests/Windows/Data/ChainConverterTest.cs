namespace Smart.Windows.Data;

using System.Globalization;

public sealed class ChainConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EmptyChainReturnsOriginalValue()
    {
        // Arrange
        var converter = new ChainConverter();

        // Act & Assert
        Assert.Equal("hello", converter.Convert("hello", typeof(string), null, Culture));
    }

    [Fact]
    public void ChainAppliesConvertersInOrder()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());
        converter.Converters.Add(new ReverseConverter()); // ReverseConverter passes non-bool through

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("HELLO", result);
    }
}
