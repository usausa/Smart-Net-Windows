namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(Window))]
public sealed class DragMoveBehavior : Behavior<Window>
{
    public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
        nameof(Enabled),
        typeof(bool),
        typeof(DragMoveBehavior),
        new PropertyMetadata(true));

    public bool Enabled
    {
        get => (bool)GetValue(EnabledProperty);
        set => SetValue(EnabledProperty, value);
    }

    protected override void OnAttached()
    {
        AssociatedObject.MouseDown += OnMouseDown;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseDown -= OnMouseDown;
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs args)
    {
        if (Enabled && (args.LeftButton == MouseButtonState.Pressed))
        {
            AssociatedObject.DragMove();
        }
    }
}
