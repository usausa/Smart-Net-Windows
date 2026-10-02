namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(UIElement))]
public sealed class LongPressBehavior : Behavior<UIElement>
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(LongPressBehavior));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(object),
        typeof(LongPressBehavior));

    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        nameof(Duration),
        typeof(TimeSpan),
        typeof(LongPressBehavior),
        new PropertyMetadata(TimeSpan.FromMilliseconds(500)));

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

    public TimeSpan Duration
    {
        get => (TimeSpan)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    private DispatcherTimer? timer;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.PreviewMouseLeftButtonDown += OnPressed;
        AssociatedObject.PreviewMouseLeftButtonUp += OnReleased;
        AssociatedObject.LostMouseCapture += OnReleased;
        AssociatedObject.MouseLeave += OnReleased;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewMouseLeftButtonDown -= OnPressed;
        AssociatedObject.PreviewMouseLeftButtonUp -= OnReleased;
        AssociatedObject.LostMouseCapture -= OnReleased;
        AssociatedObject.MouseLeave -= OnReleased;
        if (timer is not null)
        {
            timer.Stop();
            timer.Tick -= OnTick;
            timer = null;
        }

        base.OnDetaching();
    }

    private void OnPressed(object sender, MouseButtonEventArgs e)
    {
        if (timer is null)
        {
            timer = new DispatcherTimer();
            timer.Tick += OnTick;
        }

        timer.Stop();
        timer.Interval = Duration;
        timer.Start();
    }

    private void OnReleased(object sender, MouseEventArgs e)
    {
        timer?.Stop();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        timer?.Stop();

        var command = Command;
        if (command is null)
        {
            return;
        }

        var parameter = CommandParameter;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}
