namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

[ValueConversion(typeof(object), typeof(object))]
public sealed class NullToParameterConverter : IValueConverter
{
    public bool Invert { get; set; }

    public bool HandleEmptyString { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if ((value is null) ||
            (HandleEmptyString && value is string { Length: 0 }))
        {
            return Invert ? value : parameter;
        }

        return Invert ? parameter : value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
