namespace Smart.Windows.Data;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

using Smart.Converter;

public sealed class ObjectConvertConverter : IValueConverter
{
    public IObjectConverter Converter { get; set; } = ObjectConverter.Default;

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ObjectConverter.Default is expected to be trim-compatible or callers take responsibility")]
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ObjectConverter.Default is expected to be trim-compatible or callers take responsibility")]
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }
}
