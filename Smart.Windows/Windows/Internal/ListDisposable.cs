namespace Smart.Windows.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    internal sealed class ListDisposable : ICollection<IDisposable>, IDisposable
    {
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        /// <summary>
        ///
        /// </summary>
        public int Count => disposables.Count;

        /// <summary>
        ///
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }

            disposables.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDisposable> GetEnumerator()
        {
            return disposables.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        public void Add(IDisposable item)
        {
            disposables.Add(item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IDisposable item)
        {
            return disposables.Remove(item);
        }

        /// <summary>
        ///
        /// </summary>
        public void Clear()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }

            disposables.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(IDisposable item)
        {
            return disposables.Contains(item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            Array.Copy(disposables.ToArray(), 0, array, arrayIndex, array.Length - arrayIndex);
        }
    }
}
