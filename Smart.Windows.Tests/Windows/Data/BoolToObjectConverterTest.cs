namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public sealed class BoolToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void TrueReturnsConfiguredTrueValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("yes", converter.Convert(true, typeof(string), null, Culture));
    }

    [Fact]
    public void FalseReturnsConfiguredFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.Convert(false, typeof(string), null, Culture));
    }

    [Fact]
    public void NullReturnsFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.Convert(null, typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertBackTrueValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(true, converter.ConvertBack("yes", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(false, converter.ConvertBack("no", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackUnknownReturnsDoNothing()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(Binding.DoNothing, converter.ConvertBack("maybe", typeof(bool), null, Culture));
    }

    [Fact]
    public void BoolToVisibilityConverter()
    {
        // Arrange
        var converter = new BoolToVisibilityConverter
        {
            TrueValue = Visibility.Visible,
            FalseValue = Visibility.Collapsed
        };

        // Act & Assert
        Assert.Equal(Visibility.Visible, converter.Convert(true, typeof(Visibility), null, Culture));
        Assert.Equal(Visibility.Collapsed, converter.Convert(false, typeof(Visibility), null, Culture));
    }

    [Fact]
    public void BoolToColorConverter()
    {
        // Arrange
        var converter = new BoolToColorConverter
        {
            TrueValue = Colors.Red,
            FalseValue = Colors.Blue
        };

        // Act & Assert
        Assert.Equal(Colors.Red, converter.Convert(true, typeof(Color), null, Culture));
        Assert.Equal(Colors.Blue, converter.Convert(false, typeof(Color), null, Culture));
    }
}
