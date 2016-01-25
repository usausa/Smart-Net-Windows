namespace Smart.Windows
{
    using System;
    using System.Windows;

    /// <summary>
    ///
    /// </summary>
    public static class FreezableExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="freezable"></param>
        /// <returns></returns>
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
                    throw new InvalidOperationException("Object can not freez.");
                }
            }

            return freezable;
        }
    }
}
