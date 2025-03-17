using HashSetLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashSetTests
{
    /// <summary>
    /// Тестовый класс для проверки функциональности CustomHashSet<T>.
    /// </summary>
    [TestClass]
    public class CustomHashSetTests
    {
        /// <summary>
        /// Проверяет, что новая коллекция пуста.
        /// </summary>
        [TestMethod]
        public void NewHashSet_ShouldBeEmpty()
        {
            // Arrange
            var set = new CustomHashSet<int>();

            // Act & Assert
            Assert.AreEqual(0, set.Count, "Новая коллекция должна быть пустой.");
        }

        /// <summary>
        /// Проверяет добавление одного элемента и увеличение размера.
        /// </summary>
        [TestMethod]
        public void Add_SingleItem_ShouldIncreaseCount()
        {
            // Arrange
            var set = new CustomHashSet<int>();

            // Act
            set.Add(1);

            // Assert
            Assert.AreEqual(1, set.Count, "Размер должен увеличиться до 1 после добавления.");
            Assert.IsTrue(set.Contains(1), "Элемент 1 должен присутствовать в коллекции.");
        }

        /// <summary>
        /// Проверяет, что дубликаты не добавляются.
        /// </summary>
        [TestMethod]
        public void Add_DuplicateItem_ShouldNotIncreaseCount()
        {
            // Arrange
            var set = new CustomHashSet<int>();
            set.Add(1);

            // Act
            set.Add(1);

            // Assert
            Assert.AreEqual(1, set.Count, "Размер не должен увеличиваться при добавлении дубликата.");
            Assert.IsTrue(set.Contains(1), "Элемент 1 должен остаться в коллекции.");
        }

        /// <summary>
        /// Проверяет добавление нескольких уникальных элементов.
        /// </summary>
        [TestMethod]
        public void Add_MultipleUniqueItems_ShouldStoreAll()
        {
            // Arrange
            var set = new CustomHashSet<int>();

            // Act
            set.Add(1);
            set.Add(2);
            set.Add(3);

            // Assert
            Assert.AreEqual(3, set.Count, "Размер должен быть 3 после добавления 3 уникальных элементов.");
            Assert.IsTrue(set.Contains(1), "Элемент 1 должен присутствовать.");
            Assert.IsTrue(set.Contains(2), "Элемент 2 должен присутствовать.");
            Assert.IsTrue(set.Contains(3), "Элемент 3 должен присутствовать.");
        }

        /// <summary>
        /// Проверяет добавление дубликатов из примера задания (1,2,3,4,2,3,2,1).
        /// </summary>
        [TestMethod]
        public void Add_DuplicatesFromTask_ShouldStoreUniqueOnly()
        {
            // Arrange
            var set = new CustomHashSet<int>();

            // Act
            set.Add(1);
            set.Add(2);
            set.Add(3);
            set.Add(4);
            set.Add(2);
            set.Add(3);
            set.Add(2);
            set.Add(1);

            // Assert
            Assert.AreEqual(4, set.Count, "Размер должен быть 4 после добавления с дубликатами.");
            Assert.IsTrue(set.Contains(1), "Элемент 1 должен присутствовать.");
            Assert.IsTrue(set.Contains(2), "Элемент 2 должен присутствовать.");
            Assert.IsTrue(set.Contains(3), "Элемент 3 должен присутствовать.");
            Assert.IsTrue(set.Contains(4), "Элемент 4 должен присутствовать.");
        }

        /// <summary>
        /// Проверяет удаление существующего элемента.
        /// </summary>
        [TestMethod]
        public void Remove_ExistingItem_ShouldDecreaseCount()
        {
            // Arrange
            var set = new CustomHashSet<int>();
            set.Add(1);
            set.Add(2);

            // Act
            bool removed = set.Remove(1);

            // Assert
            Assert.IsTrue(removed, "Удаление должно вернуть true для существующего элемента.");
            Assert.AreEqual(1, set.Count, "Размер должен уменьшиться до 1.");
            Assert.IsFalse(set.Contains(1), "Элемент 1 не должен присутствовать после удаления.");
            Assert.IsTrue(set.Contains(2), "Элемент 2 должен остаться.");
        }

        /// <summary>
        /// Проверяет удаление несуществующего элемента.
        /// </summary>
        [TestMethod]
        public void Remove_NonExistingItem_ShouldReturnFalse()
        {
            // Arrange
            var set = new CustomHashSet<int>();
            set.Add(1);

            // Act
            bool removed = set.Remove(2);

            // Assert
            Assert.IsFalse(removed, "Удаление должно вернуть false для несуществующего элемента.");
            Assert.AreEqual(1, set.Count, "Размер не должен измениться.");
            Assert.IsTrue(set.Contains(1), "Элемент 1 должен остаться.");
        }

        /// <summary>
        /// Проверяет очистку коллекции.
        /// </summary>
        [TestMethod]
        public void Clear_ShouldRemoveAllItems()
        {
            // Arrange
            var set = new CustomHashSet<int>();
            set.Add(1);
            set.Add(2);
            set.Add(3);

            // Act
            set.Clear();

            // Assert
            Assert.AreEqual(0, set.Count, "Размер должен стать 0 после очистки.");
            Assert.IsFalse(set.Contains(1), "Элемент 1 не должен присутствовать.");
            Assert.IsFalse(set.Contains(2), "Элемент 2 не должен присутствовать.");
            Assert.IsFalse(set.Contains(3), "Элемент 3 не должен присутствовать.");
        }

        /// <summary>
        /// Проверяет работу итератора на пустой коллекции.
        /// </summary>
        [TestMethod]
        public void Enumerator_EmptySet_ShouldNotMoveNext()
        {
            // Arrange
            var set = new CustomHashSet<int>();
            var enumerator = set.GetEnumerator();

            // Act & Assert
            Assert.IsFalse(enumerator.MoveNext(), "MoveNext должен вернуть false для пустой коллекции.");
        }

        /// <summary>
        /// Проверяет работу итератора с несколькими элементами.
        /// </summary>
        [TestMethod]
        public void Enumerator_MultipleItems_ShouldIterateAll()
        {
            // Arrange
            var set = new CustomHashSet<int>();
            set.Add(1);
            set.Add(2);
            set.Add(3);
            var enumerator = set.GetEnumerator();
            int count = 0;
            bool has1 = false, has2 = false, has3 = false;

            // Act
            while (enumerator.MoveNext())
            {
                count++;
                if (enumerator.Current == 1) has1 = true;
                if (enumerator.Current == 2) has2 = true;
                if (enumerator.Current == 3) has3 = true;
            }

            // Assert
            Assert.AreEqual(3, count, "Итератор должен перебрать все 3 элемента.");
            Assert.IsTrue(has1, "Элемент 1 должен быть перебраным.");
            Assert.IsTrue(has2, "Элемент 2 должен быть перебраным.");
            Assert.IsTrue(has3, "Элемент 3 должен быть перебраным.");
        }

        /// <summary>
        /// Проверяет сброс и повторный перебор итератора.
        /// </summary>
        [TestMethod]
        public void Enumerator_Reset_ShouldAllowReiteration()
        {
            // Arrange
            var set = new CustomHashSet<int>();
            set.Add(1);
            set.Add(2);
            var enumerator = set.GetEnumerator();
            int firstCount = 0;

            while (enumerator.MoveNext()) firstCount++;

            // Act
            enumerator.Reset();
            int secondCount = 0;
            while (enumerator.MoveNext()) secondCount++;

            // Assert
            Assert.AreEqual(2, firstCount, "Первый перебор должен найти 2 элемента.");
            Assert.AreEqual(2, secondCount, "Второй перебор после Reset должен найти 2 элемента.");
        }
    }
}