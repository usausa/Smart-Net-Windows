namespace Smart.Windows.Interactivity;

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

    protected override void Invoke(object parameter)
    {
        var method = TargetObject.GetType().GetMethod(MethodName, BindingFlags.Instance | BindingFlags.Public);
        if (method is not null)
        {
            var result = method.Invoke(TargetObject, null);
            Clipboard.SetData(Format, result!);
        }
    }
}
