namespace Smart.Windows.Markup;

using System.Windows.Markup;

[ContentProperty("Value")]
[MarkupExtensionReturnType(typeof(short))]
public sealed class Int16Extension : MarkupExtension
{
    public short Value { get; set; }

    public Int16Extension(short value)
    {
        Value = value;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}
