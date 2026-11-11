namespace Smart.Windows.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;

using Microsoft.Xaml.Behaviors;

using Smart.Linq;
using Smart.Mvvm.Messaging;

[TypeConstraint(typeof(DependencyObject))]
public sealed class ResolveMethodAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(ResolveMethodAction));

    public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        nameof(MethodName),
        typeof(string),
        typeof(ResolveMethodAction),
        new PropertyMetadata(string.Empty));

    public object? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string MethodName
    {
        get => (string)GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }

    private MethodInfo? cachedMethod;

    private Type? cachedType;

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Target type is determined at runtime via XAML; callers must ensure the type is preserved")]
    protected override void Invoke(object parameter)
    {
        if (parameter is not ResolveEventArgs args)
        {
            return;
        }

        var target = TargetObject ?? AssociatedObject;
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return;
        }

        if ((cachedMethod is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethod.Name != methodName))
        {
            cachedMethod = target.GetType().GetRuntimeMethods()
                .FirstOrDefault(methodName, static (m, s) => (m.Name == s) && (m.GetParameters().Length == 0));
            if (cachedMethod is null)
            {
                return;
            }

            cachedType = target.GetType();
        }

        args.Result = cachedMethod.Invoke(target, null);
    }
}
