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
            var contentElement = obj as ContentElement;
            if (contentElement != null)
            {
                var parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                return (contentElement as FrameworkContentElement)?.Parent;
            }

            // FrameworkElement
            var frameworkElement = obj as FrameworkElement;
            if (frameworkElement != null)
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
        /// TODO
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

                var typedParent = parent as T;
                if (typedParent != null)
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
                    var typedChild = child as DependencyObject;
                    if (typedChild != null)
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
        /// TODO
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
                var typedChild = child as T;
                if (typedChild != null)
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
