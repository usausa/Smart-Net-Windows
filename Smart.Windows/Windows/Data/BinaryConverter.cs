namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

using Smart.Windows.Expressions;

[ValueConversion(typeof(object), typeof(object))]
public sealed class BinaryConverter : IValueConverter
{
    public IBinaryExpression Expression { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Expression.Eval(value, parameter);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
