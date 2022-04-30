namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

[ValueConversion(typeof(int), typeof(object))]
public sealed class ArrayIndexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if ((value is int index) && (parameter is Array array))
        {
            return array.GetValue(index);
        }

        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is Array array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var element = array.GetValue(i);
                if (((element is not null) && element.Equals(value)) ||
                    ((element is null) && (value is null)))
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
