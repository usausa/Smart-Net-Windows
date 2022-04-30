namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

[ValueConversion(typeof(object), typeof(object))]
public sealed class ParameterEqualsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, true) ? parameter : Binding.DoNothing;
    }
}
