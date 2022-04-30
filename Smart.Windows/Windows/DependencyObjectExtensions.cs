namespace Smart.Windows;

using System.Windows;
using System.Windows.Media;

public static class DependencyObjectExtensions
{
    // ------------------------------------------------------------
    // Property
    // ------------------------------------------------------------

    public static bool IsSet(this DependencyObject obj, DependencyProperty dp)
    {
        return obj.ReadLocalValue(dp) != DependencyProperty.UnsetValue;
    }

    // ------------------------------------------------------------
    // Parent
    // ------------------------------------------------------------

    public static DependencyObject? Parent(this DependencyObject obj)
    {
        // ContentElement
        if (obj is ContentElement contentElement)
        {
            var parent = ContentOperations.GetParent(contentElement);
            if (parent is not null)
            {
                return parent;
            }

            if (contentElement is FrameworkContentElement frameworkContentElement)
            {
                return frameworkContentElement.Parent;
            }

            return null;
        }

        // FrameworkElement
        if (obj is FrameworkElement frameworkElement)
        {
            var parent = frameworkElement.Parent;
            if (parent is not null)
            {
                return parent;
            }
        }

        return VisualTreeHelper.GetParent(obj);
    }

    public static T? FindParent<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        while (true)
        {
            var parent = Parent(obj);
            if (parent is null)
            {
                return null;
            }

            if (parent is T typedParent)
            {
                return typedParent;
            }

            obj = parent;
        }
    }

    // ------------------------------------------------------------
    // Children
    // ------------------------------------------------------------

    public static IEnumerable<DependencyObject> Children(this DependencyObject obj)
    {
        if ((obj is ContentElement) || (obj is FrameworkElement))
        {
            foreach (var child in LogicalTreeHelper.GetChildren(obj))
            {
                if (child is DependencyObject typedChild)
                {
                    yield return typedChild;
                }
            }
        }
        else
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                yield return VisualTreeHelper.GetChild(obj, i);
            }
        }
    }

    public static IEnumerable<T> FindChildren<T>(this DependencyObject source)
        where T : DependencyObject
    {
        foreach (var child in Children(source))
        {
            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (var descendant in FindChildren<T>(child))
            {
                yield return descendant;
            }
        }
    }
}
