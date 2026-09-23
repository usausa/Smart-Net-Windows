namespace Smart.Windows.Data;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

[ContentProperty("Converters")]
[ValueConversion(typeof(object), typeof(object))]
public sealed class ChainConverter : IValueConverter
{
    // ReSharper disable once CollectionNeverUpdated.Global
    public Collection<IValueConverter> Converters { get; } = [];

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = value;
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < Converters.Count; i++)
        {
            result = Converters[i].Convert(result, targetType, parameter, culture);
            if (IsSentinel(result))
            {
                break;
            }
        }

        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = value;
        for (var i = Converters.Count - 1; i >= 0; i--)
        {
            result = Converters[i].ConvertBack(result, targetType, parameter, culture);
            if (IsSentinel(result))
            {
                break;
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSentinel(object? value) =>
        (value == Binding.DoNothing) || (value == DependencyProperty.UnsetValue);
}
