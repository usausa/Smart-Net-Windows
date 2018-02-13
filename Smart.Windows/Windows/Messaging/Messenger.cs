namespace Smart.Windows.Messaging
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public sealed class Messenger : IMessenger
    {
        private static readonly Type ObjectType = typeof(object);

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<MessengerEventArgs> Recieved;

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        public void Send(string label)
        {
            Recieved?.Invoke(this, new MessengerEventArgs(label, ObjectType, null));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public void Send<T>(T message)
        {
            Recieved?.Invoke(this, new MessengerEventArgs(string.Empty, typeof(T), message));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <param name="parameter"></param>
        public void Send<T>(string label, T parameter)
        {
            Recieved?.Invoke(this, new MessengerEventArgs(label, typeof(T), parameter));
        }
    }
}
