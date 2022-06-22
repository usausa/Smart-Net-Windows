namespace Smart.Windows.Resolver;

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

    public static Type GetType(DependencyObject obj)
    {
        return (Type)obj.GetValue(TypeProperty);
    }

    public static void SetType(DependencyObject obj, Type value)
    {
        obj.SetValue(TypeProperty, value);
    }

    public static bool GetDisposeOnChanged(DependencyObject obj)
    {
        return (bool)obj.GetValue(DisposeOnChangedProperty);
    }

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

            element.DataContext = e.NewValue is not null ? ResolveProvider.Default.GetService((Type)e.NewValue) : null;
        }
    }
}
