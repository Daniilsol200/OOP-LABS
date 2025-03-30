// CustomSetLibrary/CustomSet.cs
using System.Collections.Generic;

namespace CustomSetLibrary
{
    public class CustomSet<T> : ICollection<T>
    {
        private T[] items;
        private int count;
        private const int INITIAL_CAPACITY = 4;

        public CustomSet()
        {
            items = new T[INITIAL_CAPACITY];
            count = 0;
        }

        public void Add(T item)
        {
            if (Contains(item)) return;

            if (count == items.Length)
                Resize();

            items[count] = item;
            count++;
        }

        private void Resize()
        {
            T[] newItems = new T[items.Length * 2];
            for (int i = 0; i < count; i++)
            {
                newItems[i] = items[i];
            }
            items = newItems;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (Equals(items[i], item))
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            items = new T[INITIAL_CAPACITY];
            count = 0;
        }

        public int Count => count;

        public bool IsReadOnly => false;

        public bool Remove(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (Equals(items[i], item))
                {
                    for (int j = i; j < count - 1; j++)
                    {
                        items[j] = items[j + 1];
                    }
                    count--;
                    items[count] = default(T);
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < count; i++)
            {
                array[arrayIndex + i] = items[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SetIterator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class SetIterator : IEnumerator<T>
        {
            private CustomSet<T> set;
            private int currentIndex;

            public SetIterator(CustomSet<T> set)
            {
                this.set = set;
                currentIndex = -1;
            }

            public T Current => set.items[currentIndex];

            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < set.count;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose() { }
        }
    }
}