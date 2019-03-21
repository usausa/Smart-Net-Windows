namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Smart.Windows.Expressions;

    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class CompareConverter : IValueConverter
    {
        public ICompareExpression Expression { get; set; } = CompareExpressions.Equal;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Expression.Eval(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
