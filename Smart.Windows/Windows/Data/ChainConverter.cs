namespace Smart.Windows.Data;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

[ContentProperty("Converters")]
[ValueConversion(typeof(object), typeof(object))]
public sealed class ChainConverter : IValueConverter
{
    public Collection<IValueConverter> Converters { get; } = new([]);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = value;
        for (var i = 0; i < Converters.Count; i++)
        {
            result = Converters[i].Convert(result, targetType, parameter, culture);
        }

        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = value;
        for (var i = Converters.Count - 1; i >= 0; i--)
        {
            result = Converters[i].ConvertBack(result, targetType, parameter, culture);
        }

        return result;
    }
}
