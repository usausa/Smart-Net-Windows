namespace Smart.Windows.Interactivity;

using Smart.Windows.Messaging;

public sealed class EventRequestTrigger : RequestTriggerBase<ParameterEventArgs>
{
    protected override void OnEventRequest(object? sender, ParameterEventArgs e)
    {
        InvokeActions(e.Parameter);
    }
}
