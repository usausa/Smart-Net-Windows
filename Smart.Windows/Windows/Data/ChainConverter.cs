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
        public Collection<IValueConverter> Converters { get; } = new Collection<IValueConverter>(new List<IValueConverter>());

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var converter in Converters)
            {
                value = converter.Convert(value, targetType, parameter, culture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var converter in Converters.Reverse())
            {
                value = converter.ConvertBack(value, targetType, parameter, culture);
            }

            return value;
        }
    }
}
