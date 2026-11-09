namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(UIElement))]
public sealed class DefaultFocusBehavior : Behavior<UIElement>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        SetFocusedElement(AssociatedObject);

        if (AssociatedObject is FrameworkElement { IsLoaded: false } element)
        {
            element.Loaded += OnLoaded;
        }
        else
        {
            FocusTarget();
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is FrameworkElement element)
        {
            element.Loaded -= OnLoaded;
        }

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ((FrameworkElement)sender).Loaded -= OnLoaded;
        FocusTarget();
    }

    private void FocusTarget()
    {
        var target = FindTarget(AssociatedObject);
        if (target is not null)
        {
            SetFocusedElement(target);
        }
    }

    private static UIElement? FindTarget(DependencyObject parent)
    {
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            if (VisualTreeHelper.GetChild(parent, i) is not UIElement { IsEnabled: true, Visibility: Visibility.Visible } child)
            {
                continue;
            }

            if (DefaultFocus.GetIsTarget(child))
            {
                return child;
            }

            var target = FindTarget(child);
            if (target is not null)
            {
                return target;
            }
        }

        return null;
    }

    private static void SetFocusedElement(UIElement element)
    {
        var scope = FocusManager.GetFocusScope(element);
        FocusManager.SetFocusedElement(scope, element);
    }
}
