namespace Smart.Windows.Markup;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;

[ContentProperty("Type")]
[MarkupExtensionReturnType(typeof(IEnumerable))]
public sealed class EnumValuesExtension : MarkupExtension
{
    public Type Type { get; set; }

    public EnumValuesExtension(Type type)
    {
        Type = type;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2111", Justification = "Enum type is specified by user; they must ensure it is preserved")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Enum.GetValues(Type) uses dynamic code; use Enum.GetValues<TEnum>() for AOT-safe usage")]
    public override object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(Type);
}
