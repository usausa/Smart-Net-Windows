namespace Smart.Windows
{
    using System;
    using System.Windows;

    /// <summary>
    ///
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static T FindFromPoint<T>(this UIElement reference, Point point) where T : DependencyObject
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var element = reference.InputHitTest(point) as DependencyObject;
            if (element == null)
            {
                return null;
            }

            var typeElememt = element as T;
            if (typeElememt != null)
            {
                return typeElememt;
            }

            return element.FindParent<T>();
        }
    }
}
