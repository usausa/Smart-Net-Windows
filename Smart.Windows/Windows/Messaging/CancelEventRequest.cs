namespace Smart.Windows.Messaging
{
    using System;
    using System.ComponentModel;

    public class CancelEventRequest : IEventRequest<CancelEventArgs>
    {
        public event EventHandler<CancelEventArgs> Requested;

        public bool IsCancel()
        {
            var args = new CancelEventArgs();
            Requested?.Invoke(this, args);
            return args.Cancel;
        }
    }
}
