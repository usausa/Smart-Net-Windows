namespace Smart.Windows.Data;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;

using Smart.Converter;

public sealed class ObjectConvertConverter : IValueConverter
{
    public IObjectConverter Converter { get; set; } = ObjectConverter.Default;

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ObjectConverter uses reflection internally; callers must ensure target types are preserved")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "ObjectConverter uses MakeGenericType/MakeGenericMethod internally; not AOT-safe by design")]
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ObjectConverter uses reflection internally; callers must ensure target types are preserved")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "ObjectConverter uses MakeGenericType/MakeGenericMethod internally; not AOT-safe by design")]
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }
}
