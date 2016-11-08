namespace Smart.Windows.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///
    /// </summary>
    public class DataContextDisposeAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            (AssociatedObject.DataContext as IDisposable)?.Dispose();
        }
    }
}
