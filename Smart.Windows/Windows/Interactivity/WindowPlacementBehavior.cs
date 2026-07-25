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

    private bool placed;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.ContentRendered += OnContentRendered;

        TryUpdatePlacement();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Loaded -= OnLoaded;
        AssociatedObject.ContentRendered -= OnContentRendered;

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => TryUpdatePlacement();

    private void OnContentRendered(object? sender, EventArgs e) => TryUpdatePlacement();

    private void TryUpdatePlacement()
    {
        if (placed)
        {
            return;
        }

        var workingArea = SystemParameters.WorkArea;
        placed = WindowPlacementHelper.UpdatePlacement(AssociatedObject, workingArea, Placement, Margin);
    }
}
