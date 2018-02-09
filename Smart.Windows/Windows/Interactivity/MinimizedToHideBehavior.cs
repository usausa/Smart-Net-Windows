namespace Smart.Windows.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;
    using System.Windows.Interop;

    /// <summary>
    ///
    /// </summary>
    [TypeConstraint(typeof(Window))]
    public sealed class MinimizedToHideBehavior : Behavior<Window>
    {
        private readonly HwndSourceHook hook;

        /// <summary>
        ///
        /// </summary>
        public MinimizedToHideBehavior()
        {
            hook = WndProc;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.SourceInitialized += SourceInitialized;
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.SourceInitialized -= SourceInitialized;
            UnregisterHook();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SourceInitialized(object sender, EventArgs eventArgs)
        {
            RegisterHook();
        }

        /// <summary>
        ///
        /// </summary>
        private void RegisterHook()
        {
            (PresentationSource.FromVisual(AssociatedObject) as HwndSource)?.AddHook(hook);
        }

        /// <summary>
        ///
        /// </summary>
        private void UnregisterHook()
        {
            (PresentationSource.FromVisual(AssociatedObject) as HwndSource)?.RemoveHook(hook);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Compatibility.")]
        private IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // WM_SYSCOMMAND, SC_MINIMIZE
            if ((msg == 0x0112) && (wParam.ToInt32() == 0xf020))
            {
                AssociatedObject.Hide();
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
