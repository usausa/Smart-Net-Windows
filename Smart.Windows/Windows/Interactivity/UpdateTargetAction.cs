namespace Smart.Windows.Interactivity;

using System.ComponentModel;
using System.Windows;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class UpdateTargetAction : TriggerAction<FrameworkElement>
{
    public static readonly DependencyProperty PropertyNameProperty =
        DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(UpdateTargetAction));

    private DependencyPropertyDescriptor? dpd;

    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        UpdatePropertyDescriptor();
    }

    protected override void OnDetaching()
    {
        dpd = null;

        base.OnDetaching();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        UpdatePropertyDescriptor();
    }

    private void UpdatePropertyDescriptor()
    {
        dpd = null;
        if ((AssociatedObject is not null) && !String.IsNullOrEmpty(PropertyName))
        {
            dpd = DependencyPropertyDescriptor.FromName(PropertyName, AssociatedObject.GetType(), AssociatedObject.GetType());
        }
    }

    protected override void Invoke(object parameter)
    {
        if (dpd is null)
        {
            return;
        }

        var binding = AssociatedObject.GetBindingExpression(dpd.DependencyProperty);
        binding?.UpdateTarget();
    }
}
