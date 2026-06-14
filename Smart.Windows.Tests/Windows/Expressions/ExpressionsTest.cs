namespace Smart.Windows.Expressions;

public sealed class CompareExpressionsTest
{
    [Fact]
    public void EqualReturnsTrueForSameValue()
    {
        Assert.True(CompareExpressions.Equal.Eval(42, 42));
    }

    [Fact]
    public void EqualReturnsFalseForDifferentValues()
    {
        Assert.False(CompareExpressions.Equal.Eval(42, 99));
    }

    [Fact]
    public void EqualReturnsTrueForBothNull()
    {
        Assert.True(CompareExpressions.Equal.Eval(null, null));
    }

    [Fact]
    public void EqualReturnsFalseForNullAndNonNull()
    {
        Assert.False(CompareExpressions.Equal.Eval(null, 1));
    }

    [Fact]
    public void NotEqualReturnsTrueForDifferentValues()
    {
        Assert.True(CompareExpressions.NotEqual.Eval(1, 2));
    }

    [Fact]
    public void NotEqualReturnsFalseForSameValue()
    {
        Assert.False(CompareExpressions.NotEqual.Eval(5, 5));
    }

    [Fact]
    public void LessThanReturnsTrueWhenLeftIsSmaller()
    {
        Assert.True(CompareExpressions.LessThan.Eval(1, 2));
    }

    [Fact]
    public void LessThanReturnsFalseWhenLeftIsGreater()
    {
        Assert.False(CompareExpressions.LessThan.Eval(3, 2));
    }

    [Fact]
    public void LessThanOrEqualReturnsTrueForEqual()
    {
        Assert.True(CompareExpressions.LessThanOrEqual.Eval(2, 2));
    }

    [Fact]
    public void GreaterThanReturnsTrueWhenLeftIsLarger()
    {
        Assert.True(CompareExpressions.GreaterThan.Eval(5, 3));
    }

    [Fact]
    public void GreaterThanOrEqualReturnsTrueForEqual()
    {
        Assert.True(CompareExpressions.GreaterThanOrEqual.Eval(4, 4));
    }

    [Fact]
    public void EqualWithStringAndIntParameter()
    {
        // ConvertHelper converts "42" to int via TypeDescriptor
        Assert.True(CompareExpressions.Equal.Eval(42, "42"));
    }
}

public sealed class BinaryExpressionsTest
{
    [Fact]
    public void MaxReturnsLargerValue()
    {
        Assert.Equal(10, BinaryExpressions.Max.Eval(10, 5));
    }

    [Fact]
    public void MaxReturnsRightWhenRightIsLarger()
    {
        Assert.Equal(20, BinaryExpressions.Max.Eval(5, 20));
    }

    [Fact]
    public void MinReturnsSmallerValue()
    {
        Assert.Equal(3, BinaryExpressions.Min.Eval(3, 7));
    }

    [Fact]
    public void MinReturnsRightWhenRightIsSmaller()
    {
        Assert.Equal(1, BinaryExpressions.Min.Eval(9, 1));
    }

    [Fact]
    public void AddIntegers()
    {
        Assert.Equal(7, BinaryExpressions.Add.Eval(3, 4));
    }

    [Fact]
    public void AddDoubles()
    {
        Assert.Equal(5.5, BinaryExpressions.Add.Eval(2.5, 3.0));
    }

    [Fact]
    public void SubIntegers()
    {
        Assert.Equal(1, BinaryExpressions.Sub.Eval(4, 3));
    }

    [Fact]
    public void AddNullLeftReturnsNull()
    {
        Assert.Null(BinaryExpressions.Add.Eval(null, 1));
    }

    [Fact]
    public void AddNullRightReturnsNull()
    {
        Assert.Null(BinaryExpressions.Add.Eval(1, null));
    }

    [Fact]
    public void MaxNullRightReturnsLeft()
    {
        Assert.Equal(5, BinaryExpressions.Max.Eval(5, null));
    }
}
