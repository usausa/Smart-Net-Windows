namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    using Smart.Windows.Expressions;

    [ValueConversion(typeof(object), typeof(Brush))]
    public sealed class CompareToBrushConverter : IValueConverter
    {
        public ICompareExpression Expression { get; set; } = CompareExpressions.Equal;

        public Brush TrueValue { get; set; } = Brushes.Transparent;

        public Brush FalseValue { get; set; } = Brushes.Transparent;

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
