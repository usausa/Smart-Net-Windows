namespace Smart.Windows.Input;

using System.Windows.Input;

#pragma warning disable CA1815
public readonly struct WaitCursor : IDisposable
{
    private readonly Cursor oldCursor;

    public WaitCursor(Cursor? cursor = null)
    {
        oldCursor = Mouse.OverrideCursor;
        Mouse.OverrideCursor = cursor ?? Cursors.Wait;
    }

    public void Dispose()
    {
        Mouse.OverrideCursor = oldCursor;
    }
}
#pragma warning restore CA1815
