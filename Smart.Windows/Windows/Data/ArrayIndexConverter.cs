namespace Smart.Windows.Data;

using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

[ValueConversion(typeof(int), typeof(object))]
public sealed class ArrayIndexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if ((value is int index) && (index >= 0))
        {
            if ((parameter is Array array) && (index < array.Length))
            {
                return array.GetValue(index);
            }

            if ((parameter is IList list) && (index < list.Count))
            {
                return list[index];
            }
        }

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter is IList list ? list.IndexOf(value) : -1;
    }
}
