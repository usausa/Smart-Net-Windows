namespace Smart.Windows.Internal;

using System.ComponentModel;
using System.Globalization;

public static class ConvertHelper
{
    public static object? Convert(Type targetType, object value)
    {
        if (targetType == value.GetType())
        {
            return value;
        }

        if (value is string str)
        {
            var typeConverter = TypeDescriptor.GetConverter(targetType);
            if (typeConverter.CanConvertFrom(typeof(string)))
            {
                return typeConverter.ConvertFromInvariantString(str);
            }
        }

#pragma warning disable CA1031
        try
        {
            return System.Convert.ChangeType(value, targetType, CultureInfo.CurrentCulture);
        }
        catch (Exception)
        {
            return null;
        }
    }
#pragma warning restore CA1031
}
