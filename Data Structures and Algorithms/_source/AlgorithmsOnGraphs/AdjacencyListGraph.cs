using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs
{
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

        public int NextAsInt()
        {
            return int.Parse(_inputs[_lineCursor++]);
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
}