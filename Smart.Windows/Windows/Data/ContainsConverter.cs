namespace Smart.Windows.Data;

using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public abstract class ContainsConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter is IList list && list.Contains(value) ? TrueValue : FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

[ValueConversion(typeof(object), typeof(bool))]
public sealed class ContainsToBoolConverter : ContainsConverter<bool>
{
    public ContainsToBoolConverter()
    {
        TrueValue = true;
        FalseValue = false;
    }
}

[ValueConversion(typeof(object), typeof(string))]
public sealed class ContainsToTextConverter : ContainsConverter<string?>
{
}

[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class ContainsToVisibilityConverter : ContainsConverter<Visibility>
{
}

[ValueConversion(typeof(object), typeof(Brush))]
public sealed class ContainsToBrushConverter : ContainsConverter<Brush>
{
    public ContainsToBrushConverter()
    {
        TrueValue = Brushes.Transparent;
        FalseValue = Brushes.Transparent;
    }
}

[ValueConversion(typeof(object), typeof(Color))]
public sealed class ContainsToColorConverter : ContainsConverter<Color>
{
    public ContainsToColorConverter()
    {
        TrueValue = Colors.Transparent;
        FalseValue = Colors.Transparent;
    }
}
