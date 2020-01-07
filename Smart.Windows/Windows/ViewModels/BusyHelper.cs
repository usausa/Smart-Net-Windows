namespace Smart.Windows.ViewModels
{
    using System;
    using System.Threading.Tasks;

    public static class BusyHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2007:DoNotDirectlyAwaitATask", Justification = "Ignore")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2007:DoNotDirectlyAwaitATask", Justification = "Ignore")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2007:DoNotDirectlyAwaitATask", Justification = "Ignore")]
        public static async ValueTask ExecuteBusyAsync(IBusyState state, Func<ValueTask> execute)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2007:DoNotDirectlyAwaitATask", Justification = "Ignore")]
        public static async ValueTask<TResult> ExecuteBusyAsync<TResult>(IBusyState state, Func<ValueTask<TResult>> execute)
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
