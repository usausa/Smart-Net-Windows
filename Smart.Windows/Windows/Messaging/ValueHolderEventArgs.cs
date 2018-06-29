namespace Smart.Windows.Messaging
{
    using System;

    public sealed class ValueHolderEventArgs : EventArgs
    {
        public object Value { get; set; }
    }
}
