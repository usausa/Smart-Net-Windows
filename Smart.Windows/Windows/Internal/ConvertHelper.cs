namespace Smart.Windows.Internal;

using System.ComponentModel;
using System.Globalization;

public static class ConvertHelper
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Ignore")]
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

        try
        {
            return System.Convert.ChangeType(value, targetType, CultureInfo.CurrentCulture);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
