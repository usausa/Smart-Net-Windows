namespace Smart.Windows.ViewModels
{
    using System;
    using System.Threading.Tasks;

    public static class BusyHelper
    {
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
