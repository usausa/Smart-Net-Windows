namespace Smart.Windows.Data
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class ContainsConverter<T> : IValueConverter
    {
        public T TrueValue { get; set; }

        public T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is IList list)
            {
                return list.Contains(value) ? TrueValue : FalseValue;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class ContainsToBooleanConverter : ContainsConverter<bool>
    {
        public ContainsToBooleanConverter()
        {
            TrueValue = true;
            FalseValue = false;
        }
    }

    [ValueConversion(typeof(object), typeof(string))]
    public sealed class ContainsToTextConverter : ContainsConverter<string>
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
}
