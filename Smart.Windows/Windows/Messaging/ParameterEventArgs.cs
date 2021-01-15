namespace Smart.Windows.Messaging
{
    using System;

    public sealed class ParameterEventArgs : EventArgs
    {
        public object Parameter { get; }

        public ParameterEventArgs(object parameter)
        {
            Parameter = parameter;
        }
    }
}
