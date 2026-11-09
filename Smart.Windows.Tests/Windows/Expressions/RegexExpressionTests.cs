namespace Smart.Windows.Expressions;

using System.Globalization;
using System.Text.RegularExpressions;

using Smart.Windows.Data;

public sealed class RegexExpressionTests
{
    [Fact]
    public void ReturnsTrueWhenTextMatchesPattern()
    {
        // Arrange
        var expression = new RegexExpression { Pattern = @"^\d{3}-\d{4}$" };

        // Act & Assert
        Assert.True(expression.Eval("123-4567", null));
        Assert.False(expression.Eval("1234567", null));
    }

    [Fact]
    public void ReturnsFalseForNullAndNonString()
    {
        // Arrange
        var expression = new RegexExpression { Pattern = ".*" };

        // Act & Assert
        Assert.False(expression.Eval(null, null));
        Assert.False(expression.Eval(1234567, null));
    }

    [Fact]
    public void IgnoresRightValue()
    {
        // Arrange
        var expression = new RegexExpression { Pattern = "^abc$" };

        // Act & Assert
        Assert.True(expression.Eval("abc", "^xyz$"));
    }

    [Fact]
    public void UsesOptionsSetAfterEvaluation()
    {
        // Arrange
        var expression = new RegexExpression { Pattern = "^abc$" };
        Assert.False(expression.Eval("ABC", null));

        // Act
        expression.Options = RegexOptions.IgnoreCase;

        // Assert
        Assert.True(expression.Eval("ABC", null));
    }

    [Fact]
    public void UsesPatternSetAfterEvaluation()
    {
        // Arrange
        var expression = new RegexExpression { Pattern = "^abc$" };
        Assert.True(expression.Eval("abc", null));

        // Act
        expression.Pattern = "^xyz$";

        // Assert
        Assert.False(expression.Eval("abc", null));
        Assert.True(expression.Eval("xyz", null));
    }

    [Fact]
    public void WorksWithCompareConverter()
    {
        // Arrange
        var converter = new CompareToBoolConverter { Expression = new RegexExpression { Pattern = @"^\d{3}-\d{4}$" } };

        // Act & Assert
        Assert.Equal(true, converter.Convert("123-4567", typeof(bool), null, CultureInfo.InvariantCulture));
        Assert.Equal(false, converter.Convert("1234567", typeof(bool), null, CultureInfo.InvariantCulture));
    }
}
