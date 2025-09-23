namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(Window))]
public sealed class WindowPlacementAction : TriggerAction<Window>
{
    public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
        nameof(Placement),
        typeof(WindowPlacement),
        typeof(WindowPlacementAction),
        new PropertyMetadata(WindowPlacement.Center));

    public WindowPlacement Placement
    {
        get => (WindowPlacement)GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public static readonly DependencyProperty MarginProperty = DependencyProperty.Register(
        nameof(Margin),
        typeof(Thickness),
        typeof(WindowPlacementAction),
        new PropertyMetadata(new Thickness(0)));

    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    protected override void Invoke(object parameter)
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var workingArea = SystemParameters.WorkArea;
        WindowPlacementHelper.UpdatePlacement(AssociatedObject, workingArea, Placement, Margin);
    }
}
