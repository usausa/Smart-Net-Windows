namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

[ValueConversion(typeof(string), typeof(string))]
public sealed class ToLowerConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string text ? text.ToLower(culture) : DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
