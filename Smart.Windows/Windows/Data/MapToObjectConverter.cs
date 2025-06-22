namespace Smart.Windows.Data;

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using Smart.Linq;

public abstract class MapEntry<T>
{
    public object Key { get; set; } = default!;

    public T Value { get; set; } = default!;
}

public abstract class MapToObjectConverter<T> : IValueConverter
{
#pragma warning disable CA1819
    public MapEntry<T>[] Entries { get; } = [];
#pragma warning restore CA1819

    public T DefaultValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not null)
        {
            if (value is IComparable comparable)
            {
                var entry = Entries.FirstOrDefault(comparable, static (x, s) => s.CompareTo(x.Key) == 0);
                if (entry is not null)
                {
                    return entry.Value;
                }
            }
            else
            {
                var entry = Entries.FirstOrDefault(value, static (x, s) => Equals(s, x.Key));
                if (entry is not null)
                {
                    return entry.Value;
                }
            }
        }

        return DefaultValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public sealed class MapToBrushEntry : MapEntry<Brush>
{
}

[ValueConversion(typeof(object), typeof(Brush))]
public sealed class MapToBrushConverter : MapToObjectConverter<Brush>
{
    public MapToBrushConverter()
    {
        DefaultValue = Brushes.Transparent;
    }
}

public sealed class MapToTextEntry : MapEntry<string?>
{
}

[ValueConversion(typeof(object), typeof(string))]
public sealed class MapToTextConverter : MapToObjectConverter<string?>
{
}

public sealed class MapToColorEntry : MapEntry<Color>
{
}

[ValueConversion(typeof(object), typeof(Color))]
public sealed class MapToColorConverter : MapToObjectConverter<Color>
{
    public MapToColorConverter()
    {
        DefaultValue = Colors.Transparent;
    }
}
