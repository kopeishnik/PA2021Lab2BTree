using System;

namespace B_Tree
{
    public class KeyValue<T>
    {
        public int Key { get; private set; }
        public T Value;
        public KeyValue(int key, T value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
