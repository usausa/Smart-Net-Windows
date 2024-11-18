namespace Smart.Windows.Data;

using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

#pragma warning disable IDE0032
[ValueConversion(typeof(string), typeof(string))]
public sealed class TextReplaceConverter : IValueConverter
{
    private string pattern = string.Empty;

    private RegexOptions options;

    private Regex? regex;

    public string Pattern
    {
        get => pattern;
        set
        {
            pattern = value;
            regex = null;
        }
    }

    public RegexOptions Options
    {
        get => options;
        set
        {
            options = value;
            regex = null;
        }
    }

    public string Replacement { get; set; } = string.Empty;

    public bool ReplaceAll { get; set; } = true;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var str = value as string;
        if (String.IsNullOrEmpty(str))
        {
            return value;
        }

        regex ??= new Regex(pattern, options);
        return regex.Replace(str, Replacement, ReplaceAll ? -1 : 1);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
