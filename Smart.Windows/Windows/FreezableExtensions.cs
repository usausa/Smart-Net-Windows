namespace Smart.Windows;

using System.Windows;

public static class FreezableExtensions
{
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
