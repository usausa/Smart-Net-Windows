namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ObjectToBoolConverter<T> : IValueConverter
    {
        public T TrueValue { get; set; }

        public T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T typedValue && Equals(typedValue, TrueValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? TrueValue : FalseValue;
        }
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public sealed class TextToBoolConverter : ObjectToBoolConverter<string>
    {
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public sealed class IntToBoolConverter : ObjectToBoolConverter<int>
    {
    }
}
