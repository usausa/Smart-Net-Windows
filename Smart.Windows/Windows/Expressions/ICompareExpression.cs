namespace Smart.Windows.Expressions
{
    using System.Globalization;

    public interface ICompareExpression
    {
        bool Eval(object left, object right);
    }
}
