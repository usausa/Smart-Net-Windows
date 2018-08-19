namespace Smart.Windows.Interactivity
{
    using Smart.Windows.Messaging;

    public sealed class EventRequestTrigger : RequestTriggerBase<EventEventArgs>
    {
        protected override void OnEventRequest(object sender, EventEventArgs e)
        {
            InvokeActions(e.Value);
        }
    }
}
