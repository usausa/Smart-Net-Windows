namespace Smart.Windows.Expressions;

using System.Collections.Concurrent;
using System.Numerics;
using System.Reflection;

public static class BinaryExpressions
{
    public static IBinaryExpression Max { get; } = new MaxExpression();

    public static IBinaryExpression Min { get; } = new MinExpression();

    public static IBinaryExpression Add { get; } = new AddExpression();

    public static IBinaryExpression Sub { get; } = new SubExpression();

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

            var op = GetOperator(left.GetType());
            return op?.Invoke(null, [left, rightValue]);
        }

        protected abstract MethodInfo? GetOperator(Type type);
    }

    private sealed class AddExpression : ArithmeticExpression
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo?> MethodCache = new();

        protected override MethodInfo? GetOperator(Type type)
        {
            return MethodCache.GetOrAdd(type, static t =>
            {
                return t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAdditionOperators<,,>))
                    ? typeof(AddExpression).GetMethod(nameof(Operation), BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(t)
                    : null;
            });
        }

        private static T Operation<T>(T left, T right)
            where T : IAdditionOperators<T, T, T>
        {
            return left + right;
        }
    }

    private sealed class SubExpression : ArithmeticExpression
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo?> MethodCache = new();

        protected override MethodInfo? GetOperator(Type type)
        {
            return MethodCache.GetOrAdd(type, static t =>
            {
                return t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISubtractionOperators<,,>))
                    ? typeof(SubExpression).GetMethod(nameof(Operation), BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(t)
                    : null;
            });
        }

        private static T Operation<T>(T left, T right)
            where T : ISubtractionOperators<T, T, T>
        {
            return left - right;
        }
    }
}
