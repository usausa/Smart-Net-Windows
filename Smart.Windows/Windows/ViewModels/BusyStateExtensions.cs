namespace Smart.Windows.ViewModels
{
    using System;
    using System.Threading.Tasks;

    public static class BusyStateExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static void Using(this IBusyState state, Action execute)
        {
            using (state.Begin())
            {
                execute();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static TResult Using<TResult>(this IBusyState state, Func<TResult> execute)
        {
            using (state.Begin())
            {
                return execute();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static async Task UsingAsync(this IBusyState state, Func<Task> execute)
        {
            using (state.Begin())
            {
                await execute().ConfigureAwait(false);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static async Task<TResult> UsingAsync<TResult>(this IBusyState state, Func<Task<TResult>> execute)
        {
            using (state.Begin())
            {
                return await execute().ConfigureAwait(false);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static IDisposable Begin(this IBusyState state) => new BusyStateScope(state);

        private readonly struct BusyStateScope : IDisposable
        {
            private readonly IBusyState state;

            public BusyStateScope(IBusyState state)
            {
                state.Require();
                this.state = state;
            }

            public void Dispose()
            {
                state.Release();
            }
        }
    }
}
