using System;
using System.Collections.Generic;
using System.Linq;

namespace B_Tree
{
    public class Node<T>
    {
        public List<KeyValue<T>> Keys;
        private List<Node<T>> _children;
        public Node<T> Parent;

        public List<Node<T>> Children
        {
            get => _children;
            set
            {
                foreach (var child in value)
                {
                    child.Parent = this;
                }
                _children = value;
            }
        }

        public bool IsLeaf => Children.Count == 0; 

        public int Index => Parent.Children.IndexOf(this);

        public Node<T> Left => Index > 0 ? Parent.Children[Index - 1] : null;

        public Node<T> Right => Index < Parent.Children.Count - 1 ? Parent.Children[Index + 1] : null;

        public Node(List<KeyValue<T>> records, Node<T> parent)
        {
            Keys = records;
            Parent = parent;
            Children = new List<Node<T>>();
        }

        public Node() { }

        public override string ToString()
        {
            return string.Join(", ", Keys);
        }
    }
}