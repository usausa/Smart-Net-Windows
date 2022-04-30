namespace Smart.Windows.Interactivity;

using System.Reflection;
using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Windows.Messaging;

[TypeConstraint(typeof(DependencyObject))]
public sealed class ResolvePropertyAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(ResolvePropertyAction));

    public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
        nameof(PropertyName),
        typeof(object),
        typeof(ResolvePropertyAction),
        new PropertyMetadata(string.Empty));

    public object? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    private PropertyInfo? cachedProperty;

    protected override void Invoke(object parameter)
    {
        var target = TargetObject ?? AssociatedObject;
        var propertyName = PropertyName;
        if (String.IsNullOrEmpty(propertyName))
        {
            return;
        }

        if ((cachedProperty is null) ||
            (cachedProperty.DeclaringType != target.GetType()) ||
            (cachedProperty.Name != propertyName))
        {
            cachedProperty = target.GetType().GetRuntimeProperty(propertyName);
            if (cachedProperty is null)
            {
                return;
            }
        }

        var eventArgs = (ResultEventArgs)parameter;
        eventArgs.Result = cachedProperty.GetValue(target);
    }
}
