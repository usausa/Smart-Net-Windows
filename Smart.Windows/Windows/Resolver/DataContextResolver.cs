namespace Smart.Windows.Resolver
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    public static class DataContextResolver
    {
        public static readonly DependencyProperty TypeProperty = DependencyProperty.RegisterAttached(
            "Type",
            typeof(Type),
            typeof(DataContextResolver),
            new PropertyMetadata(HandleTypePropertyChanged));

        public static readonly DependencyProperty DisposeOnChangedProperty = DependencyProperty.RegisterAttached(
            "DisposeOnChanged",
            typeof(bool),
            typeof(DataContextResolver),
            new PropertyMetadata(true));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static Type GetType(DependencyObject obj)
        {
            return (Type)obj.GetValue(TypeProperty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static void SetType(DependencyObject obj, Type value)
        {
            obj.SetValue(TypeProperty, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static bool GetDisposeOnChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisposeOnChangedProperty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static void SetDisposeOnChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(DisposeOnChangedProperty, value);
        }

        private static void HandleTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
            {
                return;
            }

            if (d is FrameworkElement element)
            {
                if (element.DataContext is IDisposable disposable && GetDisposeOnChanged(d))
                {
                    disposable.Dispose();
                }

                element.DataContext = e.NewValue is not null ? ResolveProvider.Default.Resolve((Type)e.NewValue) : null;
            }
        }
    }
}
