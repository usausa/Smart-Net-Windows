namespace Smart.Windows.Interactivity;

using System.Windows;

public static class WindowPlacementHelper
{
    public static bool UpdatePlacement(Window window, Rect area, WindowPlacement placement, Thickness margin = default)
    {
        var windowWidth = window.ActualWidth;
        var windowHeight = window.ActualHeight;

        if (Double.IsNaN(windowWidth) || (windowWidth <= 0))
        {
            windowWidth = window.Width;
        }
        if (Double.IsNaN(windowHeight) || (windowHeight <= 0))
        {
            windowHeight = window.Height;
        }

        if (Double.IsNaN(windowWidth) || Double.IsNaN(windowHeight) || (windowWidth <= 0) || (windowHeight <= 0))
        {
            return false;
        }

        switch (placement)
        {
            case WindowPlacement.TopLeft:
                window.Left = area.Left + margin.Left;
                window.Top = area.Top + margin.Top;
                break;
            case WindowPlacement.TopRight:
                window.Left = area.Right - windowWidth - margin.Right;
                window.Top = area.Top + margin.Top;
                break;
            case WindowPlacement.BottomLeft:
                window.Left = area.Left + margin.Left;
                window.Top = area.Bottom - windowHeight - margin.Bottom;
                break;
            case WindowPlacement.BottomRight:
                window.Left = area.Right - windowWidth - margin.Right;
                window.Top = area.Bottom - windowHeight - margin.Bottom;
                break;
            case WindowPlacement.Center:
                window.Left = area.Left + ((area.Width - windowWidth) / 2);
                window.Top = area.Top + ((area.Height - windowHeight) / 2);
                break;
        }

        return true;
    }
}
