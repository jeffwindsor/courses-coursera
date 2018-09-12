using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W4
{
    public class ShortestPaths
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
            var gis = new AdjacencyListGraphInput(inputs);
            var g = gis.ToEdges();
            var start = gis.NextAsIndex();


            var path = ShortestPath(start, g.Item1, g.Item2.ToList());

            return path.Distance.Values
                .Select(v =>
                {
                    switch (v)
                    {
                        case NegativeInfinity:
                            return "-";
                        case PositiveInfinity:
                            return "*";
                        default:
                            return v.ToString();
                    }
                })
                .ToArray();
        }

        private const long PositiveInfinity = long.MaxValue;
        private const long NegativeInfinity = long.MinValue;
        
        private static BellmanFordResult ShortestPath(int start, int size, List<Edge> edges)
        {
            var g = new AdjacencyListGraph(size);
            foreach (var edge in edges)
            {
                g.AddDirectedEdge(edge);
            }

            var result = BellmanFord(start, size, edges);
            var negative = edges
                .Where(e => ShouldRelax(e.Left,e.Right,e.Weight, result).Item1)
                .Select(e => e.Right)
                .ToList();
            
            foreach (var i in BreathFirstSearch(negative, g))
            {
                result.Distance.SetValue(i,NegativeInfinity);
            }
            return result;
        }
        
        private static BellmanFordResult BellmanFord(int start, int size, List<Edge> edges)
        {
            var result = new BellmanFordResult(size);
            result.Distance.SetValue(start,0);

            //Console.WriteLine("Initial: {0}",result.Distance);
            //IEnumerable<Edge> workingEdges = edges;
            for (var i = 0; i < size; i++)
            {
                foreach (var e in edges)
                {
                    Relax(e.Left, e.Right, e.Weight, result);
                }
                //Console.WriteLine("{1}: {0}", result.Distance,i);
            }
            return result;
        }
        
        private static bool Relax(int left, int right, long weight, BellmanFordResult r)
        {
            var should = ShouldRelax(left, right, weight, r);

            if (should.Item1)
            {
                r.Distance.SetValue(right, should.Item2);
                r.VisitedFrom.SetValue(right, left);
            }
            return should.Item1;
        }
        
        private static Tuple<bool, long> ShouldRelax(int left, int right, long weight, BellmanFordResult r)
        {
            var leftDistance = r.Distance.GetValue(left);
            var relaxedDistance = leftDistance == PositiveInfinity ? leftDistance : leftDistance + weight;
            var currentDistance = r.Distance.GetValue(right);

            return new Tuple<bool, long>(currentDistance > relaxedDistance, relaxedDistance);
        }

        private class BellmanFordResult
        {
            public BellmanFordResult(int size)
            {
                VisitedFrom = new SearchData<int>(size, -1);
                Distance = new SearchData<long>(size, PositiveInfinity);
            }
            public SearchData<long> Distance { get; private set; }
            public SearchData<int> VisitedFrom { get; private set; }
        }
        
        public static IEnumerable<int> BreathFirstSearch(IEnumerable<int> starts, AdjacencyListGraph graph)
        {
            var visited = new SearchData<bool>(graph.Size(), false);
            var queue = new Queue<int>();
            foreach (var start in starts)
            {
                visited.SetValue(start, false);
                queue.Enqueue(start);
            }

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var neighbor in graph.NeighborIndexes(current).Where(i => i != current))
                {
                    if (visited.Visited(neighbor)) continue;

                    queue.Enqueue(neighbor);
                    visited.SetValue(neighbor, true);
                }
            }

            var visitedIndexes = visited.Values
                .Select((v, i) => new {Visited = v, Index = i})
                .Where(x => x.Visited)
                .Select(x => x.Index);
            return visitedIndexes;
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
        
        public Tuple<int,IEnumerable<Edge>> ToEdges()
        {
            var line0 = ParseIntPair(_inputs[0]);
            var verticeCount = line0.Item1;
            var edgeCount = line0.Item2;

            var edges = Enumerable.Range(1, edgeCount).Select(i => ParseEdge(_inputs[i]));
            _lineCursor = edgeCount + 1;

            return new Tuple<int, IEnumerable<Edge>>(verticeCount, edges);
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
        public Edge ParseEdge(string line)
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

        public static decimal NoOpDecimal(decimal source)
        {
            return source;
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
        public long Weight { get; set; }

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

    
}
