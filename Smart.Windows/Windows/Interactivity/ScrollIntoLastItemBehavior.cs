namespace Smart.Windows.Interactivity
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(ListBox))]
    public sealed class ScrollIntoLastItemBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
            nameof(Enabled),
            typeof(bool),
            typeof(ScrollIntoLastItemBehavior),
            new PropertyMetadata(true));

        public bool Enabled
        {
            get => (bool)GetValue(EnabledProperty);
            set => SetValue(EnabledProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.ItemsSource is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += OnCollectionChanged;
            }
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.ItemsSource is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged -= OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var count = AssociatedObject.Items.Count;
                if (count == 0)
                {
                    return;
                }

                AssociatedObject.ScrollIntoView(AssociatedObject.Items[count - 1]);
            }
        }
    }
}
