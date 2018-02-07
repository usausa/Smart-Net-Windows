namespace Smart.Windows.Input
{
    using System;
    using System.Windows.Input;

    public sealed class WaitCursor : IDisposable
    {
        private readonly Cursor oldCursor;

        public WaitCursor()
            : this(Cursors.Wait)
        {
        }

        public WaitCursor(Cursor cursor)
        {
            oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = cursor;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = oldCursor;
        }
    }
}
