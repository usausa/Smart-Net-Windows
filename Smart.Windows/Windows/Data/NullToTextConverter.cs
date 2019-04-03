namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(string))]
    public sealed class NullToTextConverter : IValueConverter
    {
        public string NullValue { get; set; }

        public string NonNullValue { get; set; }

        public bool HandleEmptyString { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is null) ||
                (HandleEmptyString && String.IsNullOrEmpty(value as string)))
            {
                return NullValue;
            }

            return NonNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
