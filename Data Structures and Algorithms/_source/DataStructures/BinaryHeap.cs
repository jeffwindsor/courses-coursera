using System;

namespace DataStructures.W2
{
    public class MaxBinaryHeap<T> : BinaryHeap<T> where T : IComparable<T>
    {
        public MaxBinaryHeap(T[] source) : base(source) { }

        public override int SiftUp(int i)
        {
            var swapId = i;
            while (i > 0 && Value(ParentId(i)).CompareTo(Value(i)) > 0 )
            {
                swapId = ParentId(i);
                Swap(i, swapId);
                i = swapId;
            }
            return swapId;
        }

        public override int SiftDown(int i)
        {
            var maxIndex = i;
            var l = LeftChildId(i);
            if (l < _size && Value(l).CompareTo(Value(maxIndex)) > 0) maxIndex = l;

            var r = RightChildId(i);
            if (r < _size && Value(r).CompareTo(Value(maxIndex)) > 0) maxIndex = r;

            if (i != maxIndex)
            {
                Swap(i, maxIndex);
                return SiftDown(maxIndex);
            }
            return maxIndex;
        }
    }

    public class MinBinaryHeap<T> : BinaryHeap<T> where T : IComparable<T>
    {
        public MinBinaryHeap(T[] source) : base(source)
        {
            for (int i = source.Length / 2; i >= 0; i--)
            {
                SiftDown(i);
            }
        }

        public override int SiftUp(int i)
        {
            var swapId = i;
            while (i > 0 && Value(ParentId(i)).CompareTo(Value(i)) > 0)
            {
                swapId = ParentId(i);
                Swap(i, swapId);
                i = swapId;
            }
            return swapId;
        }

        public override int SiftDown(int i)
        {
            var maxIndex = i;

            var r = RightChildId(i);
            if (r < _size && Value(r).CompareTo(Value(maxIndex)) < 0) maxIndex = r;

            var l = LeftChildId(i);
            if (l < _size && Value(l).CompareTo(Value(maxIndex)) < 0) maxIndex = l;

            if (i != maxIndex)
            {
                Swap(i, maxIndex);
                return SiftDown(maxIndex);
            }
            return maxIndex;
        }
    }

    public abstract class BinaryHeap<T> where T : IComparable<T>
    {
        protected int _maxSize;
        protected int _size;
        private T[] H;

        protected BinaryHeap(T[] source)
        {
            H = source;
            _maxSize = source.Length;
            _size = source.Length;
        }

        protected int ParentId(int i) { return ((i - 1) / 2); }
        protected int LeftChildId(int i) { return 2 * i + 1; }
        protected int RightChildId(int i) { return 2 * i + 2; }

        protected T Value(int i) { return H[i]; }

        public int Size
        {
            get { return _size; }
        }
        public abstract int SiftUp(int i);

        public abstract int SiftDown(int i);

        public void Insert(T p)
        {
            if (_size == _maxSize) throw new ArgumentOutOfRangeException();

            _size += 1;
            H[_size - 1] = p;
            SiftUp(_size - 1);
        }

        public T ExtractMax()
        {
            var result = H[0];
            H[0] = H[_size - 1];
            _size -= 1;
            SiftDown(0);

            return result;
        }

        //public void Remove(int i)
        //{
        //    H[i] = int.MaxValue;
        //    SiftUp(i);
        //    ExtractMax();
        //}

        public void ChangePriority(int i, T p)
        {
            var q = H[i];
            H[i] = p;
            if (p.CompareTo(q) > 0)
                SiftUp(i);
            else
                SiftDown(i);

        }

        protected void Swap(int id1, int id2)
        {
            var temp = H[id2];
            H[id2] = H[id1];
            H[id1] = temp;
        }
    }
}
