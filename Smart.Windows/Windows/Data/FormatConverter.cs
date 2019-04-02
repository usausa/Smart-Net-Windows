namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class FormatConverter : IValueConverter
    {
        public string Format { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return string.Empty;
            }

            if (value is IFormattable formattable)
            {
                return formattable.ToString(Format, culture);
            }

            return value.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
