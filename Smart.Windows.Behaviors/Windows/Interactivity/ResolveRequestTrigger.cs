namespace Smart.Windows.Interactivity;

using Smart.Windows.Messaging;

public sealed class ResolveRequestTrigger : RequestTriggerBase<ResolveEventArgs>
{
    protected override void OnEventRequest(object? sender, ResolveEventArgs e)
    {
        InvokeActions(e);
    }
}
