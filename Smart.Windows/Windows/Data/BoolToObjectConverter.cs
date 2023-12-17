namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public abstract class BoolToObjectConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? TrueValue : FalseValue;
        }

        return FalseValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Equals(value, TrueValue))
        {
            return true;
        }

        if (Equals(value, FalseValue))
        {
            return false;
        }

        return Binding.DoNothing;
    }
}

[ValueConversion(typeof(bool), typeof(string))]
public sealed class BoolToTextConverter : BoolToObjectConverter<string?>
{
}

[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BoolToVisibilityConverter : BoolToObjectConverter<Visibility>
{
}

[ValueConversion(typeof(bool), typeof(Brush))]
public sealed class BoolToBrushConverter : BoolToObjectConverter<Brush>
{
    public BoolToBrushConverter()
    {
        TrueValue = Brushes.Transparent;
        FalseValue = Brushes.Transparent;
    }
}

[ValueConversion(typeof(bool), typeof(Color))]
public sealed class BoolToColorConverter : BoolToObjectConverter<Color>
{
    public BoolToColorConverter()
    {
        TrueValue = Colors.Transparent;
        FalseValue = Colors.Transparent;
    }
}
