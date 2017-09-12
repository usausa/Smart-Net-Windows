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
        public static T FindFromPoint<T>(this UIElement reference, Point point)
            where T : DependencyObject
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            if (reference.InputHitTest(point) is DependencyObject element)
            {
                if (element is T typeElememt)
                {
                    return typeElememt;
                }

                return element.FindParent<T>();
            }

            return null;
        }
    }
}
