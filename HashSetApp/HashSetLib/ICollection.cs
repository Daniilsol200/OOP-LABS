namespace HashSetLib
{
    /// <summary>
    /// Интерфейс для коллекции, поддерживающей базовые операции добавления, удаления и перебора элементов.
    /// </summary>
    /// <typeparam name="T">Тип элементов, хранимых в коллекции.</typeparam>
    public interface ICollection<T>
    {
        /// <summary>
        /// Добавляет элемент в коллекцию.
        /// </summary>
        /// <param name="item">Элемент для добавления.</param>
        void Add(T item);

        /// <summary>
        /// Удаляет элемент из коллекции.
        /// </summary>
        /// <param name="item">Элемент для удаления.</param>
        /// <returns>true, если элемент был удалён; false, если элемент не найден.</returns>
        bool Remove(T item);

        /// <summary>
        /// Проверяет, содержится ли элемент в коллекции.
        /// </summary>
        /// <param name="item">Элемент для проверки.</param>
        /// <returns>true, если элемент присутствует; false в противном случае.</returns>
        bool Contains(T item);

        /// <summary>
        /// Получает количество элементов в коллекции.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Очищает коллекцию, удаляя все элементы.
        /// </summary>
        void Clear();

        /// <summary>
        /// Возвращает итератор для перебора элементов коллекции.
        /// </summary>
        /// <returns>Объект итератора.</returns>
        IEnumerator<T> GetEnumerator();
    }
}