namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(DependencyObject))]
public sealed class ExecuteCommandAction : TriggerAction<DependencyObject>
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(IValueConverter),
        typeof(ExecuteCommandAction));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(IValueConverter),
        typeof(ExecuteCommandAction));

    public static readonly DependencyProperty ConverterProperty = DependencyProperty.Register(
        nameof(Converter),
        typeof(IValueConverter),
        typeof(ExecuteCommandAction));

    public static readonly DependencyProperty ConverterParameterProperty = DependencyProperty.Register(
        nameof(ConverterParameter),
        typeof(object),
        typeof(ExecuteCommandAction));

    public ICommand? Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public IValueConverter? Converter
    {
        get => (IValueConverter)GetValue(ConverterProperty);
        set => SetValue(ConverterProperty, value);
    }

    public object? ConverterParameter
    {
        get => GetValue(ConverterParameterProperty);
        set => SetValue(ConverterParameterProperty, value);
    }

    protected override void Invoke(object parameter)
    {
        var command = Command;
        if (command is null)
        {
            return;
        }

        var commandParameter = CommandParameter;
        var argument = (commandParameter is not null) || this.IsSet(CommandParameterProperty)
            ? commandParameter
            : Converter?.Convert(parameter, typeof(object), ConverterParameter, null) ?? parameter;
        if (command.CanExecute(argument))
        {
            command.Execute(argument);
        }
    }
}
