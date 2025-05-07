namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

using Smart.Converter;

public sealed class ObjectConvertConverter : IValueConverter
{
    public IObjectConverter Converter { get; set; } = ObjectConverter.Default;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }
}
