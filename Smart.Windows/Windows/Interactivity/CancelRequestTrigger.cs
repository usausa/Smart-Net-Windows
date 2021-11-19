namespace Smart.Windows.Interactivity;

using System.ComponentModel;

public sealed class CancelRequestTrigger : RequestTriggerBase<CancelEventArgs>
{
    protected override void OnEventRequest(object? sender, CancelEventArgs e)
    {
        InvokeActions(e);
    }
}
