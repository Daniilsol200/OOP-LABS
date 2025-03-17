namespace HashSetLib
{
    public class HashNode<T>
    {
        public T Value { get; set; }
        public HashNode<T> Next { get; set; }
        public bool IsDeleted { get; set; }

        public HashNode(T value)
        {
            Value = value;
            Next = null;
            IsDeleted = false;
        }
    }
}