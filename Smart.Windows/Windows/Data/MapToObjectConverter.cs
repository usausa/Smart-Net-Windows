namespace Smart.Windows.Data;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public abstract class MapEntry<T>
{
    public object Key { get; set; } = default!;

    public T Value { get; set; } = default!;
}

public abstract class MapToObjectConverter<T> : IValueConverter
{
    public Collection<MapEntry<T>> Entries { get; } = new([]);

    public T DefaultValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not null)
        {
            if (value is IComparable comparable)
            {
                var entry = Entries.FirstOrDefault(x => comparable.CompareTo(x.Key) == 0);
                if (entry is not null)
                {
                    return entry.Value;
                }
            }
            else
            {
                var entry = Entries.FirstOrDefault(x => Equals(value, x.Key));
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
