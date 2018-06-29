﻿namespace Smart.Windows.Interactivity
{
    using Smart.Windows.Messaging;

    /// <summary>
    ///
    /// </summary>
    public sealed class ResolveEventTriggerBase : EventRequestTriggerBase<ValueHolderEventArgs>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEventRequest(object sender, ValueHolderEventArgs e)
        {
            InvokeActions(e);
        }
    }
}
