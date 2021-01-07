namespace Smart.Windows.Input
{
    using System;
    using System.Windows.Input;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "Ignore")]
    public readonly struct WaitCursor : IDisposable
    {
        private readonly Cursor oldCursor;

        public WaitCursor(Cursor cursor = null)
        {
            oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = cursor ?? Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = oldCursor;
        }
    }
}
