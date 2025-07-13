namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(TextBox))]
public sealed class SelectAllTextOnFocusBehavior : Behavior<TextBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.GotKeyboardFocus += SelectAllText;
        AssociatedObject.GotMouseCapture += SelectAllText;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.GotKeyboardFocus -= SelectAllText;
        AssociatedObject.GotMouseCapture -= SelectAllText;
    }

    private void SelectAllText(object sender, RoutedEventArgs e)
    {
        AssociatedObject.SelectAll();
    }
}
