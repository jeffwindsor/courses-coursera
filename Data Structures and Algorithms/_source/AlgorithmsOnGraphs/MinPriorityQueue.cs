using System;
using System.Linq;

namespace AlgorithmsOnGraphs
{
    public class MinPriorityQueue<TPriority> where TPriority : IComparable<TPriority>
    {
        private class Node
        {
            public int Value { get; set; }
            public TPriority Priority { get; set; }
        }

        private const int NOT_IN_HEAP = -1;
        private readonly int _maxSize;
        private int _currentSize;
        private readonly Node[] _heap;
        private readonly int[] _valueToHeapIndexMap;
        private readonly TPriority _maxPriority;

        public MinPriorityQueue(int size, TPriority maxPriority)
        {
            _heap = new Node[size];
            _valueToHeapIndexMap = Enumerable.Range(0,size).Select(_ => NOT_IN_HEAP).ToArray();
            _maxSize = size;
            _maxPriority = maxPriority;
        }

        public void Enqueue(int value, TPriority priority)
        {
            if (_currentSize == _maxSize) throw new ArgumentOutOfRangeException();
            _currentSize += 1;

            _valueToHeapIndexMap[value] = LastIndex;
            _heap[LastIndex] = new Node { Value = value, Priority = priority };
            SiftUp(LastIndex);
        }

        public int Dequeue()
        {
            var result = _heap[FirstIndex];

            Swap(FirstIndex, LastIndex);
            _heap[LastIndex] = null;
            _valueToHeapIndexMap[result.Value] = NOT_IN_HEAP;
            _currentSize -= 1;

            SiftDown(FirstIndex);
            return result.Value;
        }

        private int Remove(int index)
        {
            _heap[index].Priority = _maxPriority;
            SiftUp(index);
            return Dequeue();
        }

        public bool Contains(int value)
        {
            return _valueToHeapIndexMap[value] != NOT_IN_HEAP;
        }

        public void ChangePriority(int value, TPriority priority)
        {
            var heapIndex = _valueToHeapIndexMap[value];
            if (heapIndex == NOT_IN_HEAP)
                return;

            var node = _heap[heapIndex];
            var oldPriority = node.Priority;
            node.Priority = priority;

            if (IsPrioritySwap(priority, oldPriority))
                SiftUp(heapIndex);
            else
                SiftDown(heapIndex);
        }

        private static int ParentIndex(int i) { return ((i - 1) / 2); }
        private static int LeftChildIndex(int i) { return 2 * i + 1; }
        private static int RightChildIndex(int i) { return 2 * i + 2; }
        private int LastIndex { get { return _currentSize - 1; }}
        private const int FirstIndex = 0;
        public bool IsEmpty()
        {
            return _currentSize == 0;
        }
        

        private void SiftUp(int index)
        {
            while (index > FirstIndex && IsSwap(index, ParentIndex(index)))
            {
                var parentIndex = ParentIndex(index);
                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void SiftDown(int index)
        {
            while (true)
            {
                var maxIndex = index;

                var rightIndex = RightChildIndex(index);
                if (rightIndex < _currentSize && IsSwap(rightIndex, maxIndex))
                    maxIndex = rightIndex;

                var leftIndex = LeftChildIndex(index);
                if (leftIndex < _currentSize && IsSwap(leftIndex, maxIndex))
                    maxIndex = leftIndex;

                if (index == maxIndex) return;

                Swap(index, maxIndex);
                index = maxIndex;
            }
        }

        private bool IsSwap(int source, int target)
        {
            return IsPrioritySwap(_heap[source].Priority, _heap[target].Priority);
        }

        private static bool IsPrioritySwap(TPriority source, TPriority target)
        {
            return source.CompareTo(target) < 0; //   source < target;
        }

        private void Swap(int source, int target)
        {
            var one = _heap[source];
            var two = _heap[target];

            _valueToHeapIndexMap[one.Value] = target;
            _valueToHeapIndexMap[two.Value] = source;

            _heap[target] = one;
            _heap[source] = two;


        }

        public override string ToString()
        {
            var values = _heap.Take(_currentSize).Select((n, i) => string.Format("{0}:{1}", n.Priority, n.Value));
            return string.Join(", ", values);
        }
    }
}