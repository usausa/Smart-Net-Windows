namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(Window))]
public sealed class WindowPlacementBehavior : Behavior<Window>
{
    public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
        nameof(Placement),
        typeof(WindowPlacement),
        typeof(WindowPlacementBehavior),
        new PropertyMetadata(WindowPlacement.TopLeft));

    public WindowPlacement Placement
    {
        get => (WindowPlacement)GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public static readonly DependencyProperty MarginProperty = DependencyProperty.Register(
        nameof(Margin),
        typeof(Thickness),
        typeof(WindowPlacementBehavior),
        new PropertyMetadata(new Thickness(0)));

    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        var workingArea = SystemParameters.WorkArea;
        WindowPlacementHelper.UpdatePlacement(AssociatedObject, workingArea, Placement, Margin);
    }
}
