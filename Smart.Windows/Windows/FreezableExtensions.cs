namespace Smart.Windows
{
    using System;
    using System.Windows;

    public static class FreezableExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public static T ToFrozen<T>(this T freezable)
            where T : Freezable
        {
            if (!freezable.IsFrozen)
            {
                if (freezable.CanFreeze)
                {
                    freezable.Freeze();
                }
                else
                {
                    throw new InvalidOperationException("Object can not freeze.");
                }
            }

            return freezable;
        }
    }
}
