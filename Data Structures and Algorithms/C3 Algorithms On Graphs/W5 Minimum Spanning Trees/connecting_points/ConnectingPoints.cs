using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W5
{
    public class ConnectingPoints
    {
        public static void Main(string[] args)
        {
            string s;
            var inputs = new List<string>();
            while ((s = Console.ReadLine()) != null)
                inputs.Add(s);

            foreach (var result in Answer(inputs.ToArray()))
                Console.WriteLine(result);
        }

        private const long PositiveInfinity = long.MaxValue;
        public static IList<string> Answer(IList<string> inputs)
        {
            var gis = Inputs.AdjacencyListGraphDecimal(inputs);
            var points = gis.ToPoints();
            var verticeCount = points.Item1;
            var vertices = points.Item2.ToArray();

            //make edge for all points to all other points except self
            //  with weight = SQRT( SQR(x1 - x2) + SQR(y1 - y2))
            //  left = index of point
            //  right = index of to point
            var lines = 
                from x in Enumerable.Range(0, verticeCount)
                from y in Enumerable.Range(0, verticeCount)
                where x != y
                let xp = vertices[x]
                let yp = vertices[y]
                select new Edge<decimal> {Left = x, Right = y, Weight = GetDistance(xp,yp)};
            //then build tree from edges
            var g = gis.ToUndirectedAdjacencyGraph(new Tuple<int, IEnumerable<Edge<decimal>>>(verticeCount,lines));
            var primsResult = PrimsAlgorithm(g);
            var answer = primsResult.Cost.Values.Sum();

            return new [] { answer.ToString("0.000000000") };
        }

        private static decimal GetDistance(Point a, Point b)
        {
            var value = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            return Convert.ToDecimal(value);
        }

        private static PrimsResult PrimsAlgorithm(AdjacencyListGraph<decimal> graph)
        {
            var size = graph.Size();

            //Initialize result with 0 as first vertex
            var result = new PrimsResult(size);
            result.Cost.SetValue(0,0);

            //priority queue of costs
            var q = new MinPriorityQueue<decimal>(size, decimal.MaxValue);
            for (var i = 0; i < size; i++)
            {
                q.Enqueue(i,result.Cost.GetValue(i));
            }

            //Walk
            while (!q.IsEmpty())
            {
                //Console.WriteLine("Queue: {0}", q);
                var currentIndex = q.Dequeue();

                //Console.WriteLine("Extract: {0}", currentIndex);
                foreach (var edge in graph.Neighbors(currentIndex))
                {
                    var z = edge.Right;
                    var w = edge.Weight;
                    if (!q.Contains(z) || result.Cost.GetValue(z) <= w) continue;

                    result.Cost.SetValue(z,w);
                    result.Parent.SetValue(z,currentIndex);
                    q.ChangePriority(z,w);
                }
            }
            return result;
        }


        private class PrimsResult
        {
            public PrimsResult(int size)
            {
                Parent = new SearchData<int>(size, -1);
                Cost = new SearchData<decimal>(size, PositiveInfinity);
            }
            public SearchData<decimal> Cost { get; private set; }
            public SearchData<int> Parent { get; private set; }
        }
    }

    public class SearchData<T> where T : IEquatable<T>
    {
        private readonly T[] _values;
        public readonly T InitialValue;
        public SearchData(int size, T initialValue)
        {
            InitialValue = initialValue;
            _values = Enumerable.Repeat(initialValue, size).ToArray();
        }
        
        public bool Visited(int v)
        {
            return !_values[v].Equals(InitialValue);
        }

        public int Length {get { return _values.Length; }}
        public ICollection<T> Values { get { return _values; } }
        public virtual void SetValue(int v, T value)
        {
            _values[v] = value;
        }
        public virtual T GetValue(int v)
        {
            return _values[v];
        }

        public override string ToString()
        {
            var items = _values.Select((v, i) => string.Format("{0}:{1}", i, v));
            return string.Join(", ", items);
        }
    }


    public class Inputs
    {
        public static AdjacencyListGraphInput<long> AdjacencyListGraphLong(IList<string> inputs)
        {
            return new AdjacencyListGraphInput<long>(inputs, long.Parse);
        }
        public static AdjacencyListGraphInput<decimal> AdjacencyListGraphDecimal(IList<string> inputs)
        {
            return new AdjacencyListGraphInput<decimal>(inputs, decimal.Parse);
        }
        public static AdjacencyListGraphInput<double> AdjacencyListGraphDouble(IList<string> inputs)
        {
            return new AdjacencyListGraphInput<double>(inputs, double.Parse);
        }
    }

    public class AdjacencyListGraphInput<TWeight> where TWeight : IComparable<TWeight>, IEquatable<TWeight>
    {
        private int _lineCursor;
        private readonly IList<string> _inputs;
        private readonly Func<string, TWeight> _parseWeight;
        public AdjacencyListGraphInput(IList<string> inputs, Func<string, TWeight> parseWeight = null)
        {
            _inputs = inputs;
            _parseWeight = parseWeight;
        }

        public Tuple<int, IEnumerable<Point>> ToPoints()
        {
            var pointCount = int.Parse(_inputs[0]);
            var points = Enumerable.Range(1, pointCount).Select(i => ParsePoint(_inputs[i]));
            _lineCursor = pointCount + 1;

            return new Tuple<int, IEnumerable<Point>>(pointCount, points);
        }

        public Tuple<int,IEnumerable<Edge<TWeight>>> ToEdges()
        {
            var line0 = ParseIntPair(_inputs[0]);
            var verticeCount = line0.Item1;
            var edgeCount = line0.Item2;

            var edges = Enumerable.Range(1, edgeCount).Select(i => ParseEdge(_inputs[i]));
            _lineCursor = edgeCount + 1;

            return new Tuple<int, IEnumerable<Edge<TWeight>>>(verticeCount, edges);
        }

        public AdjacencyListGraph<TWeight> ToUndirectedAdjacencyGraph(Tuple<int, IEnumerable<Edge<TWeight>>> edges = null)
        {
            return ToAdjacencyGraph(edges ?? ToEdges(), (g, e) =>
            {
                g.AddDirectedEdge(e);
                g.AddDirectedEdge(ReverseEdge(e));
            });
        }

        public AdjacencyListGraph<TWeight> ToDirectedAdjacencyGraph(Tuple<int, IEnumerable<Edge<TWeight>>> edges = null)
        {
            return ToAdjacencyGraph(edges ?? ToEdges(), 
                (g, e) => g.AddDirectedEdge(e));
        }

        public AdjacencyListGraph<TWeight> ToDirectedReverseAdjacencyGraph(Tuple<int, IEnumerable<Edge<TWeight>>> edges = null)
        {
            return ToAdjacencyGraph(edges ?? ToEdges(), 
                (g, e) => g.AddDirectedEdge(ReverseEdge(e)));
        }

        private static AdjacencyListGraph<TWeight> ToAdjacencyGraph(Tuple<int, IEnumerable<Edge<TWeight>>> edges,
            Action<AdjacencyListGraph<TWeight>, Edge<TWeight>> addEdge)
        {
            return ToAdjacencyGraph(edges.Item1, edges.Item2, addEdge);
        }

        private static AdjacencyListGraph<TWeight> ToAdjacencyGraph(int verticeCount, IEnumerable<Edge<TWeight>> edges, Action<AdjacencyListGraph<TWeight>, Edge<TWeight>> addEdge)
        {
            var graph = new AdjacencyListGraph<TWeight>(verticeCount);
            foreach (var edge in edges)
            {
                addEdge(graph, edge);
            }
            return graph;
        }

        public Edge<TWeight> NextAsEdge()
        {
            return ParseEdge(_inputs[_lineCursor++]);
        }

        public int NextAsIndex()
        {
            return ParseIndex(_inputs[_lineCursor++]);
        }

        private static readonly char[] Splits = { ' ' };
        public static Tuple<int, int> ParseIntPair(string line)
        {
            var s = line.Split(Splits);
            return new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1]));
        }
        public Edge<TWeight> ParseEdge(string line)
        {
            //Index given in 1 convert to 0
            var linePath = line.Split(Splits, StringSplitOptions.RemoveEmptyEntries);
            return new Edge<TWeight>
            {
                Left = ParseIndex(linePath[0]),
                Right = ParseIndex(linePath[1]),
                Weight = (linePath.Length ==3)? _parseWeight(linePath[2]) : default(TWeight)
            };
        }
        public static Point ParsePoint(string line)
        {
            var s = line.Split(Splits);
            return new Point {X = int.Parse(s[0]), Y = int.Parse(s[1])};
        }
        public static int ParseIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }

        public static decimal NoOpDecimal(decimal source)
        {
            return source;
        }

        private static string ParseSource(int index)
        {
            return (index + 1).ToString(); //input is 1 based return zero based
        }

        private static Edge<TWeight> ReverseEdge(Edge<TWeight> e)
        {
            return new Edge<TWeight>
            {
                Right = e.Left,
                Left = e.Right,
                Weight = e.Weight
            };
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Edge<TWeight>
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public TWeight Weight { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", Right, Weight);
        }
    }
    public class AdjacencyListGraph<TWeight>
    {
        private readonly AdjacencyListArray<TWeight> _lists;
        public AdjacencyListGraph(int size)
        {
            _lists = new AdjacencyListArray<TWeight>(size);
        }

        public int Size()
        {
            return _lists.Length;
        }

        public IEnumerable<Edge<TWeight>> Neighbors(int i)
        {
            return _lists[i].Select(kvp => kvp.Value);
        }
        public IEnumerable<int> NeighborIndexes(int i)
        {
            return _lists[i].Select(kvp => kvp.Value.Right);
        }

        public void AddDirectedEdge(Edge<TWeight> edge)
        {
            _lists[edge.Left][edge.Right] = edge;
        }

        public override string ToString()
        {
            return _lists.ToString();
        }
    }

    public class AdjacencyList<TWeight> : Dictionary<int,Edge<TWeight>>
    {
        public bool IsSink { get { return !this.Any(); } }

        public override string ToString()
        {
            return string.Join(", ", this.Select(kvp => kvp.Value.ToString()));
        }
    }

    public class AdjacencyListArray<TWeight>
    {
        private readonly AdjacencyList<TWeight>[] _list;
        public AdjacencyListArray(int size)
        {
            _list = new AdjacencyList<TWeight>[size];
        }
        public AdjacencyList<TWeight> this[int i]
        {
            get
            {
                return _list[i] ?? (_list[i] = new AdjacencyList<TWeight>());
            }
        }

        public int Length { get { return _list.Length; } }

        public override string ToString()
        {
            var listItems = _list.Select((item, i) => string.Format("{0} => {1}", i, item == null ? "" : item.ToString()));
            return string.Join(" || ", listItems);
        }
    }
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
