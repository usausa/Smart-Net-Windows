namespace Smart.Windows.Data;

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

[ValueConversion(typeof(object), typeof(string))]
public sealed class EnumDescriptionConverter : IValueConverter
{
    private static readonly ConcurrentDictionary<(Type, object), string> Cache = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return DependencyProperty.UnsetValue;
        }

        if (value is Enum)
        {
            return Cache.GetOrAdd((value.GetType(), value), static key => GetDescription(key.Item2));
        }

        return value.ToString();
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Enum type members are expected to be preserved by the application")]
    private static string GetDescription(object value)
    {
        var type = value.GetType();
        var mis = type.GetMember(value.ToString()!);
        if (mis.Length > 0)
        {
            var attr = mis[0].GetCustomAttribute<DescriptionAttribute>();
            if (attr is not null)
            {
                return attr.Description;
            }
        }

        return value.ToString()!;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
