namespace Smart.Windows.Interactivity
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(ListBox))]
    public sealed class ListBoxSelectedItemsBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(ICollection),
            typeof(ListBoxSelectedItemsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Ignore")]
        public ICollection SelectedItems
        {
            get => (ICollection)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
            SelectedItems = AssociatedObject.SelectedItems;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;

            base.OnDetaching();
        }

        private void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = AssociatedObject.SelectedItems;
        }
    }
}
