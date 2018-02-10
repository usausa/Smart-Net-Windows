namespace Smart.Windows.Interactivity
{
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(ListBox))]
    public sealed class ScrollIntoLastItemAction : TriggerAction<ListBox>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
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
