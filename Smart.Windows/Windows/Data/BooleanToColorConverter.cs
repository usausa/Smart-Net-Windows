namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Color))]
    public sealed class BooleanToColorConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        public Color TrueColor { get; set; } = Colors.Transparent;

        /// <summary>
        ///
        /// </summary>
        public Color FalseColor { get; set; } = Colors.Transparent;

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
            return value != null && (bool)value ? TrueColor : FalseColor;
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
            if (Equals(value, TrueColor))
            {
                return true;
            }

            if (Equals(value, FalseColor))
            {
                return false;
            }

            return Binding.DoNothing;
        }
    }
}
