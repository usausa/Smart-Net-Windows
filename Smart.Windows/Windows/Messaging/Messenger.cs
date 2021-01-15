namespace Smart.Windows.Messaging
{
    using System;

    public sealed class Messenger : IMessenger
    {
        private static readonly Type ObjectType = typeof(object);

        public event EventHandler<MessengerEventArgs> Received;

        public void Send(string label)
        {
            Received?.Invoke(this, new MessengerEventArgs(label, ObjectType, null));
        }

        public void Send<T>(T message)
        {
            Received?.Invoke(this, new MessengerEventArgs(string.Empty, typeof(T), message));
        }

        public void Send<T>(string label, T parameter)
        {
            Received?.Invoke(this, new MessengerEventArgs(label, typeof(T), parameter));
        }
    }
}
