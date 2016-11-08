namespace Smart.Windows
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    ///
    /// </summary>
    public static class DesignTime
    {
        private static bool? isInDesignMode;

        /// <summary>
        ///
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                return isInDesignMode ?? (isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject())).Value;
            }
        }
    }
}
