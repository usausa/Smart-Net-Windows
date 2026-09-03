namespace Smart.Windows.Expressions;

public sealed class ConvertHelperTests
{
    [Fact]
    public void SameTypeReturnsValue()
    {
        // Act
        var result = ConvertHelper.Convert(typeof(int), 123);

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void ConvertibleValueIsConverted()
    {
        // Act
        var result = ConvertHelper.Convert(typeof(long), 123);

        // Assert
        Assert.Equal(123L, result);
    }

    [Fact]
    public void OverflowReturnsNull()
    {
        // Act
        var result = ConvertHelper.Convert(typeof(int), Int64.MaxValue);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void InvalidCastReturnsNull()
    {
        // Act
        var result = ConvertHelper.Convert(typeof(int), new object());

        // Assert
        Assert.Null(result);
    }
}
