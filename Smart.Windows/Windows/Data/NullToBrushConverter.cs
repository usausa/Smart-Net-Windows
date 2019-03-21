namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(object), typeof(Brush))]
    public sealed class NullToBrushConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        public Brush NullValue { get; set; } = Brushes.Transparent;

        /// <summary>
        ///
        /// </summary>
        public Brush NonNullValue { get; set; } = Brushes.Transparent;

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
                return NullValue;
            }

            return NonNullValue;
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
