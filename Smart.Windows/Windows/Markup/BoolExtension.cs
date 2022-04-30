namespace Smart.Windows.Markup;

using System.Windows.Markup;

[ContentProperty("Value")]
[MarkupExtensionReturnType(typeof(bool))]
public sealed class BoolExtension : MarkupExtension
{
    public bool Value { get; set; }

    public BoolExtension(bool value)
    {
        Value = value;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}
