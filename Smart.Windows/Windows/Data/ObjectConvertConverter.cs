namespace Smart.Windows.Data;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Smart.Converter;

[RequiresUnreferencedCode("Uses ObjectConverter, which uses reflection to discover types at runtime.")]
[RequiresDynamicCode("Uses ObjectConverter, which uses MakeGenericType/MakeGenericMethod at runtime.")]
public sealed class ObjectConvertConverter : IValueConverter
{
    public IObjectConverter Converter { get; set; } = ObjectConverter.Default;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Converter.TryConvert(value, targetType, out var result))
        {
            return result;
        }

        if (Converter.CanConvert(value, targetType))
        {
            return DependencyProperty.UnsetValue;
        }

        // Convert throws when there is no converter for the types
        return Converter.Convert(value, targetType);
    }
}
