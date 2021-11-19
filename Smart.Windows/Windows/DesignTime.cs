namespace Smart.Windows;

using System.ComponentModel;
using System.Windows;

public static class DesignTime
{
    private static bool? isInDesignMode;

    public static bool IsInDesignMode
    {
        get
        {
            isInDesignMode ??= (bool)DesignerProperties
                .IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject)).DefaultValue;

            return isInDesignMode.Value;
        }
    }
}
