using System;

namespace HashSetLib
{
    public class CustomHashSet<T> : ICollection<T>
    {
        private HashNode<T>[] buckets;
        private int size;
        private const int DEFAULT_CAPACITY = 16;
        private const double LOAD_FACTOR = 0.75;

        public CustomHashSet()
        {
            buckets = new HashNode<T>[DEFAULT_CAPACITY];
            size = 0;
        }

        public int Count => size;

        private int GetBucketIndex(T item)
        {
            if (item == null) return 0;
            int hash = item.GetHashCode();
            return Math.Abs(hash % buckets.Length);
        }

        public void Add(T item)
        {
            if (Contains(item)) return;

            if ((double)(size + 1) / buckets.Length >= LOAD_FACTOR)
            {
                Resize();
            }

            int index = GetBucketIndex(item);
            HashNode<T> newNode = new HashNode<T>(item);

            if (buckets[index] == null)
            {
                buckets[index] = newNode;
            }
            else
            {
                HashNode<T> current = buckets[index];
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            size++;
        }

        public bool Contains(T item)
        {
            int index = GetBucketIndex(item);
            HashNode<T> current = buckets[index];

            while (current != null)
            {
                if (!current.IsDeleted &&
                    (item == null ? current.Value == null : item.Equals(current.Value)))
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public bool Remove(T item)
        {
            int index = GetBucketIndex(item);
            HashNode<T> current = buckets[index];
            HashNode<T> prev = null;

            while (current != null)
            {
                if (!current.IsDeleted &&
                    (item == null ? current.Value == null : item.Equals(current.Value)))
                {
                    if (prev == null)
                    {
                        buckets[index] = current.Next;
                    }
                    else
                    {
                        prev.Next = current.Next;
                    }
                    size--;
                    return true;
                }
                prev = current;
                current = current.Next;
            }
            return false;
        }

        public void Clear()
        {
            buckets = new HashNode<T>[DEFAULT_CAPACITY];
            size = 0;
        }

        private void Resize()
        {
            HashNode<T>[] oldBuckets = buckets;
            buckets = new HashNode<T>[oldBuckets.Length * 2];
            size = 0;

            foreach (var bucket in oldBuckets)
            {
                HashNode<T> current = bucket;
                while (current != null)
                {
                    if (!current.IsDeleted)
                    {
                        Add(current.Value);
                    }
                    current = current.Next;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new HashSetEnumerator(this);
        }

        private class HashSetEnumerator : IEnumerator<T>
        {
            private CustomHashSet<T> set;
            private int bucketIndex;
            private HashNode<T> currentNode;

            public HashSetEnumerator(CustomHashSet<T> set)
            {
                this.set = set;
                bucketIndex = -1;
                currentNode = null;
            }

            public T Current => currentNode.Value;

            public bool MoveNext()
            {
                if (currentNode != null && currentNode.Next != null)
                {
                    currentNode = currentNode.Next;
                    if (!currentNode.IsDeleted)
                        return true;
                }

                while (bucketIndex < set.buckets.Length - 1)
                {
                    bucketIndex++;
                    currentNode = set.buckets[bucketIndex];

                    while (currentNode != null)
                    {
                        if (!currentNode.IsDeleted)
                            return true;
                        currentNode = currentNode.Next;
                    }
                }
                return false;
            }

            public void Reset()
            {
                bucketIndex = -1;
                currentNode = null;
            }
        }
    }
}