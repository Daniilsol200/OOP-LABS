namespace HashSetLib
{
    /// <summary>
    /// Интерфейс для итератора, обеспечивающего последовательный доступ к элементам коллекции.
    /// </summary>
    /// <typeparam name="T">Тип элементов, перебираемых итератором.</typeparam>
    public interface IEnumerator<T>
    {
        /// <summary>
        /// Получает текущий элемент, на котором находится итератор.
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Перемещает итератор к следующему элементу в коллекции.
        /// </summary>
        /// <returns>true, если следующий элемент существует; false, если перебор завершён.</returns>
        bool MoveNext();

        /// <summary>
        /// Сбрасывает итератор в начальное состояние.
        /// </summary>
        void Reset();
    }
}