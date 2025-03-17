namespace HashSetLib
{
    /// <summary>
    /// Узел хэш-таблицы, используемый для хранения значения и ссылки на следующий элемент в цепочке.
    /// </summary>
    /// <typeparam name="T">Тип хранимого значения.</typeparam>
    public class HashNode<T>
    {
        /// <summary>
        /// Значение, хранимое в узле.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Ссылка на следующий узел в цепочке (для разрешения коллизий).
        /// </summary>
        public HashNode<T> Next { get; set; }

        /// <summary>
        /// Указывает, помечен ли узел как удалённый.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Инициализирует новый узел с заданным значением.
        /// </summary>
        /// <param name="value">Значение для хранения в узле.</param>
        public HashNode(T value)
        {
            Value = value;
            Next = null;
            IsDeleted = false;
        }
    }
}