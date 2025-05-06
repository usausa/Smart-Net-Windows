namespace Smart.Windows.Interactivity.Messaging;

using Smart.Mvvm.Messaging;

public sealed class ResolveRequestTrigger : RequestTriggerBase<ResolveEventArgs>
{
    protected override void OnEventRequest(object? sender, ResolveEventArgs e)
    {
        InvokeActions(e);
    }
}
