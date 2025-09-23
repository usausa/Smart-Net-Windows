namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(Window))]
public sealed class ContextMenuBehavior : Behavior<Window>
{
    public static readonly DependencyProperty MenuProperty =
        DependencyProperty.Register(
            nameof(Menu),
            typeof(ContextMenu),
            typeof(ContextMenuBehavior),
            new PropertyMetadata(null));

    public ContextMenu? Menu
    {
        get => (ContextMenu?)GetValue(MenuProperty);
        set => SetValue(MenuProperty, value);
    }

    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.Register(
            nameof(IsEnabled),
            typeof(bool),
            typeof(ContextMenuBehavior),
            new PropertyMetadata(true));

    public bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.MouseRightButtonUp += OnMouseRightButtonUp;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseRightButtonUp -= OnMouseRightButtonUp;

        base.OnDetaching();
    }

    private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        if ((Menu is null) || !IsEnabled)
        {
            return;
        }

        Menu.PlacementTarget = AssociatedObject;
        Menu.IsOpen = true;
    }
}
