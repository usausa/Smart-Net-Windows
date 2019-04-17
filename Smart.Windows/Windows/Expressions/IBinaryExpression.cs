namespace Smart.Windows.Expressions
{
    using System.Globalization;

    public interface IBinaryExpression
    {
        object Eval(object left, object right);
    }
}
