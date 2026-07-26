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

    private static readonly DependencyProperty ResolvedProperty = DependencyProperty.RegisterAttached(
        "Resolved",
        typeof(object),
        typeof(DataContextResolver));

    public static readonly DependencyProperty DisposeOnChangedProperty = DependencyProperty.RegisterAttached(
        "DisposeOnChanged",
        typeof(bool),
        typeof(DataContextResolver),
        new PropertyMetadata(true));

    public static Type GetType(DependencyObject obj) =>
        (Type)obj.GetValue(TypeProperty);

    public static void SetType(DependencyObject obj, Type value) =>
        obj.SetValue(TypeProperty, value);

    public static bool GetDisposeOnChanged(DependencyObject obj) =>
        (bool)obj.GetValue(DisposeOnChangedProperty);

    public static void SetDisposeOnChanged(DependencyObject obj, bool value) =>
        obj.SetValue(DisposeOnChangedProperty, value);

    private static void HandleTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d))
        {
            return;
        }

        if (d is FrameworkElement element)
        {
            var context = e.NewValue is not null ? ResolveHelper.Resolve((Type)e.NewValue) : null;

            var resolved = element.GetValue(ResolvedProperty);
            element.SetValue(ResolvedProperty, context);
            element.DataContext = context;

            if (GetDisposeOnChanged(d) &&
                !ReferenceEquals(resolved, context) &&
                resolved is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
