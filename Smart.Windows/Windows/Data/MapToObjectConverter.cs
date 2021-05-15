namespace Smart.Windows.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Media;

    public class MapEntry<T>
    {
        [AllowNull]
        public object Key { get; set; }

        [AllowNull]
        public T Value { get; set; }
    }

    public class MapToObjectConverter<T> : IValueConverter
    {
        public Collection<MapEntry<T>> Entries { get; } = new(new List<MapEntry<T>>());

        [AllowNull]
        public T DefaultValue { get; set; }

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
}
