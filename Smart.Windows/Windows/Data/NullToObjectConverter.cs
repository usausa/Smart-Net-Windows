namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public abstract class NullToObjectConverter<T> : IValueConverter
{
    public T NullValue { get; set; } = default!;

    public T NonNullValue { get; set; } = default!;

    public bool HandleEmptyString { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if ((value is null) ||
            (HandleEmptyString && value is string { Length: 0 }))
        {
            return NullValue;
        }

        return NonNullValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

[ValueConversion(typeof(object), typeof(bool))]
public sealed class NullToBoolConverter : NullToObjectConverter<bool>
{
    public NullToBoolConverter()
    {
        NullValue = false;
        NonNullValue = true;
    }
}

[ValueConversion(typeof(object), typeof(string))]
public sealed class NullToTextConverter : NullToObjectConverter<string?>
{
}

[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class NullToVisibilityConverter : NullToObjectConverter<Visibility>
{
}

[ValueConversion(typeof(object), typeof(Brush))]
public sealed class NullToBrushConverter : NullToObjectConverter<Brush>
{
    public NullToBrushConverter()
    {
        NullValue = Brushes.Transparent;
        NonNullValue = Brushes.Transparent;
    }
}

[ValueConversion(typeof(object), typeof(Color))]
public sealed class NullToColorConverter : NullToObjectConverter<Color>
{
    public NullToColorConverter()
    {
        NullValue = Colors.Transparent;
        NonNullValue = Colors.Transparent;
    }
}
