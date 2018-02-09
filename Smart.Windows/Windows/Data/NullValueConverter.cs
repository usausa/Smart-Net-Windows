namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(object), typeof(object))]
    public sealed class NullValueConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        public bool HandleEmptyString { get; set; }

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
            if ((value == null) ||
                (HandleEmptyString && String.IsNullOrEmpty(value as string)))
            {
                return parameter;
            }

            return value;
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
