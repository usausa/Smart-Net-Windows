namespace Smart.Windows.Messaging
{
    /// <summary>
    ///
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameter"></param>
        void Send(object message, object parameter);
    }
}
