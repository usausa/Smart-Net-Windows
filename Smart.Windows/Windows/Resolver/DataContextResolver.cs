namespace Smart.Windows.Resolver
{
    using System;
    using System.Windows;

    /// <summary>
    ///
    /// </summary>
    public static class DataContextResolver
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.RegisterAttached(
            "Type",
            typeof(Type),
            typeof(DataContextResolver),
            new PropertyMetadata(null, HandleTypePropertyChanged));

        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty DisposeOnChangedProperty = DependencyProperty.RegisterAttached(
            "DisposeOnChanged",
            typeof(bool),
            typeof(DataContextResolver),
            new PropertyMetadata(true));

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Type GetType(DependencyObject obj)
        {
            return (Type)obj.GetValue(TypeProperty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetType(DependencyObject obj, Type value)
        {
            obj.SetValue(TypeProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetDisposeOnChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisposeOnChangedProperty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetDisposeOnChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(DisposeOnChangedProperty, value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void HandleTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (element.DataContext is IDisposable disposable && GetDisposeOnChanged(d))
                {
                    disposable.Dispose();
                }

                element.DataContext = e.NewValue != null ? ResolveProvider.Default.Resolve((Type)e.NewValue) : null;
            }
        }
    }
}
