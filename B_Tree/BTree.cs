using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B_Tree
{
    public enum Mode
    {
        Normal, Test
    }
    public class BTree<T>
    {
        public Node<T> Root { get; private set; }
        private int _limit;
        private int _numberOfComparisons;
        public BTree(int limit)
        {
            _limit = limit;
        }
        public BTree(Mode mode)
        {
            if (mode == Mode.Test)
            {
                _limit = 3;
            }
            else
            {
                _limit = 50;
            }
        }

        // search data 
        public T SearchData(int key)
        {
            _numberOfComparisons = 0;
            T result = RecursiveDataSearch(key, Root); 
            if (isDefault(result)) Console.WriteLine($"No key {key} in the tree");
            else Console.WriteLine($"Search result:\n{key} {result}\nComparisons: { _numberOfComparisons}\n");
            return result;
        }

        bool isDefault(T @object) => @object == null ? default(T) == null : @object.Equals(default(T));

        private T RecursiveDataSearch(int key, Node<T> current)
        {
            T result;
            int keyIndex = FindIndex(key, current);
            if (keyIndex < current.Keys.Count && current.Keys[keyIndex].Key == key)
            {
                return current.Keys[keyIndex].Value;
            }
            _numberOfComparisons++;
            if (!current.IsLeaf) result = RecursiveDataSearch(key, current.Children[keyIndex]);
            else return default;
            return result;
        }

        // search result
        private Node<T> Search(int key)
        {
            return RecursiveSearch(key, Root);
        }

        private Node<T> RecursiveSearch(int key, Node<T> current)
        {
            Node<T> result;
            int keyIndex = FindIndex(key, current);
            if (keyIndex < current.Keys.Count && current.Keys[keyIndex].Key == key)
            {
                return current;
            }
            if (!current.IsLeaf)
                result = RecursiveSearch(key, current.Children[keyIndex]);
            else
                return null;
            return result;
        }

        // insert 
        public void Insert(T data, int key, bool print)
        {
            if (Root == null)
                Root = new Node<T>(new List<KeyValue<T>> { new(key, data) }, null);
            else
            {
                Node<T> current = Root;
                RecursiveInsert(new KeyValue<T>(key, data), current);
            }
            if (print) Console.WriteLine($"New	record	inserted:{ key}.{ data}");
        }

        private void RecursiveInsert(KeyValue<T> newRecord, Node<T> current)
        {
            int index = FindIndex(newRecord.Key, current); 
            if (!current.IsLeaf)
            {
                RecursiveInsert(newRecord, current.Children[index]);
            }
            else
                current.Keys.Insert(FindIndex(newRecord.Key, current), newRecord);
            if (current.Keys.Count == 2 * _limit - 1)
            {
                Split(current);
            }
        }

        private void Split(Node<T> current)
        {
            KeyValue<T> middleRecord = current.Keys[_limit - 1];
            Node<T> left = new(current.Keys.GetRange(0, _limit - 1), current.Parent);
            Node<T> right = new(current.Keys.GetRange(_limit, _limit - 1), current.Parent);
            if (!current.IsLeaf)
            {
                left.Children = current.Children.GetRange(0, _limit); right.Children = current.Children.GetRange(_limit, _limit);
            }
            current.Keys.RemoveAt(_limit - 1);
            if (current.Parent != null)
            {
                int index = FindIndex(current.Keys[_limit].Key, current.Parent);
                current.Parent.Keys.Insert(index, middleRecord); 
                current.Parent.Children[index] = left; 
                current.Parent.Children.Insert(index + 1, right);
            }
            else
            {
                Root = new Node<T>(new List<KeyValue<T>> { middleRecord }, null);
                left.Parent = Root;
                right.Parent = Root;
                Root.Children.Add(left);
                Root.Children.Add(right);
            }
        }

        // output
        public override string ToString()
        {
            StringBuilder output = new StringBuilder(); 
            RecursiveOutput(ref output, Root, 0, false); 
            return output.ToString();
        }

        private void RecursiveOutput(ref StringBuilder current, Node<T> node, int depth, bool isLast)
        {
            current.Append(Multiply("│ ", depth) + (isLast ? "└─ " : "├─ ") + "(" + node + ")" + "\n");
            if (!node.IsLeaf)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    RecursiveOutput(ref current, node.Children[i], depth + 1, i == node.Children.Count - 1 && node.Children[i].IsLeaf);
                }
            }
        }
        private static string Multiply(string str, int times)
        {
            return string.Concat(Enumerable.Repeat(str, times));
        }

        // delete
        public bool Delete(int key)
        {
            Node<T> removedNode = Search(key);
            if (removedNode == null)
            {
                Console.WriteLine($"There is no key {key} in the tree"); 
                return false;
            }
            if (removedNode.IsLeaf) DeleteLeaf(key, removedNode);
            else DeleteInside(key, removedNode);
            Console.WriteLine($"Record {key} deleted"); 
            return true;
        }

        private void DeleteLeaf(int key, Node<T> current)
        {
            int removedKeyIndex = FindIndex(key, current); current.Keys.RemoveAt(removedKeyIndex);
            if (current.Keys.Count >= _limit - 1)
                return;
            RebalanceLeaf(current);
        }

        private void DeleteInside(int key, Node<T> current)

        {

            int removedKeyIndex = FindIndex(key, current); current.Keys.RemoveAt(removedKeyIndex); RebalanceInside(current, removedKeyIndex);

        }

        private void RebalanceInside(Node<T> current, int keyIndex)
        {
            Node<T> leftSubnode = current.Children[keyIndex]; 
            Node<T> rightSubnode = null;
            if (keyIndex + 1 < current.Children.Count) rightSubnode = current.Children[keyIndex + 1];
            if (leftSubnode != null && leftSubnode.Keys.Count > _limit - 1)
            {
                KeyValue<T> newRecord = leftSubnode.Keys[^1]; current.Keys.Insert(FindIndex(newRecord.Key, current), newRecord);
                leftSubnode.Keys.Remove(newRecord);
            }
            else if (rightSubnode != null && rightSubnode.Keys.Count > _limit - 1)
            {
                KeyValue<T> newRecord = rightSubnode.Keys[0]; current.Keys.Insert(FindIndex(newRecord.Key, current), newRecord);
                rightSubnode.Keys.Remove(newRecord);
            }
            else if (leftSubnode != null)
            {
                KeyValue<T> newRecord = leftSubnode.Keys[^1]; current.Keys.Insert(FindIndex(newRecord.Key, current), newRecord);
                leftSubnode.Keys.Remove(newRecord);
                if (!leftSubnode.IsLeaf)
                    RebalanceInside(leftSubnode, FindIndex(newRecord.Key, current));
                else
                    RebalanceLeaf(leftSubnode);
            }
            else if (rightSubnode != null)
            {
                KeyValue<T> newRecord = rightSubnode.Keys[0]; current.Keys.Insert(FindIndex(newRecord.Key, current), newRecord);
                rightSubnode.Keys.Remove(newRecord);
                if (!rightSubnode.IsLeaf)
                    RebalanceInside(rightSubnode, FindIndex(newRecord.Key, current));
                else
                    RebalanceLeaf(rightSubnode);
            }
        }

        private void RebalanceLeaf(Node<T> current)
        {
            if (current.Left != null && current.Left.Keys.Count > _limit - 1)
                RotateLeft(current);
            else if (current.Right != null && current.Right.Keys.Count > _limit - 1)
                RotateRight(current);
            else if (current.Left != null)
                MergeLeft(current);
            else
                MergeRight(current);
        }

        //rotate
        private void RotateLeft(Node<T> current)
        {
            Rotate(current, current.Left, true);
        }

        private void RotateRight(Node<T> current)
        {
            Rotate(current, current.Right, false);
        }

        private void Rotate(Node<T> current, Node<T> neighbour, bool rotateLeft)
        {
            KeyValue<T> parentRecord = current.Parent.Keys[rotateLeft ? current.Index - 1 : current.Index];
            current.Keys.Insert(FindIndex(parentRecord.Key, current), parentRecord);
            current.Parent.Keys.Remove(parentRecord);
            KeyValue<T> neighbourRecord = neighbour.Keys[rotateLeft ? ^1 : 0];
            current.Parent.Keys.Insert(FindIndex(neighbourRecord.Key, current.Parent), neighbourRecord);
            neighbour.Keys.Remove(neighbourRecord);
        }

        // merge
        private void MergeLeft(Node<T> current)

        {

            Merge(current, current.Left, true);

        }

        private void MergeRight(Node<T> current)

        {
            Merge(current, current.Right, false);

        }

        private void Merge(Node<T> current, Node<T> neighbour, bool rotateLeft)
        {
            current.Keys.InsertRange(rotateLeft ? 0 :
            current.Keys.Count, neighbour.Keys);
            foreach (var child in neighbour.Children)
            {
                child.Parent = current;
            }
            current.Children.InsertRange(rotateLeft ? 0 :
            current.Children.Count, neighbour.Children);
            KeyValue<T> parentRecord = current.Parent.Keys[rotateLeft ?

            current.Index - 1 : current.Index]; current.Parent.Children.Remove(neighbour);

            current.Keys.Insert(FindIndex(parentRecord.Key, current),
            parentRecord);

            current.Parent.Keys.Remove(parentRecord);

            if (current.Parent != null && current.Parent.Parent == null && current.Parent.Keys.Count == 0)

            {

                current.Parent = null;

                Root = current;

            }

            else if (current.Parent != null && current.Parent.Parent != null

            && current.Parent.Keys.Count < _limit - 1) RebalanceLeaf(current.Parent);

        }

        public int FindIndex(int key, Node<T> current)
        {
            _numberOfComparisons = 0;
            int left = 0;
            int right = current.Keys.Count;
            while (left <= right && left < current.Keys.Count)
            {
                int middle = (left + right) / 2;
                if (current.Keys[middle].Key == key)
                    return middle;
                _numberOfComparisons++;
                if (current.Keys[middle].Key < key)
                    left = middle + 1;
                else
                    right = middle - 1;
            }
            return left;
        }

        public List<KeyValue<T>> Obhid()
        {
            List<KeyValue<T>> visited = new();
            Node<T> current = new(new List<KeyValue<T>>(), null); 
            current.Children = new(); 
            current.Keys.AddRange(Root.Keys); 
            current.Children.AddRange(Root.Children); 
            RecursiveObhid(ref visited, current); 
            visited.Sort((a, b) => a.Key.CompareTo(b.Key)); 
            return visited;
        }

        private void RecursiveObhid(ref List<KeyValue<T>> visited, Node<T> current)
        {
            if (current == null)
                return;
            for (int i = 0; i < current.Children.Count; i++)
            {
                RecursiveObhid(ref visited, current.Children[i]);
            }
            visited.AddRange(current.Keys);
        }
    }
}

