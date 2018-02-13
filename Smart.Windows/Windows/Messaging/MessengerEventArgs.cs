namespace Smart.Windows.Messaging
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public sealed class MessengerEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        public string Label { get; }

        /// <summary>
        ///
        /// </summary>
        public Type MessageType { get; }

        /// <summary>
        ///
        /// </summary>
        public object Message { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <param name="messageType"></param>
        /// <param name="message"></param>
        public MessengerEventArgs(string label, Type messageType, object message)
        {
            Label = label;
            MessageType = messageType;
            Message = message;
        }
    }
}
