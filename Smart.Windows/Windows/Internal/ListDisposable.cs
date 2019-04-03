namespace Smart.Windows.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class ListDisposable : ICollection<IDisposable>, IDisposable
    {
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        public int Count => disposables.Count;

        public bool IsReadOnly => false;

        public void Dispose()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }

            disposables.Clear();
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            return disposables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IDisposable item)
        {
            disposables.Add(item);
        }

        public bool Remove(IDisposable item)
        {
            return disposables.Remove(item);
        }

        public void Clear()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }

            disposables.Clear();
        }

        public bool Contains(IDisposable item)
        {
            return disposables.Contains(item);
        }

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            Array.Copy(disposables.ToArray(), 0, array, arrayIndex, array.Length - arrayIndex);
        }
    }
}
