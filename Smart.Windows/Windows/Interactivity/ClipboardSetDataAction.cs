namespace Smart.Windows.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(DependencyObject))]
public sealed class ClipboardSetDataAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(ClipboardSetDataAction));

    public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        nameof(MethodName),
        typeof(string),
        typeof(ClipboardSetDataAction));

    public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
        nameof(Format),
        typeof(string),
        typeof(ClipboardSetDataAction));

    public object TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string MethodName
    {
        get => (string)GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }

    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    private MethodInfo? cachedMethod;

    private Type? cachedType;

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Target type is determined at runtime via XAML; callers must ensure the type is preserved")]
    protected override void Invoke(object parameter)
    {
        var target = TargetObject;
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return;
        }

        if ((cachedMethod is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethod.Name != methodName))
        {
            cachedMethod = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            cachedType = target.GetType();
            if (cachedMethod is null)
            {
                return;
            }
        }

        var result = cachedMethod.Invoke(target, null);
        if (result is null)
        {
            return;
        }

        Clipboard.SetData(Format, result);
    }
}
