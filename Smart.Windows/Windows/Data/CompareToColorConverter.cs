namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    using Smart.Windows.Expressions;

    [ValueConversion(typeof(object), typeof(Color))]
    public sealed class CompareToColorConverter : IValueConverter
    {
        public ICompareExpression Expression { get; set; } = CompareExpressions.Equal;

        public Color TrueValue { get; set; } = Colors.Transparent;

        public Color FalseValue { get; set; } = Colors.Transparent;

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
