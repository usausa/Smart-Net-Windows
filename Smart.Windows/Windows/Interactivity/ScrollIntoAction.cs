namespace Smart.Windows.Interactivity;

using System.Windows;
using System.Windows.Controls;

using Microsoft.Xaml.Behaviors;

[TypeConstraint(typeof(ListBox))]
public sealed class ScrollIntoAction : TriggerAction<ListBox>
{
    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
        nameof(Position),
        typeof(ScrollPosition),
        typeof(ScrollIntoAction),
        new PropertyMetadata(ScrollPosition.Last));

    public ScrollPosition Position
    {
        get => (ScrollPosition)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    protected override void Invoke(object parameter)
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
