using System;

namespace HashSetLib
{
    /// <summary>
    /// Пользовательская реализация хэш-таблицы, обеспечивающая хранение уникальных элементов.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранимых в коллекции.</typeparam>
    public class CustomHashSet<T> : ICollection<T>
    {
        private HashNode<T>[] buckets;
        private int size;
        private const int DEFAULT_CAPACITY = 16;
        private const double LOAD_FACTOR = 0.75;

        /// <summary>
        /// Инициализирует новую пустую хэш-таблицу с начальной ёмкостью.
        /// </summary>
        public CustomHashSet()
        {
            buckets = new HashNode<T>[DEFAULT_CAPACITY];
            size = 0;
        }

        /// <summary>
        /// Получает количество элементов в коллекции.
        /// </summary>
        public int Count => size;

        /// <summary>
        /// Вычисляет индекс bucket'а для заданного элемента на основе его хэш-кода.
        /// </summary>
        /// <param name="item">Элемент, для которого вычисляется индекс.</param>
        /// <returns>Индекс в массиве buckets.</returns>
        private int GetBucketIndex(T item)
        {
            if (item == null) return 0;
            int hash = item.GetHashCode();
            return Math.Abs(hash % buckets.Length);
        }

        /// <summary>
        /// Добавляет элемент в коллекцию, если его ещё нет.
        /// </summary>
        /// <param name="item">Элемент для добавления.</param>
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

        /// <summary>
        /// Проверяет, содержится ли элемент в коллекции.
        /// </summary>
        /// <param name="item">Элемент для проверки.</param>
        /// <returns>true, если элемент найден; false в противном случае.</returns>
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

        /// <summary>
        /// Удаляет элемент из коллекции.
        /// </summary>
        /// <param name="item">Элемент для удаления.</param>
        /// <returns>true, если элемент был удалён; false, если не найден.</returns>
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

        /// <summary>
        /// Очищает коллекцию, возвращая её в начальное состояние.
        /// </summary>
        public void Clear()
        {
            buckets = new HashNode<T>[DEFAULT_CAPACITY];
            size = 0;
        }

        /// <summary>
        /// Увеличивает размер хэш-таблицы и перераспределяет элементы.
        /// </summary>
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

        /// <summary>
        /// Возвращает итератор для перебора элементов коллекции.
        /// </summary>
        /// <returns>Объект итератора.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new HashSetEnumerator(this);
        }

        /// <summary>
        /// Внутренний класс итератора для обхода элементов хэш-таблицы.
        /// </summary>
        private class HashSetEnumerator : IEnumerator<T>
        {
            private CustomHashSet<T> set;
            private int bucketIndex;
            private HashNode<T> currentNode;

            /// <summary>
            /// Инициализирует итератор для заданной хэш-таблицы.
            /// </summary>
            /// <param name="set">Коллекция, элементы которой будут перебираться.</param>
            public HashSetEnumerator(CustomHashSet<T> set)
            {
                this.set = set;
                bucketIndex = -1;
                currentNode = null;
            }

            /// <summary>
            /// Получает текущий элемент, на котором находится итератор.
            /// </summary>
            public T Current => currentNode.Value;

            /// <summary>
            /// Перемещает итератор к следующему элементу.
            /// </summary>
            /// <returns>true, если следующий элемент найден; false, если перебор завершён.</returns>
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

            /// <summary>
            /// Сбрасывает итератор в начальное состояние.
            /// </summary>
            public void Reset()
            {
                bucketIndex = -1;
                currentNode = null;
            }
        }
    }
}