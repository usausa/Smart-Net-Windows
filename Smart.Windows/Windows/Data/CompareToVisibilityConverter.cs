namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using Smart.Windows.Expressions;

    [ValueConversion(typeof(object), typeof(Visibility))]
    public sealed class CompareToVisibilityConverter : IValueConverter
    {
        public ICompareExpression Expression { get; set; } = CompareExpressions.Equal;

        public Visibility TrueValue { get; set; }

        public Visibility FalseValue { get; set; }

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
