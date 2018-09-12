using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W4
{
    public class Dijkstras
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

        public static IList<string> Answer(IList<string> inputs)
        {
            var gi = new AdjacencyListGraphInput(inputs);
            var graph = gi.ToDirectedAdjacencyGraph();
            var points = gi.NextAsEdge();
            //Console.WriteLine(graph);

            var s = new DijkstrasAlgorithm(graph);
            var cost = s.LowestCostPath(points.Left,points.Right);
            
            return new[] { cost.ToString() };
        }
    }

    public class DijkstrasAlgorithm
    {
        private readonly AdjacencyListGraph _graph;
        private SearchData _distance;
        private SearchData _visitedFrom;
        private const int MaxDistance = int.MaxValue;

        public DijkstrasAlgorithm(AdjacencyListGraph g)
        {
            _graph = g;
        }

        public int LowestCostPath(int from, int to)
        {
            //Search From to establish visited values
            Explore(from);
            //Return lowest cost for path
            var d = _distance.GetValue(to);
            return (d == MaxDistance) ? -1 : d;
        }
        
        private void Explore(int start)
        {
            _visitedFrom = new SearchData(_graph.Size());
            _distance = new SearchData(_graph.Size(), MaxDistance);
            _distance.SetValue(start,0);

            //Make Prioirty Queue
            var pq = new MinPriorityQueue(_graph.Size());
            for (var i = 0; i < _distance.Length; i++)
            {
                pq.Enqueue(i,_distance.GetValue(i));
            }

            while (!pq.IsEmpty())
            {
                //Console.WriteLine("Queue: {0}", pq);

                var currentIndex = pq.Dequeue();

                //Console.WriteLine("Extract: {0}", currentIndex);

                foreach (var edge in _graph.Neighbors(currentIndex))
                {
                    var neighborIndex = edge.Right;
                    var d = _distance.GetValue(neighborIndex);
                    var dFromC = _distance.GetValue(currentIndex) == MaxDistance
                        ? MaxDistance
                        : _distance.GetValue(currentIndex) + edge.Weight;

                    //Console.WriteLine("Edge {1} => {0} : Distance {2} : {3}",neighborIndex,currentIndex,dFromC,d);

                    if (d <= dFromC) continue;

                    //Set New Distance Values
                    _distance.SetValue(neighborIndex, dFromC);
                    _visitedFrom.SetValue(neighborIndex, currentIndex);
                    pq.ChangePriority(neighborIndex, dFromC);
                }
            }
        }
    }
    public class MinPriorityQueue
    {
        private class Node
        {
            public int Value { get; set; }
            public int Priority { get; set; }
        }

        private const int NOT_IN_HEAP = -1;
        private readonly int _maxSize;
        private int _currentSize;
        private readonly Node[] _heap;
        private readonly int[] _valueToHeapIndexMap;

        public MinPriorityQueue(int size)
        {
            _heap = new Node[size];
            _valueToHeapIndexMap = Enumerable.Range(0,size).Select(_ => NOT_IN_HEAP).ToArray();
            _maxSize = size;
        }

        public void Enqueue(int value, int priority)
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
            _heap[index].Priority = int.MaxValue;
            SiftUp(index);
            return Dequeue();
        }

        public void ChangePriority(int value, int priority)
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

        private static bool IsPrioritySwap(int source, int target)
        {
            return source < target;
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
    public class SearchData
    {
        private readonly int[] _values;
        public readonly int InitialValue;
        public SearchData(int size, int initialValue = -1)
        {
            InitialValue = initialValue;
            _values = Enumerable.Repeat(initialValue, size).ToArray();
        }
        
        public bool Visited(int v)
        {
            return _values[v] != InitialValue;
        }

        public int Length {get { return _values.Length; }}
        public ICollection<int> Values { get { return _values; } }
        public virtual void SetValue(int v, int value)
        {
            _values[v] = value;
        }
        public virtual int GetValue(int v)
        {
            return _values[v];
        }

        public override string ToString()
        {
            var items = _values.Select((v, i) => string.Format("{0}:{1}", i, v));
            return string.Join(", ", items);
        }
    }
    public class AdjacencyListGraphInput
    {
        private int _lineCursor;
        private readonly IList<string> _inputs;

        public AdjacencyListGraphInput(IList<string> inputs)
        {
            _inputs = inputs;
        }

        public AdjacencyListGraph ToUndirectedAdjacencyGraph()
        {
            return ToAdjacencyGraph((g, e) =>
            {
                g.AddDirectedEdge(e);
                g.AddDirectedEdge(ReverseEdge(e));
            });
        }
        public AdjacencyListGraph ToDirectedAdjacencyGraph()
        {
            return ToAdjacencyGraph((g, e) => g.AddDirectedEdge(e));
        }

        public AdjacencyListGraph ToDirectedReverseAdjacencyGraph()
        {
            return ToAdjacencyGraph((g, e) => g.AddDirectedEdge(ReverseEdge(e)));
        }

        private AdjacencyListGraph ToAdjacencyGraph(Action<AdjacencyListGraph, Edge> addEdge)
        {
            var line0 = ParseIntPair(_inputs[0]);
            var verticeCount = line0.Item1;
            var edgeCount = line0.Item2;
            var graph = new AdjacencyListGraph(verticeCount);
            foreach (var edge in Enumerable.Range(1, edgeCount).Select(i => ParseEdge(_inputs[i])))
            {
                addEdge(graph, edge);
            }
            _lineCursor = edgeCount + 1;

            return graph;
        }

        public Edge NextAsEdge()
        {
            return ParseEdge(_inputs[_lineCursor++]);
        }


        private static char[] Splits = { ' ' };
        public static Tuple<int, int> ParseIntPair(string line)
        {
            var s = line.Split(Splits);
            return new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1]));
        }
        public static Edge ParseEdge(string line)
        {
            //Index given in 1 convert to 0
            var linePath = line.Split(Splits, StringSplitOptions.RemoveEmptyEntries);
            return new Edge
            {
                Left = ParseIndex(linePath[0]),
                Right = ParseIndex(linePath[1]),
                Weight = (linePath.Length ==3)? int.Parse(linePath[2]) : 0
            };
        }
        public static int ParseIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }
        private static string ParseSource(int index)
        {
            return (index + 1).ToString(); //input is 1 based return zero based
        }

        private static Edge ReverseEdge(Edge e)
        {
            return new Edge
            {
                Right = e.Left,
                Left = e.Right,
                Weight = e.Weight
            };
        }
    }
    public class Edge
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", Right, Weight);
        }
    }
    public class AdjacencyListGraph
    {
        private readonly AdjacencyListArray _lists;
        public AdjacencyListGraph(int size)
        {
            _lists = new AdjacencyListArray(size);
        }

        public int Size()
        {
            return _lists.Length;
        }

        public IEnumerable<Edge> Neighbors(int i)
        {
            return _lists[i].Select(kvp => kvp.Value);
        }
        public IEnumerable<int> NeighborIndexes(int i)
        {
            return _lists[i].Select(kvp => kvp.Value.Right);
        }

        public void AddDirectedEdge(Edge edge)
        {
            _lists[edge.Left][edge.Right] = edge;
        }

        public override string ToString()
        {
            return _lists.ToString();
        }
    }

    public class AdjacencyList : Dictionary<int,Edge>
    {
        public bool IsSink { get { return !this.Any(); } }

        public override string ToString()
        {
            return string.Join(", ", this.Select(kvp => kvp.Value.ToString()));
        }
    }

    public class AdjacencyListArray
    {
        private readonly AdjacencyList[] _list;
        public AdjacencyListArray(int size)
        {
            _list = new AdjacencyList[size];
        }
        public AdjacencyList this[int i]
        {
            get
            {
                return _list[i] ?? (_list[i] = new AdjacencyList());
            }
        }

        public int Length { get { return _list.Length; } }

        public override string ToString()
        {
            var listItems = _list.Select((item, i) => string.Format("{0} => {1}", i, item == null ? "" : item.ToString()));
            return string.Join(" || ", listItems);
        }
    }
}
