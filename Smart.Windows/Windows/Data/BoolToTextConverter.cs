namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(bool), typeof(string))]
    public sealed class BoolToTextConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        public string TrueText { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string FalseText { get; set; }

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
            return value != null && (bool)value ? TrueText : FalseText;
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
            if (Equals(value, TrueText))
            {
                return true;
            }

            if (Equals(value, FalseText))
            {
                return false;
            }

            return Binding.DoNothing;
        }
    }
}
