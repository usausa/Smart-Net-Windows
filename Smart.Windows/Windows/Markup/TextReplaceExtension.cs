namespace Smart.Windows.Markup;

using System.Text.RegularExpressions;
using System.Windows.Markup;

using Smart.Windows.Data;

[MarkupExtensionReturnType(typeof(TextReplaceConverter))]
public sealed class TextReplaceExtension : MarkupExtension
{
    public string Pattern { get; set; } = string.Empty;

    public string Replacement { get; set; } = string.Empty;

    public RegexOptions Options { get; set; }

    public bool ReplaceAll { get; set; } = true;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new TextReplaceConverter { Pattern = Pattern, Replacement = Replacement, Options = Options, ReplaceAll = ReplaceAll };
}
