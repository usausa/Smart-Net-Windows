namespace Smart.Windows.Input
{
    using System;
    using System.Windows.Input;

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
