namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;

public sealed class MultiBinaryConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void FoldsValuesWithExpression()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = Expressions.BinaryExpressions.Add };

        // Act
        var result = converter.Convert([1, 2, 3], typeof(int), null, Culture);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void UnsetValueReturnsUnsetValue()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = Expressions.BinaryExpressions.Add };

        // Act
        var result = converter.Convert([1, DependencyProperty.UnsetValue, 3], typeof(int), null, Culture);

        // Assert
        Assert.Equal(DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = Expressions.BinaryExpressions.Add };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => converter.ConvertBack(1, [], null, Culture));
    }
}
