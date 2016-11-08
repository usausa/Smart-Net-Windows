namespace Smart.Windows.Interactivity
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class ListBoxSelectedItemsBehavior : Behavior<ListBox>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ICollection), typeof(ListBoxSelectedItemsBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        ///
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Ignore")]
        public ICollection SelectedItems
        {
            get { return (ICollection)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
            SelectedItems = AssociatedObject.SelectedItems;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = AssociatedObject.SelectedItems;
        }
    }
}
