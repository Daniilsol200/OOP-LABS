using System.Collections.Generic;

namespace HashSetLib
{
    public interface ICollection<T>
    {
        void Add(T item);
        bool Remove(T item);
        bool Contains(T item);
        int Count { get; }
        void Clear();
        IEnumerator<T> GetEnumerator();
    }
}