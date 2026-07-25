namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

public sealed class EnumDescriptionConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private enum TestEnum
    {
        [System.ComponentModel.Description("First Item")]
        First,
        Second
    }

    [Fact]
    public void ReturnsDescriptionWhenPresent()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal("First Item", converter.Convert(TestEnum.First, typeof(string), null, Culture));
    }

    [Fact]
    public void ReturnsToStringWhenNoDescription()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal("Second", converter.Convert(TestEnum.Second, typeof(string), null, Culture));
    }

    [Fact]
    public void NullReturnsUnsetValue()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Equal(DependencyProperty.UnsetValue, converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("test", typeof(TestEnum), null, Culture));
    }
}
