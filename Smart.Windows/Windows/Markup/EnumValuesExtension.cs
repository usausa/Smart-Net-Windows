namespace Smart.Windows.Markup;

using System.Collections;
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

    public override object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(Type);
}
