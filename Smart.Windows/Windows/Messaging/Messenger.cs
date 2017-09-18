namespace Smart.Windows.Messaging
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class Messenger : IMessenger
    {
        /// <summary>
        ///
        /// </summary>
        public event EventHandler<MessengerEventArgs> Recieved;

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            Recieved?.Invoke(this, new MessengerEventArgs(message, null));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        public void Send(object parameter)
        {
            Recieved?.Invoke(this, new MessengerEventArgs(string.Empty, parameter));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameter"></param>
        public void Send(string message, object parameter)
        {
            Recieved?.Invoke(this, new MessengerEventArgs(message, parameter));
        }
    }
}
