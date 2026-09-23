namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Smart.Windows.Expressions;

public sealed class MultiBinaryConverter : IMultiValueConverter
{
    public IBinaryExpression Expression { get; set; } = default!;

    public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Contains(DependencyProperty.UnsetValue))
        {
            return DependencyProperty.UnsetValue;
        }

        var value = values[0];
        for (var i = 1; i < values.Length; i++)
        {
            value = Expression.Eval(value, values[i]);
        }

        return value;
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
