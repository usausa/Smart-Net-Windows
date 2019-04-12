namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class BooleanToBrushConverter<T> : IValueConverter
    {
        public T TrueValue { get; set; }

        public T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueValue : FalseValue;
            }

            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
    public sealed class BooleanToTextConverter : BooleanToBrushConverter<string>
    {
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BooleanToVisibilityConverter : BooleanToBrushConverter<Visibility>
    {
    }

    [ValueConversion(typeof(bool), typeof(Brush))]
    public sealed class BooleanToBrushConverter : BooleanToBrushConverter<Brush>
    {
        public BooleanToBrushConverter()
        {
            TrueValue = Brushes.Transparent;
            FalseValue = Brushes.Transparent;
        }
    }

    [ValueConversion(typeof(bool), typeof(Color))]
    public sealed class BooleanToColorConverter : BooleanToBrushConverter<Color>
    {
        public BooleanToColorConverter()
        {
            TrueValue = Colors.Transparent;
            FalseValue = Colors.Transparent;
        }
    }
}
