namespace Smart.Windows.Expressions;

public interface IBinaryExpression
{
    object? Eval(object? left, object? right);
}
