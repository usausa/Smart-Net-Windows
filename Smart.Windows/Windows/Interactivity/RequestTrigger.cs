namespace Smart.Windows.Interactivity
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public sealed class RequestTrigger : RequestTriggerBase<EventArgs>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEventRequest(object sender, EventArgs e)
        {
            InvokeActions(null);
        }
    }
}
