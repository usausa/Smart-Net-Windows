namespace Smart.Windows.Interactivity;

using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

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
        if (AssociatedObject.ItemsSource is INotifyCollectionChanged ncc)
        {
            ncc.CollectionChanged += OnCollectionChanged;
        }
    }

    private void OnUnLoaded(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.ItemsSource is INotifyCollectionChanged ncc)
        {
            ncc.CollectionChanged -= OnCollectionChanged;
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
