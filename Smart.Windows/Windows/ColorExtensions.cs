namespace Smart.Windows;

using System.Windows.Media;

public static class ColorExtensions
{
    public static HslColor ToHsl(this Color color)
    {
        var (hue, max, min) = Analyze(color);
        var chroma = max - min;
        var lightness = (max + min) / 2;
        var saturation = chroma > 0 ? chroma / (1 - Math.Abs((2 * lightness) - 1)) : 0;
        return new HslColor(hue, saturation, lightness, color.A / 255d);
    }

    public static HsvColor ToHsv(this Color color)
    {
        var (hue, max, min) = Analyze(color);
        var saturation = max > min ? (max - min) / max : 0;
        return new HsvColor(hue, saturation, max, color.A / 255d);
    }

    public static Color ToColor(this HslColor hsl)
    {
        var saturation = Math.Clamp(hsl.S, 0, 1);
        var lightness = Math.Clamp(hsl.L, 0, 1);
        var chroma = (1 - Math.Abs((2 * lightness) - 1)) * saturation;
        return FromChroma(hsl.H, chroma, lightness - (chroma / 2), hsl.A);
    }

    public static Color ToColor(this HsvColor hsv)
    {
        var saturation = Math.Clamp(hsv.S, 0, 1);
        var value = Math.Clamp(hsv.V, 0, 1);
        var chroma = value * saturation;
        return FromChroma(hsv.H, chroma, value - chroma, hsv.A);
    }

    private static (double Hue, double Max, double Min) Analyze(Color color)
    {
        int r = color.R;
        int g = color.G;
        int b = color.B;
        var max = Math.Max(Math.Max(r, g), b);
        var min = Math.Min(Math.Min(r, g), b);
        var chroma = (double)(max - min);
        var sector = max == min ? 0
            : max == r ? (((g - b) / chroma) + 6) % 6
            : max == g ? ((b - r) / chroma) + 2
            : ((r - g) / chroma) + 4;
        return (sector * 60, max / 255d, min / 255d);
    }

    private static Color FromChroma(double hue, double chroma, double match, double alpha)
    {
        var sector = (((hue % 360) + 360) % 360) / 60;
        var x = chroma * (1 - Math.Abs((sector % 2) - 1));
        var (r, g, b) = sector switch
        {
            < 1 => (chroma, x, 0d),
            < 2 => (x, chroma, 0d),
            < 3 => (0d, chroma, x),
            < 4 => (0d, x, chroma),
            < 5 => (x, 0d, chroma),
            _ => (chroma, 0d, x)
        };
        return Color.FromArgb(ToByte(alpha), ToByte(r + match), ToByte(g + match), ToByte(b + match));
    }

    private static byte ToByte(double value) => (byte)Math.Round(Math.Clamp(value, 0, 1) * 255);
}
