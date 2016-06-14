namespace Smart.Windows.Messaging
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class Messenger
    {
        /// <summary>
        ///
        /// </summary>
        public event EventHandler<MessengerEventArgs> Recieved;

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameter"></param>
        public void Send(string message = null, object parameter = null)
        {
            Recieved?.Invoke(this, new MessengerEventArgs(message, parameter));
        }
    }
}
