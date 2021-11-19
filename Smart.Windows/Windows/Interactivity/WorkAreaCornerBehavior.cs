namespace Smart.Windows.Interactivity;

using System.Windows;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(Window))]
public sealed class WorkAreaCornerBehavior : Behavior<Window>
{
    public static readonly DependencyProperty CornerProperty = DependencyProperty.Register(
        nameof(Corner),
        typeof(WorkAreaCorner),
        typeof(WorkAreaCornerBehavior),
        new PropertyMetadata(WorkAreaCorner.TopLeft));

    public WorkAreaCorner Corner
    {
        get => (WorkAreaCorner)GetValue(CornerProperty);
        set => SetValue(CornerProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        var workArea = SystemParameters.WorkArea;
        switch (Corner)
        {
            case WorkAreaCorner.TopLeft:
                AssociatedObject.Left = workArea.Left;
                AssociatedObject.Top = workArea.Top;
                break;
            case WorkAreaCorner.TopRight:
                AssociatedObject.Left = workArea.Right - AssociatedObject.Width;
                AssociatedObject.Top = workArea.Top;
                break;
            case WorkAreaCorner.BottomLeft:
                AssociatedObject.Left = workArea.Left;
                AssociatedObject.Top = workArea.Bottom - AssociatedObject.Height;
                break;
            case WorkAreaCorner.BottomRight:
                AssociatedObject.Left = workArea.Right - AssociatedObject.Width;
                AssociatedObject.Top = workArea.Bottom - AssociatedObject.Height;
                break;
        }
    }
}
