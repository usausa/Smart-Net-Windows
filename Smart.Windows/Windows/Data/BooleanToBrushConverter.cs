namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Brush))]
    public sealed class BooleanToBrushConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        public Brush TrueBrush { get; set; } = Brushes.Transparent;

        /// <summary>
        ///
        /// </summary>
        public Brush FalseBrush { get; set; } = Brushes.Transparent;

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
            return value != null && (bool)value ? TrueBrush : FalseBrush;
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
            if (Equals(value, TrueBrush))
            {
                return true;
            }

            if (Equals(value, FalseBrush))
            {
                return false;
            }

            return Binding.DoNothing;
        }
    }
}
