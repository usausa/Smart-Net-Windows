namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

[ValueConversion(typeof(object), typeof(object))]
public sealed class ParameterEqualsConverter : IValueConverter
{
    private static readonly object BoxedTrue = true;
    private static readonly object BoxedFalse = false;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, parameter) ? BoxedTrue : BoxedFalse;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, true) ? parameter : Binding.DoNothing;
    }
}
