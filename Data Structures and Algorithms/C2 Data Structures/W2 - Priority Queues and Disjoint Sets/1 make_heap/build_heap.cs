using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.W2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Process(MakeHeap.Process);
        }
        private static void Process(Func<string[], string[]> process)
        {
            var input = new List<string>();
            string s;
            while ((s = Console.ReadLine()) != null)
            {
                input.Add(s);
            }

            foreach (var item in process(input.ToArray()))
            {
                Console.WriteLine(item);
            }
        }
    }

    public class MakeHeap
    {
        public static string[] Process(string[] inputs)
        {
            var n = int.Parse(inputs[0]);
            var data = inputs[1]
                .Split(new[] { ' ' })
                .Take(n)
                .Select(int.Parse)
                .ToArray();

            var heap = BuildHeap(n, data);
            var countResult = new[] { heap.Swaps.Count.ToString() };
            var swapResults = heap.Swaps.Select((pair) => string.Format("{0} {1}", pair.Item1, pair.Item2));
            return countResult.Concat(swapResults).ToArray();
        }
        public static BinaryHeap BuildHeap(int n, int[] data)
        {
            var current = data.Take(n).ToArray();
            var heap = new MinBinaryHeap(current);
            for (int i = n / 2; i >= 0; i--)
            {
                heap.SiftDown(i);
            }
            return heap;
        }
    }

    public class MinBinaryHeap : BinaryHeap
    {
        public MinBinaryHeap(int[] source) : base(source) { }
        public override int SiftUp(int i)
        {
            var swapId = i;
            while (i > 0 && Value(ParentId(i)) > Value(i))
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
            if (r < _size && Value(r) < Value(maxIndex)) maxIndex = r;

            var l = LeftChildId(i);
            if (l < _size && Value(l) < Value(maxIndex)) maxIndex = l;

            if (i != maxIndex)
            {
                Swap(i, maxIndex);
                return SiftDown(maxIndex);
            }
            return maxIndex;
        }
    }

    public class MaxBinaryHeap : BinaryHeap
    {
        public MaxBinaryHeap(int[] source) : base(source) { }
        public override int SiftUp(int i)
        {
            var swapId = i;
            while (i > 0 && Value(ParentId(i)) < Value(i))
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
            if (l < _size && Value(l) > Value(maxIndex)) maxIndex = l;

            var r = RightChildId(i);
            if (r < _size && Value(r) > Value(maxIndex)) maxIndex = r;

            if (i != maxIndex)
            {
                Swap(i, maxIndex);
                return SiftDown(maxIndex);
            }
            return maxIndex;
        }
    }

    public abstract class BinaryHeap
    {
        protected int _maxSize;
        protected int _size;
        private int[] H;
        
        protected BinaryHeap(int[] source)
        {
            H = source;
            _maxSize = source.Length;
            _size = source.Length;
        }

        protected int ParentId(int i) { return ((i - 1) / 2); }
        protected int LeftChildId(int i) { return 2 * i + 1; }
        protected int RightChildId(int i) { return 2 * i + 2; }

        protected int Value(int i) { return H[i]; }

        public abstract int SiftUp(int i);

        public abstract int SiftDown(int i);

        public void Insert(int p)
        {
            if (_size == _maxSize) throw new ArgumentOutOfRangeException();

            _size += 1;
            H[_size] = p;
            SiftUp(_size);
        }

        public int ExtractMax()
        {
            var result = H[1];
            H[1] = H[_size];
            _size -= 1;
            SiftDown(1);

            return result;
        }

        public void Remove(int i)
        {
            H[i] = int.MaxValue;
            SiftUp(i);
            ExtractMax();
        }

        public void ChangePriority(int i, int p)
        {
            var q = H[i];
            H[i] = p;
            if (p > q)
                SiftUp(i);
            else
                SiftDown(i);

        }

        protected void Swap(int id1, int id2)
        {
            var temp = H[id2];
            H[id2] = H[id1];
            H[id1] = temp;

            _swaps.Add(new Tuple<int, int>(id1, id2));
        }

        private List<Tuple<int, int>> _swaps = new List<Tuple<int, int>>();
        public IReadOnlyCollection<Tuple<int, int>> Swaps { get { return _swaps; } }
    }
}
