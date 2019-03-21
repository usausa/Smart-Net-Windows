namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Smart.Windows.Expressions;

    [ValueConversion(typeof(object), typeof(string))]
    public sealed class CompareToTextConverter : IValueConverter
    {
        public ICompareExpression Expression { get; set; } = CompareExpressions.Equal;

        public string TrueValue { get; set; }

        public string FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Expression.Eval(value, parameter) ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
