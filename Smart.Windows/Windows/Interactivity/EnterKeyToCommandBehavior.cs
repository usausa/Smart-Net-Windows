namespace Smart.Windows.Interactivity
{
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    public sealed class EnterKeyToCommandBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(EnterKeyToCommandBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(EnterKeyToCommandBehavior),
            new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= OnKeyDown;

            base.OnDetaching();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Command is null)
            {
                return;
            }

            if (e.Key == Key.Enter)
            {
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }

                e.Handled = true;
            }
        }
    }
}
