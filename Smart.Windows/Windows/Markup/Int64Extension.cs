namespace Smart.Windows.Markup;

using System.Windows.Markup;

[ContentProperty("Value")]
[MarkupExtensionReturnType(typeof(long))]
public sealed class Int64Extension : MarkupExtension
{
    public long Value { get; set; }

    public Int64Extension(long value)
    {
        Value = value;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}
