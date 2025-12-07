namespace Smart.Windows.Data;

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

public sealed class AllConverter : IMultiValueConverter
{
    public bool Invert { get; set; }

    public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        foreach (var value in values)
        {
            if (!ConvertToBoolean(value, culture))
            {
                return Invert;
            }
        }

        return !Invert;
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ConvertToBoolean(object? value, CultureInfo culture) =>
        value is not null && System.Convert.ToBoolean(value, culture);
}
