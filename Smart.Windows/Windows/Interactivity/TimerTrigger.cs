namespace Smart.Windows.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    using Microsoft.Xaml.Behaviors;

    [TypeConstraint(typeof(FrameworkElement))]
    public sealed class TimerTrigger : TriggerBase<FrameworkElement>
    {
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            nameof(Interval),
            typeof(TimeSpan),
            typeof(TimerTrigger),
            new FrameworkPropertyMetadata(default(TimeSpan)));

        public TimeSpan Interval
        {
            get => (TimeSpan)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        private DispatcherTimer timer;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;

            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            StopTimer();
            StartTimer();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }

        private void StartTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = Interval
            };
            timer.Tick += OnTick;
            timer.Start();
        }

        private void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            InvokeActions(null);
        }
    }
}
