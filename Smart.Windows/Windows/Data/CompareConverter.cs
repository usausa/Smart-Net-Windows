namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using Smart.Windows.Expressions;

public abstract class CompareConverter<T> : IValueConverter
{
    public ICompareExpression Expression { get; set; } = CompareExpressions.Equal;

    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Expression.Eval(value, parameter) ? TrueValue : FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

[ValueConversion(typeof(object), typeof(bool))]
public sealed class CompareToBoolConverter : CompareConverter<bool>
{
    public CompareToBoolConverter()
    {
        TrueValue = true;
        FalseValue = false;
    }
}

[ValueConversion(typeof(object), typeof(string))]
public sealed class CompareToTextConverter : CompareConverter<string?>
{
}

[ValueConversion(typeof(object), typeof(Visibility))]
public sealed class CompareToVisibilityConverter : CompareConverter<Visibility>
{
}

[ValueConversion(typeof(object), typeof(Brush))]
public sealed class CompareToBrushConverter : CompareConverter<Brush>
{
    public CompareToBrushConverter()
    {
        TrueValue = Brushes.Transparent;
        FalseValue = Brushes.Transparent;
    }
}

[ValueConversion(typeof(object), typeof(Color))]
public sealed class CompareToColorConverter : CompareConverter<Color>
{
    public CompareToColorConverter()
    {
        TrueValue = Colors.Transparent;
        FalseValue = Colors.Transparent;
    }
}
