namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class ChangeCursorAction : TriggerAction<FrameworkElement>
{
    public static readonly DependencyProperty CursorProperty = DependencyProperty.Register(
        nameof(Cursor),
        typeof(Cursor),
        typeof(ChangeCursorAction));

    public static readonly DependencyProperty ApplicationWideProperty = DependencyProperty.Register(
        nameof(ApplicationWide),
        typeof(bool),
        typeof(ChangeCursorAction),
        new PropertyMetadata(false));

    public Cursor Cursor
    {
        get => (Cursor)GetValue(CursorProperty);
        set => SetValue(CursorProperty, value);
    }

    public bool ApplicationWide
    {
        get => (bool)GetValue(ApplicationWideProperty);
        set => SetValue(ApplicationWideProperty, value);
    }

    protected override void Invoke(object parameter)
    {
        if (ApplicationWide)
        {
            Mouse.OverrideCursor = Cursor;
        }
        else
        {
            AssociatedObject.Cursor = Cursor;
        }
    }
}
