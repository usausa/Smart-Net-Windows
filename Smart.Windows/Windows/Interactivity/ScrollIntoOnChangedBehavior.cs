namespace Smart.Windows.Interactivity;

using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(ListBox))]
public sealed class ScrollIntoOnChangedBehavior : Behavior<ListBox>
{
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

    private static readonly DependencyProperty ItemsSourceWatcherProperty = DependencyProperty.Register(
        "ItemsSourceWatcher",
        typeof(object),
        typeof(ScrollIntoOnChangedBehavior),
        new PropertyMetadata(null, HandleItemsSourceChanged));

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

    private bool loaded;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.Unloaded += OnUnloaded;

        BindingOperations.SetBinding(
            this,
            ItemsSourceWatcherProperty,
            new Binding { Path = new PropertyPath(ItemsControl.ItemsSourceProperty), Source = AssociatedObject });

        if (AssociatedObject.IsLoaded)
        {
            loaded = true;
            Subscribe(AssociatedObject.ItemsSource as INotifyCollectionChanged);
        }
    }

    protected override void OnDetaching()
    {
        loaded = false;
        Subscribe(null);

        BindingOperations.ClearBinding(this, ItemsSourceWatcherProperty);

        AssociatedObject.Loaded -= OnLoaded;
        AssociatedObject.Unloaded -= OnUnloaded;

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        loaded = true;
        Subscribe(AssociatedObject.ItemsSource as INotifyCollectionChanged);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        loaded = false;
        Subscribe(null);
    }

    private static void HandleItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (ScrollIntoOnChangedBehavior)d;
        if (behavior.loaded)
        {
            behavior.Subscribe(e.NewValue as INotifyCollectionChanged);
        }
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
        var listBox = AssociatedObject;
        if (!Enabled || (listBox is null))
        {
            return;
        }

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var count = listBox.Items.Count;
            if (count == 0)
            {
                return;
            }

            var item = Position == ScrollPosition.First ? listBox.Items[0] : listBox.Items[count - 1];
            if (item is not null)
            {
                listBox.ScrollIntoView(item);
            }
        }
    }
}
