namespace Smart.Windows.Interactivity
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class EventRequestTrigger : EventRequestTriggerBase<EventArgs>
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
