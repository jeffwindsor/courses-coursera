using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W4
{
    public class NegativeCycle
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
            
            var answer = HasNegativeCycle(g.Item1, g.Item2.ToList()) ? "1" : "0";
            return new[] { answer };
        }

        private static bool HasNegativeCycle(int size, List<Edge> edges)
        {
            var result = BellmanFord(size, edges);
            return edges.Any(e => Relax(e, result));
        }

        private class BellmanFordResult
        {
            public BellmanFordResult(int size)
            {
                VisitedFrom = new SearchData<int>(size, -1);
                Distance = new SearchData<double>(size, 0);
            }
            public SearchData<double> Distance { get; private set; }
            public SearchData<int> VisitedFrom { get; private set; }
        }

        private static BellmanFordResult BellmanFord(int size, List<Edge> edges)
        {
            var result = new BellmanFordResult(size);
            //Console.WriteLine("Initial: {0}",result.Distance);
            IEnumerable<Edge> workingEdges = edges;
            for (var i = 0; i < size; i++)
            {
                foreach (var edge in edges){ Relax(edge,result); }
                //workingEdges = workingEdges.Where(e => Relax(e, result) e.Weight == 0);
                //Console.WriteLine("{1}: {0}", result.Distance,i);
            }
            return result;
        }
        
        private static bool Relax(Edge e, BellmanFordResult r)
        {
            return Relax(e.Left, e.Right, e.Weight, r);
        }

        private static bool Relax(int left, int right, double weight, BellmanFordResult r)
        {
            var leftDistance = r.Distance.GetValue(left);
            var relaxedDistance = leftDistance + weight;
            var currentDistance = r.Distance.GetValue(right);

            if (currentDistance <= relaxedDistance) return false;

            r.Distance.SetValue(right, relaxedDistance);
            r.VisitedFrom.SetValue(right, left);
            return true;
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

    public class AdjacencyListGraphInput
    {
        private int _lineCursor;
        private readonly IList<string> _inputs;

        public AdjacencyListGraphInput(IList<string> inputs)
        {
            _inputs = inputs;
            WeightParseFunction = x => x;  //No Op
        }

        public Func<double, double> WeightParseFunction { get; set; }

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
                Weight = (linePath.Length ==3)? WeightParseFunction(double.Parse(linePath[2])) : 0.0
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
        public double Weight { get; set; }

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
