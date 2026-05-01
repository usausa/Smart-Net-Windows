namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

[ValueConversion(typeof(bool), typeof(bool))]
public sealed class ReverseConverter : IValueConverter
{
    private static readonly object BoxedTrue = true;
    private static readonly object BoxedFalse = false;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolValue ? (boolValue ? BoxedFalse : BoxedTrue) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolValue ? (boolValue ? BoxedFalse : BoxedTrue) : value;
    }
}
