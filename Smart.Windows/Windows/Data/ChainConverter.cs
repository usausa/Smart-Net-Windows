namespace Smart.Windows.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Markup;

    [ContentProperty("Converters")]
    [ValueConversion(typeof(object), typeof(object))]
    public sealed class ChainConverter : IValueConverter
    {
        public Collection<IValueConverter> Converters { get; } = new(new List<IValueConverter>());

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Converters.Aggregate(value, (current, converter) =>
                converter.Convert(current, targetType, parameter, culture));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Converters.Reverse().Aggregate(value, (current, converter) =>
                converter.ConvertBack(current, targetType, parameter, culture));
        }
    }
}
