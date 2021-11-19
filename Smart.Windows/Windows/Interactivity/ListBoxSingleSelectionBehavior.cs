namespace Smart.Windows.Interactivity;

using System.Collections;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

public sealed class ListBoxSingleSelectionBehavior : Behavior<ListBox>
{
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(object),
        typeof(ListBoxSingleSelectionBehavior),
        new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null)
        {
            return;
        }

        AssociatedObject.SelectionMode = SelectionMode.Multiple;
        AssociatedObject.SelectionChanged += OnSelectionChanged;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        base.OnDetaching();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox)
        {
            if (e.AddedItems.Count > 0)
            {
                var target = e.AddedItems[0];
                foreach (var item in new ArrayList(listBox.SelectedItems))
                {
                    if (item != target)
                    {
                        listBox.SelectedItems.Remove(item);
                    }
                }
            }

            SelectedItem = listBox.SelectedItems.Count > 0 ? listBox.SelectedItems[0] : null;
        }
    }
}
