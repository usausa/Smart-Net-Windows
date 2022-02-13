namespace Smart.Windows;

using System.Windows;

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
}
