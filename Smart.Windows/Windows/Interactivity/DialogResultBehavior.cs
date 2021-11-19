namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(Button))]
public sealed class DialogResultBehavior : Behavior<Button>
{
    public static readonly DependencyProperty DialogResultProperty = DependencyProperty.Register(
        nameof(DialogResult),
        typeof(bool),
        typeof(DialogResultBehavior),
        new FrameworkPropertyMetadata(true));

    public bool DialogResult
    {
        get => (bool)GetValue(DialogResultProperty);
        set => SetValue(DialogResultProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Click += OnButtonClick;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Click -= OnButtonClick;

        base.OnDetaching();
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        var window = AssociatedObject.FindParent<Window>();
        if (window is not null)
        {
            window.DialogResult = DialogResult;
        }
    }
}
