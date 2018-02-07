namespace Smart.Windows.Messaging
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler<MessengerEventArgs> Recieved;

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        void Send(object parameter);

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameter"></param>
        void Send(string message, object parameter);
    }
}
