namespace Smart.Windows.Messaging
{
    using System;

    public sealed class EventEventArgs : EventArgs
    {
        public object Value { get; }

        public EventEventArgs(object value)
        {
            Value = value;
        }
    }
}
