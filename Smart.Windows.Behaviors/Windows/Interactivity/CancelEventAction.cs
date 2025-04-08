namespace Smart.Windows.Interactivity;

using System.ComponentModel;
using System.Windows;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(DependencyObject))]
public sealed class CancelEventAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty CancelProperty = DependencyProperty.Register(
        nameof(Cancel),
        typeof(bool),
        typeof(CancelEventAction),
        new PropertyMetadata(false));

    public bool Cancel
    {
        get => (bool)GetValue(CancelProperty);
        set => SetValue(CancelProperty, value);
    }

    protected override void Invoke(object parameter)
    {
        var args = (CancelEventArgs)parameter;
        args.Cancel = Cancel;
    }
}
