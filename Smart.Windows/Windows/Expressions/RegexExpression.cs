namespace Smart.Windows.Expressions;

using System.Text.RegularExpressions;

public sealed class RegexExpression : ICompareExpression
{
    private Regex? regex;

    public string Pattern { get; set => (field, regex) = (value, null); } = string.Empty;

    public RegexOptions Options { get; set => (field, regex) = (value, null); }

    public bool Eval(object? left, object? right)
    {
        if (left is not string text)
        {
            return false;
        }

        regex ??= new Regex(Pattern, Options);
        return regex.IsMatch(text);
    }
}
