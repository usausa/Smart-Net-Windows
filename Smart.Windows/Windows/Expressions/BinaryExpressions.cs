namespace Smart.Windows.Expressions;

using System.Diagnostics.CodeAnalysis;

public static class BinaryExpressions
{
    public static IBinaryExpression Max { get; } = new MaxExpression();

    public static IBinaryExpression Min { get; } = new MinExpression();

    public static IBinaryExpression Add { get; } = new AddExpression();

    public static IBinaryExpression Sub { get; } = new SubExpression();

    private abstract class CompareExpression : IBinaryExpression
    {
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ConvertHelper uses TypeDescriptor; callers are XAML-driven runtime expressions")]
        public object? Eval(object? left, object? right)
        {
            if ((left is IComparable comparable) && (right is not null))
            {
                var convertedValue = ConvertHelper.Convert(left.GetType(), right);
                if (convertedValue is null)
                {
                    return left;
                }

                return EvalComparison(comparable.CompareTo(convertedValue), left, right);
            }

            return left;
        }

        protected abstract object EvalComparison(int comparison, object left, object right);
    }

    private sealed class MaxExpression : CompareExpression
    {
        protected override object EvalComparison(int comparison, object left, object right)
        {
            return comparison >= 0 ? left : right;
        }
    }

    private sealed class MinExpression : CompareExpression
    {
        protected override object EvalComparison(int comparison, object left, object right)
        {
            return comparison <= 0 ? left : right;
        }
    }

    private abstract class ArithmeticExpression : IBinaryExpression
    {
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ConvertHelper uses TypeDescriptor; callers are XAML-driven runtime expressions")]
        public object? Eval(object? left, object? right)
        {
            if ((left is null) || (right is null))
            {
                return null;
            }

            object? rightValue;
            if (left.GetType() == right.GetType())
            {
                rightValue = right;
            }
            else
            {
                rightValue = ConvertHelper.Convert(left.GetType(), right);
                if (rightValue is null)
                {
                    return null;
                }
            }

            return Calculate(left, rightValue);
        }

        protected abstract object? Calculate(object left, object right);
    }

    private sealed class AddExpression : ArithmeticExpression
    {
        protected override object? Calculate(object left, object right) => left switch
        {
            int l when right is int r => l + r,
            long l when right is long r => l + r,
            double l when right is double r => l + r,
            float l when right is float r => l + r,
            decimal l when right is decimal r => l + r,
            short l when right is short r => (short)(l + r),
            uint l when right is uint r => l + r,
            ulong l when right is ulong r => l + r,
            _ => null
        };
    }

    private sealed class SubExpression : ArithmeticExpression
    {
        protected override object? Calculate(object left, object right) => left switch
        {
            int l when right is int r => l - r,
            long l when right is long r => l - r,
            double l when right is double r => l - r,
            float l when right is float r => l - r,
            decimal l when right is decimal r => l - r,
            short l when right is short r => (short)(l - r),
            uint l when right is uint r => l - r,
            ulong l when right is ulong r => l - r,
            _ => null
        };
    }
}
