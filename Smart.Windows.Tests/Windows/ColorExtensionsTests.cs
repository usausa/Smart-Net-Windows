namespace Smart.Windows;

using System.Windows.Media;

public sealed class ColorExtensionsTests
{
    private const int Precision = 6;

    [Theory]
    [InlineData(255, 0, 0, 0d, 1d, 0.5d)]
    [InlineData(0, 255, 0, 120d, 1d, 0.5d)]
    [InlineData(0, 0, 255, 240d, 1d, 0.5d)]
    [InlineData(255, 255, 255, 0d, 0d, 1d)]
    [InlineData(0, 0, 0, 0d, 0d, 0d)]
    public void ToHslReturnsHueSaturationLightness(int r, int g, int b, double h, double s, double l)
    {
        // Act
        var hsl = Color.FromRgb((byte)r, (byte)g, (byte)b).ToHsl();

        // Assert
        Assert.Equal(h, hsl.H, Precision);
        Assert.Equal(s, hsl.S, Precision);
        Assert.Equal(l, hsl.L, Precision);
        Assert.Equal(1d, hsl.A, Precision);
    }

    [Theory]
    [InlineData(255, 0, 0, 0d, 1d, 1d)]
    [InlineData(0, 255, 0, 120d, 1d, 1d)]
    [InlineData(0, 0, 128, 240d, 1d, 128d / 255)]
    [InlineData(255, 255, 255, 0d, 0d, 1d)]
    [InlineData(0, 0, 0, 0d, 0d, 0d)]
    public void ToHsvReturnsHueSaturationValue(int r, int g, int b, double h, double s, double v)
    {
        // Act
        var hsv = Color.FromRgb((byte)r, (byte)g, (byte)b).ToHsv();

        // Assert
        Assert.Equal(h, hsv.H, Precision);
        Assert.Equal(s, hsv.S, Precision);
        Assert.Equal(v, hsv.V, Precision);
        Assert.Equal(1d, hsv.A, Precision);
    }

    [Fact]
    public void ToHslAndToHsvKeepAlpha()
    {
        // Arrange
        var color = Color.FromArgb(51, 255, 0, 0);

        // Act & Assert
        Assert.Equal(0.2d, color.ToHsl().A, Precision);
        Assert.Equal(0.2d, color.ToHsv().A, Precision);
    }

    [Theory]
    [InlineData(0d, 255, 0, 0)]
    [InlineData(120d, 0, 255, 0)]
    [InlineData(240d, 0, 0, 255)]
    [InlineData(360d, 255, 0, 0)]
    [InlineData(480d, 0, 255, 0)]
    [InlineData(-120d, 0, 0, 255)]
    public void ToColorWrapsHue(double h, int r, int g, int b)
    {
        // Arrange
        var expected = Color.FromRgb((byte)r, (byte)g, (byte)b);

        // Act & Assert
        Assert.Equal(expected, new HslColor(h, 1, 0.5, 1).ToColor());
        Assert.Equal(expected, new HsvColor(h, 1, 1, 1).ToColor());
    }

    [Fact]
    public void ToColorClampsOutOfRangeValues()
    {
        // Act & Assert
        Assert.Equal(Color.FromArgb(255, 255, 0, 0), new HslColor(0, 2, 0.5, 2).ToColor());
        Assert.Equal(Color.FromArgb(0, 0, 0, 0), new HslColor(0, 1, -1, -1).ToColor());
        Assert.Equal(Color.FromArgb(255, 255, 255, 255), new HsvColor(0, -1, 2, 1).ToColor());
    }

    [Fact]
    public void ToColorReturnsOriginalColor()
    {
        // Act & Assert
        for (var r = 0; r <= 255; r += 15)
        {
            for (var g = 0; g <= 255; g += 15)
            {
                for (var b = 0; b <= 255; b += 15)
                {
                    var color = Color.FromArgb(128, (byte)r, (byte)g, (byte)b);
                    Assert.Equal(color, color.ToHsl().ToColor());
                    Assert.Equal(color, color.ToHsv().ToColor());
                }
            }
        }
    }

    [Fact]
    public void WithChangesOneComponent()
    {
        // Arrange
        var hsl = Colors.Red.ToHsl();

        // Act
        var color = (hsl with { L = 0.25 }).ToColor();

        // Assert
        Assert.Equal(Color.FromRgb(128, 0, 0), color);
    }
}
