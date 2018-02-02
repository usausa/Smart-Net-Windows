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
        //public static bool IsInDesignMode => isInDesignMode ?? (isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject())).Value;
        public static bool IsInDesignMode
        {
            get
            {
                if (!isInDesignMode.HasValue)
                {
                    isInDesignMode = (bool)DesignerProperties
                        .IsInDesignModeProperty
                        .GetMetadata(typeof(DependencyObject)).DefaultValue;
                }

                return isInDesignMode.Value;
            }
        }
    }
}
