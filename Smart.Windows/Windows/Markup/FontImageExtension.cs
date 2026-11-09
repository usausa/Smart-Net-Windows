namespace Smart.Windows.Markup;

using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

[MarkupExtensionReturnType(typeof(DrawingImage))]
public sealed class FontImageExtension : MarkupExtension
{
    private static readonly FontFamily DefaultFontFamily = new("Segoe Fluent Icons, Segoe MDL2 Assets");

    public string Glyph { get; set; } = string.Empty;

    public FontFamily FontFamily { get; set; } = DefaultFontFamily;

    public double Size { get; set; } = 16;

    public Brush Brush { get; set; } = Brushes.Black;

    public FontWeight FontWeight { get; set; } = FontWeights.Normal;

    public FontStyle FontStyle { get; set; } = FontStyles.Normal;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var brush = Brush.IsFrozen ? Brush : Brush.CloneCurrentValue();
        var text = new FormattedText(Glyph, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), Size, brush, 1);
        var drawing = new DrawingGroup
        {
            Children =
            {
                new GeometryDrawing(Brushes.Transparent, null, new RectangleGeometry(new Rect(0, 0, text.Width, text.Height))),
                new GeometryDrawing(brush, null, text.BuildGeometry(default))
            }
        };

        var image = new DrawingImage(drawing);
        image.Freeze();
        return image;
    }
}
