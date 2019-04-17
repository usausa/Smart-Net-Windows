namespace Smart.Windows.Expressions
{
    using System;
    using System.Globalization;

    public static class BinaryExpressions
    {
        public static IBinaryExpression Max { get; } = new MaxExpression();

        public static IBinaryExpression Min { get; } = new MinExpression();

        public static IBinaryExpression Add { get; } = new AddExpression();

        public static IBinaryExpression Multiply { get; } = new MultiplyExpression();

        // TODO base
        private sealed class MaxExpression : IBinaryExpression
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Ignore")]
            public object Eval(object left, object right)
            {
                if ((left is IComparable comparable) && (right != null))
                {
                    object convertedValue;
                    try
                    {
                        convertedValue = Convert.ChangeType(right, left.GetType(), CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        convertedValue = null;
                    }

                    if (convertedValue is null)
                    {
                        return left;
                    }

                    return comparable.CompareTo(convertedValue) > 0 ? left : right;
                }

                return left;
            }
        }

        // TODO base
        private sealed class MinExpression : IBinaryExpression
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Ignore")]
            public object Eval(object left, object right)
            {
                if ((left is IComparable comparable) && (right != null))
                {
                    object convertedValue;
                    try
                    {
                        convertedValue = Convert.ChangeType(right, left.GetType(), CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        convertedValue = null;
                    }

                    if (convertedValue is null)
                    {
                        return left;
                    }

                    return comparable.CompareTo(convertedValue) < 0 ? left : right;
                }

                return left;
            }
        }

        private sealed class AddExpression : IBinaryExpression
        {
            public object Eval(object left, object right)
            {
                throw new System.NotImplementedException();
            }
        }

        private sealed class MultiplyExpression : IBinaryExpression
        {
            public object Eval(object left, object right)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
