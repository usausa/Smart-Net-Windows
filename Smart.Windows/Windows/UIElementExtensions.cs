namespace Smart.Windows;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public static class UIElementExtensions
{
    public static T? FindFromPoint<T>(this UIElement reference, Point point)
        where T : DependencyObject
    {
        if (reference.InputHitTest(point) is DependencyObject element)
        {
            if (element is T typeElement)
            {
                return typeElement;
            }

            return element.FindParent<T>();
        }

        return null;
    }

    public static BitmapSource RenderToBitmap(this UIElement element) =>
        element.RenderToBitmap(1d);

    public static BitmapSource RenderToBitmap(this UIElement element, double scale)
    {
        var renderWidth = (int)(element.RenderSize.Width * scale);
        var renderHeight = (int)(element.RenderSize.Height * scale);

        var renderTarget = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Pbgra32);
        var sourceBrush = new VisualBrush(element);

        var drawingVisual = new DrawingVisual();
        using (var drawingContext = drawingVisual.RenderOpen())
        {
            drawingContext.PushTransform(new ScaleTransform(scale, scale));
            drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), element.RenderSize));
        }

        renderTarget.Render(drawingVisual);
        return renderTarget;
    }
}
