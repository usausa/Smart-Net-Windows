namespace Smart.Windows.Interactivity
{
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(ListBox))]
    public sealed class ScrollIntoLastItemAction : TriggerAction<ListBox>
    {
        protected override void Invoke(object parameter)
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
