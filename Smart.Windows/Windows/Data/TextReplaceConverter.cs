namespace Smart.Windows.Data
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(string))]
    public sealed class TextReplaceConverter : IValueConverter
    {
        public string Pattern { get; set; }

        public string Replacement { get; set; }

        public RegexOptions Options { get; set; }

        public bool ReplaceAll { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (String.IsNullOrEmpty(str))
            {
                return value;
            }

            var regex = new Regex(Pattern, Options);
            return regex.Replace(str, Replacement ?? string.Empty, ReplaceAll ? -1 : 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
