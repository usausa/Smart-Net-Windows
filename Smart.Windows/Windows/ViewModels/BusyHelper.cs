namespace Smart.Windows.ViewModels
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    ///
    /// </summary>
    public static class BusyHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        public static async Task ExecuteBusyAsync(IBusyState state, Func<Task> execute)
        {
            try
            {
                state.IsBusy = true;

                await execute();
            }
            finally
            {
                state.IsBusy = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="state"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteBusyAsync<TResult>(IBusyState state, Func<Task<TResult>> execute)
        {
            try
            {
                state.IsBusy = true;

                return await execute();
            }
            finally
            {
                state.IsBusy = false;
            }
        }
    }
}
