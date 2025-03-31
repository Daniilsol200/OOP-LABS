// CustomSetTests/CustomSetTests.cs
using CustomSetLibrary;
using Xunit;

namespace CustomSetTests
{
    public class CustomSetTests
    {
        [Fact]
        public void Add_AddsUniqueElement_IncreasesCount()
        {
            // Arrange
            var set = new CustomSet<string>();

            // Act
            set.Add("a");

            // Assert
            Assert.Equal(1, set.Count);
            Assert.True(set.Contains("a"));
        }

        [Fact]
        public void Add_DuplicateElement_DoesNotIncreaseCount()
        {
            // Arrange
            var set = new CustomSet<string>();
            set.Add("a");

            // Act
            set.Add("a");

            // Assert
            Assert.Equal(1, set.Count);
            Assert.True(set.Contains("a"));
        }

        [Fact]
        public void Add_MultipleUniqueElements_CorrectCountAndContains()
        {
            // Arrange
            var set = new CustomSet<string>();

            // Act
            set.Add("11");
            set.Add("abc");
            set.Add("xyz");

            // Assert
            Assert.Equal(3, set.Count);
            Assert.True(set.Contains("11"));
            Assert.True(set.Contains("abc"));
            Assert.True(set.Contains("xyz"));
            Assert.False(set.Contains("def"));
        }

        [Fact]
        public void Contains_EmptySet_ReturnsFalse()
        {
            // Arrange
            var set = new CustomSet<string>();

            // Act & Assert
            Assert.False(set.Contains("a"));
        }

        [Fact]
        public void Remove_ExistingElement_DecreasesCountAndRemoves()
        {
            // Arrange
            var set = new CustomSet<string>();
            set.Add("11");
            set.Add("abc");

            // Act
            bool removed = set.Remove("11");

            // Assert
            Assert.True(removed);
            Assert.Equal(1, set.Count);
            Assert.False(set.Contains("11"));
            Assert.True(set.Contains("abc"));
        }

        [Fact]
        public void Remove_NonExistingElement_ReturnsFalseAndNoChange()
        {
            // Arrange
            var set = new CustomSet<string>();
            set.Add("abc");

            // Act
            bool removed = set.Remove("11");

            // Assert
            Assert.False(removed);
            Assert.Equal(1, set.Count);
            Assert.True(set.Contains("abc"));
        }

        [Fact]
        public void Clear_RemovesAllElements()
        {
            // Arrange
            var set = new CustomSet<string>();
            set.Add("11");
            set.Add("abc");

            // Act
            set.Clear();

            // Assert
            Assert.Equal(0, set.Count);
            Assert.False(set.Contains("11"));
            Assert.False(set.Contains("abc"));
        }

        [Fact]
        public void Count_EmptySet_ReturnsZero()
        {
            // Arrange
            var set = new CustomSet<string>();

            // Assert
            Assert.Equal(0, set.Count);
        }

        [Fact]
        public void IsReadOnly_ReturnsFalse()
        {
            // Arrange
            var set = new CustomSet<string>();

            // Assert
            Assert.False(set.IsReadOnly);
        }

        [Fact]
        public void CopyTo_CopiesElementsToArray()
        {
            // Arrange
            var set = new CustomSet<string>();
            set.Add("11");
            set.Add("abc");
            string[] array = new string[4];

            // Act
            set.CopyTo(array, 1);

            // Assert
            Assert.Equal("11", array[1]);
            Assert.Equal("abc", array[2]);
            Assert.Null(array[0]); // Начало массива не тронуто
            Assert.Null(array[3]); // Конец массива не тронуто
        }

        [Fact]
        public void GetEnumerator_IteratesOverElements()
        {
            // Arrange
            var set = new CustomSet<string>();
            set.Add("11");
            set.Add("abc");

            // Act
            var enumerator = set.GetEnumerator();
            var elements = new List<string>();
            while (enumerator.MoveNext())
            {
                elements.Add(enumerator.Current);
            }

            // Assert
            Assert.Equal(2, elements.Count);
            Assert.Contains("11", elements);
            Assert.Contains("abc", elements);
        }

        [Fact]
        public void Add_ForcesResize_CorrectlyHandlesExpansion()
        {
            // Arrange
            var set = new CustomSet<string>();

            // Act
            set.Add("1");  // 1
            set.Add("2");  // 2
            set.Add("3");  // 3
            set.Add("4");  // 4 (начальный размер)
            set.Add("5");  // вызывает Resize

            // Assert
            Assert.Equal(5, set.Count);
            Assert.True(set.Contains("1"));
            Assert.True(set.Contains("5"));
        }
    }
}