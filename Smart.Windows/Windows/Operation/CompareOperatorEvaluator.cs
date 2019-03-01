namespace Smart.Windows.Operation
{
    using System;
    using System.Globalization;

    public static class CompareOperatorEvaluator
    {
        public static bool Compare(CompareOperator op, object left, object right)
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

                if (convertedValue == null)
                {
                    return op == CompareOperator.NotEqual;
                }

                var comparison = comparable.CompareTo(convertedValue);
                switch (op)
                {
                    case CompareOperator.Equal:
                        return comparison == 0;
                    case CompareOperator.NotEqual:
                        return comparison != 0;
                    case CompareOperator.LessThan:
                        return comparison < 0;
                    case CompareOperator.LessThanOrEqual:
                        return comparison <= 0;
                    case CompareOperator.GreaterThan:
                        return comparison > 0;
                    case CompareOperator.GreaterThanOrEqual:
                        return comparison >= 0;
                }

                return false;
            }

            switch (op)
            {
                case CompareOperator.Equal:
                    return Equals(left, right);
                case CompareOperator.NotEqual:
                    return !Equals(left, right);
            }

            return false;
        }
    }
}
