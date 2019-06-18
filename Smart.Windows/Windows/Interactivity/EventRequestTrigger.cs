namespace Smart.Windows.Interactivity
{
    using Smart.Windows.Messaging;

    public sealed class EventRequestTrigger : RequestTriggerBase<ParameterEventArgs>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        protected override void OnEventRequest(object sender, ParameterEventArgs e)
        {
            InvokeActions(e.Parameter);
        }
    }
}
