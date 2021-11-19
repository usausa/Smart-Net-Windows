namespace Smart.Windows.Interactivity;

using Smart.Windows.Messaging;

public sealed class ResolveRequestTrigger : RequestTriggerBase<ResultEventArgs>
{
    protected override void OnEventRequest(object? sender, ResultEventArgs e)
    {
        InvokeActions(e);
    }
}
