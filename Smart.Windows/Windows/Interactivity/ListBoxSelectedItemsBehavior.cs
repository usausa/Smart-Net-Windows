namespace Smart.Windows.Interactivity;

using System.Collections;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(ListBox))]
public sealed class ListBoxSelectedItemsBehavior : Behavior<ListBox>
{
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
        nameof(SelectedItems),
        typeof(ICollection),
        typeof(ListBoxSelectedItemsBehavior),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

#pragma warning disable CA2227
    public ICollection SelectedItems
    {
        get => (ICollection)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }
#pragma warning restore CA2227

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
