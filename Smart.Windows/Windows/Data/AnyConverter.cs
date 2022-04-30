namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

public sealed class AnyConverter : IMultiValueConverter
{
    public bool Invert { get; set; }

    public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values.Any(value => System.Convert.ToBoolean(value, culture)) ? !Invert : Invert;
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
