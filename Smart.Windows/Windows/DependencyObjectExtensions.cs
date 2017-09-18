namespace Smart.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    ///
    /// </summary>
    public static class DependencyObjectExtensions
    {
        // ------------------------------------------------------------
        // Parent
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DependencyObject Parent(this DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            // ContentElement
            if (obj is ContentElement contentElement)
            {
                var parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                return (contentElement as FrameworkContentElement)?.Parent;
            }

            // FrameworkElement
            if (obj is FrameworkElement frameworkElement)
            {
                var parent = frameworkElement.Parent;
                if (parent != null)
                {
                    return parent;
                }
            }

            return VisualTreeHelper.GetParent(obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            while (true)
            {
                var parent = Parent(obj);
                if (parent == null)
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> Children(this DependencyObject obj)
        {
            if (obj == null)
            {
                yield break;
            }

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

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindChildrens<T>(this DependencyObject source)
            where T : DependencyObject
        {
            if (source == null)
            {
                yield break;
            }

            foreach (var child in Children(source))
            {
                if (child is T typedChild)
                {
                    yield return typedChild;
                }

                foreach (var descendant in FindChildrens<T>(child))
                {
                    yield return descendant;
                }
            }
        }
    }
}
