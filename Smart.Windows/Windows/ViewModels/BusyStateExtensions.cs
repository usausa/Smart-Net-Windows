namespace Smart.Windows.ViewModels;

public static class BusyStateExtensions
{
    public static void Using(this IBusyState state, Action execute)
    {
        using (state.Begin())
        {
            execute();
        }
    }

    public static TResult Using<TResult>(this IBusyState state, Func<TResult> execute)
    {
        using (state.Begin())
        {
            return execute();
        }
    }

    public static async Task UsingAsync(this IBusyState state, Func<Task> execute)
    {
        using (state.Begin())
        {
            await execute().ConfigureAwait(true);
        }
    }

    public static async Task<TResult> UsingAsync<TResult>(this IBusyState state, Func<Task<TResult>> execute)
    {
        using (state.Begin())
        {
            return await execute().ConfigureAwait(true);
        }
    }

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
