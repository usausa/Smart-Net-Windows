namespace Smart.Windows.Interactivity
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class ScrollOnLastItemBehavior : Behavior<ItemsControl>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register("Enabled", typeof(bool), typeof(WindowCloseToHideAction), new PropertyMetadata(true));

        /// <summary>
        ///
        /// </summary>
        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnLoaded;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnLoaded;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var incc = AssociatedObject.ItemsSource as INotifyCollectionChanged;
            if (incc == null)
            {
                return;
            }

            incc.CollectionChanged += OnCollectionChanged;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            var incc = AssociatedObject.ItemsSource as INotifyCollectionChanged;
            if (incc == null)
            {
                return;
            }

            incc.CollectionChanged -= OnCollectionChanged;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                (AssociatedObject as ListBox)?.ScrollIntoView(AssociatedObject.Items[count - 1]);
            }
        }
    }
}
