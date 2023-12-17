namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;

public abstract class ObjectToBoolConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is T typedValue && Equals(typedValue, TrueValue);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? TrueValue : FalseValue;
    }
}

[ValueConversion(typeof(string), typeof(bool))]
public sealed class TextToBoolConverter : ObjectToBoolConverter<string?>
{
}

[ValueConversion(typeof(int), typeof(bool))]
public sealed class IntToBoolConverter : ObjectToBoolConverter<int>
{
}
