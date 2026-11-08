namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

public sealed class FormatConverterTests
{
    private static readonly CultureInfo English = CultureInfo.GetCultureInfo("en-US");

    private static readonly CultureInfo Japanese = CultureInfo.GetCultureInfo("ja-JP");

    [Fact]
    public void ConvertFormatsWithParameter()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal("$12.50", converter.Convert(12.5m, typeof(string), "C2", English));
    }

    [Fact]
    public void ConvertUsesBindingCulture()
    {
        // Arrange
        var converter = new FormatConverter();
        var date = new DateTime(2026, 11, 8);

        // Act & Assert
        Assert.Equal("11/8/2026", converter.Convert(date, typeof(string), "d", English));
        Assert.Equal("2026/11/08", converter.Convert(date, typeof(string), "d", Japanese));
    }

    [Fact]
    public void ConvertUseCurrentCultureIgnoresBindingCulture()
    {
        // Arrange
        var converter = new FormatConverter { UseCurrentCulture = true };
        var date = new DateTime(2026, 11, 8);
        var original = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = Japanese;
        try
        {
            // Act
            var result = converter.Convert(date, typeof(string), "d", English);

            // Assert
            Assert.Equal("2026/11/08", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void ConvertCompositeFormat()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal("Price: $12.50", converter.Convert(12.5m, typeof(string), "Price: {0:C2}", English));
    }

    [Fact]
    public void ConvertWithoutParameterUsesGeneralFormat()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal("1234.5", converter.Convert(1234.5d, typeof(string), null, English));
    }

    [Fact]
    public void ConvertNullReturnsNull()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Null(converter.Convert(null, typeof(string), "C2", English));
    }

    [Fact]
    public void ConvertNotFormattableReturnsValue()
    {
        // Arrange
        var converter = new FormatConverter();
        var value = new object();

        // Act & Assert
        Assert.Same(value, converter.Convert(value, typeof(string), "C2", English));
    }

    [Fact]
    public void ConvertBackParsesCurrency()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal(1234.5m, converter.ConvertBack("$1,234.50", typeof(decimal), "C2", English));
        Assert.Equal(12.5m, converter.ConvertBack("12.5", typeof(decimal), "C2", English));
    }

    [Fact]
    public void ConvertBackParsesNullableNumber()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal(42, converter.ConvertBack("42", typeof(int?), null, English));
        Assert.Null(converter.ConvertBack(string.Empty, typeof(int?), null, English));
    }

    [Fact]
    public void ConvertBackEmptyToValueTypeReturnsUnsetValue()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Same(DependencyProperty.UnsetValue, converter.ConvertBack(" ", typeof(int), null, English));
    }

    [Fact]
    public void ConvertBackInvalidTextReturnsUnsetValue()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Same(DependencyProperty.UnsetValue, converter.ConvertBack("abc", typeof(decimal), "C2", English));
    }

    [Fact]
    public void ConvertBackUsesNumberStyles()
    {
        // Arrange
        var converter = new FormatConverter { NumberStyles = NumberStyles.Integer };

        // Act & Assert
        Assert.Same(DependencyProperty.UnsetValue, converter.ConvertBack("1,234", typeof(int), null, English));
    }

    [Fact]
    public void ConvertBackParsesDate()
    {
        // Arrange
        var converter = new FormatConverter();
        var date = new DateTime(2026, 11, 8);

        // Act & Assert
        Assert.Equal(date, converter.ConvertBack("20261108", typeof(DateTime), "yyyyMMdd", English));
        Assert.Equal(date, converter.ConvertBack("2026/11/8", typeof(DateTime), "d", Japanese));
        Assert.Equal(date, converter.ConvertBack("11/8/2026", typeof(DateTime?), "d", English));
    }

    [Fact]
    public void ConvertBackParsesTimeTypes()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal(new DateTimeOffset(2026, 11, 8, 9, 30, 0, TimeSpan.FromHours(9)), converter.ConvertBack("2026-11-08T09:30:00+09:00", typeof(DateTimeOffset), null, English));
        Assert.Equal(new DateOnly(2026, 11, 8), converter.ConvertBack("2026-11-08", typeof(DateOnly), "yyyy-MM-dd", English));
        Assert.Equal(new TimeOnly(9, 30), converter.ConvertBack("09:30", typeof(TimeOnly), "HH:mm", English));
        Assert.Equal(TimeSpan.FromMinutes(90), converter.ConvertBack("01:30:00", typeof(TimeSpan), null, English));
    }

    [Fact]
    public void ConvertBackParsesEnum()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal(DayOfWeek.Monday, converter.ConvertBack("monday", typeof(DayOfWeek), null, English));
        Assert.Same(DependencyProperty.UnsetValue, converter.ConvertBack("unknown", typeof(DayOfWeek), null, English));
    }

    [Fact]
    public void ConvertBackStringReturnsText()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Equal("text", converter.ConvertBack("text", typeof(string), "C2", English));
    }

    [Fact]
    public void ConvertBackUnsupportedTypeThrows()
    {
        // Arrange
        var converter = new FormatConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack("true", typeof(bool), null, English));
    }
}
