namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(TextBox))]
public sealed class TypingStoppedBehavior : Behavior<TextBox>
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(TypingStoppedBehavior));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(object),
        typeof(TypingStoppedBehavior));

    public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
        nameof(Delay),
        typeof(TimeSpan),
        typeof(TypingStoppedBehavior),
        new PropertyMetadata(TimeSpan.FromSeconds(1)));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public TimeSpan Delay
    {
        get => (TimeSpan)GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }

    private DispatcherTimer? timer;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.TextChanged += OnTextChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.TextChanged -= OnTextChanged;
        if (timer is not null)
        {
            timer.Stop();
            timer.Tick -= OnTick;
            timer = null;
        }

        base.OnDetaching();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (timer is null)
        {
            timer = new DispatcherTimer();
            timer.Tick += OnTick;
        }

        timer.Stop();
        timer.Interval = Delay;
        timer.Start();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        timer?.Stop();

        var command = Command;
        if (command is null)
        {
            return;
        }

        var commandParameter = CommandParameter;
        var parameter = (commandParameter is not null) || (ReadLocalValue(CommandParameterProperty) != DependencyProperty.UnsetValue) ? commandParameter : AssociatedObject.Text;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}
