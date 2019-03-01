namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Smart.Windows.Operation;

    /// <summary>
    ///
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class CompareConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        public CompareOperator Operator { get; set; } = CompareOperator.Equal;

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
            return CompareOperatorEvaluator.Compare(Operator, value, parameter);
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
