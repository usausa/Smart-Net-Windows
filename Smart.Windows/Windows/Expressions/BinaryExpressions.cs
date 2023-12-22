namespace Smart.Windows.Expressions;

using Smart.Windows.Internal;

public static class BinaryExpressions
{
    public static IBinaryExpression Max { get; } = new MaxExpression();

    public static IBinaryExpression Min { get; } = new MinExpression();

    public static IBinaryExpression Add { get; } = new AddExpression();

    public static IBinaryExpression Multiply { get; } = new MultiplyExpression();

    private abstract class CompareExpression : IBinaryExpression
    {
        public object? Eval(object? left, object? right)
        {
            if ((left is IComparable comparable) && (right is not null))
            {
                var convertedValue = ConvertHelper.Convert(left.GetType(), right);
                if (convertedValue is null)
                {
                    return left;
                }

                return comparable.CompareTo(convertedValue) > 0 ? left : right;
            }

            return left;
        }

        protected abstract object EvalComparison(int comparison, object left, object right);
    }

    private sealed class MaxExpression : CompareExpression
    {
        protected override object EvalComparison(int comparison, object left, object right)
        {
            return comparison > 0 ? left : right;
        }
    }

    private sealed class MinExpression : CompareExpression
    {
        protected override object EvalComparison(int comparison, object left, object right)
        {
            return comparison < 0 ? left : right;
        }
    }

    private abstract class ArithmeticExpression : IBinaryExpression
    {
        public object? Eval(object? left, object? right)
        {
            if ((left is null) || (right is null))
            {
                return null;
            }

            var mi = left.GetType().GetMethod(MethodName);
            if ((mi is not null) &&
                (mi.GetParameters().Length == 2) &&
                (mi.GetParameters()[0].ParameterType == left.GetType()))
            {
                var convertedValue = ConvertHelper.Convert(mi.GetParameters()[1].ParameterType, right);
                if (convertedValue is null)
                {
                    return null;
                }

                return mi.Invoke(null, [left, convertedValue]);
            }
            else
            {
                var convertedValue = ConvertHelper.Convert(left.GetType(), right);
                if (convertedValue is null)
                {
                    return null;
                }

                return EvalInternal(left, convertedValue);
            }
        }

        protected abstract string MethodName { get; }

        protected abstract object EvalInternal(dynamic x, dynamic y);
    }

    private sealed class AddExpression : ArithmeticExpression
    {
        protected override string MethodName => "op_Addition";

        protected override object EvalInternal(dynamic x, dynamic y) => x + y;
    }

    private sealed class MultiplyExpression : ArithmeticExpression
    {
        protected override string MethodName => "op_Multiply";

        protected override object EvalInternal(dynamic x, dynamic y) => x * y;
    }
}
