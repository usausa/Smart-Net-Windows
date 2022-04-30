namespace Smart.Windows.Markup;

using System.Windows.Markup;
using System.Windows.Media;

using Smart.Windows.Data;

[MarkupExtensionReturnType(typeof(ColorBlendConverter))]
public sealed class ColorBlendExtension : MarkupExtension
{
    public Color Color { get; set; }

    public double Raito { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ColorBlendConverter { Color = Color, Raito = Raito };
}
