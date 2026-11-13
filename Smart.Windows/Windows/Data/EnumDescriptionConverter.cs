namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

[ValueConversion(typeof(object), typeof(string))]
public sealed class EnumDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return DependencyProperty.UnsetValue;
        }

        if (value is Enum e)
        {
            return Enums.GetDescription(e);
        }

        return value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
