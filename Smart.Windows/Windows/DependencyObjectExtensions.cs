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
            var parent = obj.Parent();
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

    public static T? FindParent<T>(this DependencyObject obj, Func<T, bool> predicate)
        where T : DependencyObject =>
        obj.FindParent<T, Func<T, bool>>(predicate, static (x, p) => p(x));

    public static T? FindParent<T, TState>(this DependencyObject obj, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject
    {
        var parent = obj.Parent();
        while (parent is not null)
        {
            if ((parent is T typedParent) && predicate(typedParent, state))
            {
                return typedParent;
            }

            parent = parent.Parent();
        }

        return null;
    }

    public static T? FindParentOrSelf<T>(this DependencyObject obj)
        where T : DependencyObject =>
        obj as T ?? obj.FindParent<T>();

    public static T? FindParentOrSelf<T>(this DependencyObject obj, Func<T, bool> predicate)
        where T : DependencyObject =>
        (obj is T self) && predicate(self) ? self : obj.FindParent(predicate);

    public static T? FindParentOrSelf<T, TState>(this DependencyObject obj, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject =>
        (obj is T self) && predicate(self, state) ? self : obj.FindParent(state, predicate);

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
        foreach (var child in source.Children())
        {
            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (var descendant in child.FindChildren<T>())
            {
                yield return descendant;
            }
        }
    }

    public static IEnumerable<T> FindChildren<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject
    {
        foreach (var child in source.FindChildren<T>())
        {
            if (predicate(child))
            {
                yield return child;
            }
        }
    }

    public static IEnumerable<T> FindChildren<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject
    {
        foreach (var child in source.FindChildren<T>())
        {
            if (predicate(child, state))
            {
                yield return child;
            }
        }
    }

    public static T? FindChild<T>(this DependencyObject source)
        where T : DependencyObject =>
        source.FindChildren<T>().FirstOrDefault();

    public static T? FindChild<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject =>
        source.FindChildren<T>().FirstOrDefault(predicate);

    public static T? FindChild<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject
    {
        foreach (var child in source.FindChildren<T>())
        {
            if (predicate(child, state))
            {
                return child;
            }
        }

        return null;
    }

    public static T? FindChildOrSelf<T>(this DependencyObject source)
        where T : DependencyObject =>
        source as T ?? source.FindChild<T>();

    public static T? FindChildOrSelf<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject =>
        (source is T self) && predicate(self) ? self : source.FindChild(predicate);

    public static T? FindChildOrSelf<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject =>
        (source is T self) && predicate(self, state) ? self : source.FindChild(state, predicate);

    // ------------------------------------------------------------
    // Ancestry
    // ------------------------------------------------------------

    public static bool IsDescendantOf(this DependencyObject element, DependencyObject ancestor)
    {
        var current = element.Parent();
        while (current is not null)
        {
            if (current == ancestor)
            {
                return true;
            }

            current = current.Parent();
        }

        return false;
    }
}
