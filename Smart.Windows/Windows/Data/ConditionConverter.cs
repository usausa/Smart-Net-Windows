namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(object), typeof(object))]
    public class ConditionConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        public Func<object, bool> Predicate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object TrueValue { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object FalseValue { get; set; }

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
            return (Predicate?.Invoke(value) ?? (bool)value) ? TrueValue : FalseValue;
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
            return Equals(value, TrueValue);
        }
    }
}
