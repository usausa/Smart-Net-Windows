namespace Smart.Windows.Interactivity;

using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(ListBox))]
public sealed class ScrollIntoOnChangedBehavior : Behavior<ListBox>
{
    private static readonly DependencyPropertyDescriptor ItemsSourceDescriptor =
        DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ListBox))!;

    public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
        nameof(Enabled),
        typeof(bool),
        typeof(ScrollIntoOnChangedBehavior),
        new PropertyMetadata(true));

    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
        nameof(Position),
        typeof(ScrollPosition),
        typeof(ScrollIntoOnChangedBehavior),
        new PropertyMetadata(ScrollPosition.Last));

    public bool Enabled
    {
        get => (bool)GetValue(EnabledProperty);
        set => SetValue(EnabledProperty, value);
    }

    public ScrollPosition Position
    {
        get => (ScrollPosition)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    private INotifyCollectionChanged? subscribedCollection;

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
        ItemsSourceDescriptor.AddValueChanged(AssociatedObject, OnItemsSourceChanged);
        Subscribe(AssociatedObject.ItemsSource as INotifyCollectionChanged);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        ItemsSourceDescriptor.RemoveValueChanged(AssociatedObject, OnItemsSourceChanged);
        Subscribe(null);
    }

    private void OnItemsSourceChanged(object? sender, EventArgs e)
    {
        Subscribe(AssociatedObject.ItemsSource as INotifyCollectionChanged);
    }

    private void Subscribe(INotifyCollectionChanged? collection)
    {
        if (ReferenceEquals(subscribedCollection, collection))
        {
            return;
        }

        if (subscribedCollection is not null)
        {
            subscribedCollection.CollectionChanged -= OnCollectionChanged;
        }

        subscribedCollection = collection;

        if (subscribedCollection is not null)
        {
            subscribedCollection.CollectionChanged += OnCollectionChanged;
        }
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
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

            var item = Position == ScrollPosition.First ? AssociatedObject.Items[0] : AssociatedObject.Items[count - 1];
            if (item is not null)
            {
                AssociatedObject.ScrollIntoView(item);
            }
        }
    }
}
