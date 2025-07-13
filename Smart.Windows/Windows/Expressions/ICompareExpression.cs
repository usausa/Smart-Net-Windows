namespace Smart.Windows.Expressions;

public interface ICompareExpression
{
    bool Eval(object? left, object? right);
}
