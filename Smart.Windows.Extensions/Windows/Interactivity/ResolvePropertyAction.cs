namespace Smart.Windows.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Mvvm.Messaging;

[TypeConstraint(typeof(DependencyObject))]
public sealed class ResolvePropertyAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(ResolvePropertyAction));

    public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
        nameof(PropertyName),
        typeof(string),
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

    private Type? cachedType;

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Target type is determined at runtime via XAML; callers must ensure the type is preserved")]
    protected override void Invoke(object parameter)
    {
        if (parameter is not ResolveEventArgs args)
        {
            return;
        }

        var target = TargetObject ?? AssociatedObject;
        var propertyName = PropertyName;
        if (String.IsNullOrEmpty(propertyName))
        {
            return;
        }

        if ((cachedProperty is null) ||
            (cachedType != target.GetType()) ||
            (cachedProperty.Name != propertyName))
        {
            cachedProperty = target.GetType().GetRuntimeProperty(propertyName);
            cachedType = target.GetType();
            if (cachedProperty is null)
            {
                return;
            }
        }

        args.Result = cachedProperty.GetValue(target);
    }
}
