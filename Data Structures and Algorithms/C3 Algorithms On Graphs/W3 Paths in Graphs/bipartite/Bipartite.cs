using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W3
{
    public class Bipartite
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
            var gi = new GraphInput(inputs);
            var graph = gi.ToUndirectedAdjacencyGraph();
            //Console.WriteLine(graph);
            
            var answer = new BreadthFirstSearchWithBipartiteDetection(graph).IsBipartite() ? "1" : "0";
            return new[] { answer };
        }
        
    }
    public class NonBipartiteException: Exception { }
    public class BreadthFirstSearchWithBipartiteDetection
    {
        private const int RED = 0;
        private const int BLUE = 1;
        private readonly ISearchableGraph _graph;
        private readonly SearchData _searchData;

        public BreadthFirstSearchWithBipartiteDetection(ISearchableGraph g)
        {
            _graph = g;
            _searchData = new SearchData(g.Size());
        }

        public bool IsBipartite()
        {
            try
            {
                Explore(0);
                return true;
            }
            catch (NonBipartiteException)
            {
                return false;
            }
        }
        private void Explore(int start)
        {
            _searchData.SetValue(start, RED);

            var queue = new Queue<int>();
            queue.Enqueue(start);
            while (queue.Any())
            {
                var current = queue.Dequeue();
                var neighborColor = GetNeighborColor(current);
                foreach (var neighbor in _graph.Neighbors(current).Where(i => i != current))
                {
                    if (_searchData.Visited(neighbor))
                    {
                        //Check Visited neighbor is the correct color
                        if(_searchData.GetValue(neighbor) != neighborColor)
                            throw new NonBipartiteException();

                        continue;
                    }

                    queue.Enqueue(neighbor);
                    _searchData.SetValue(neighbor, neighborColor);
                }
            }
        }

        private int GetNeighborColor(int index)
        {
            switch (_searchData.GetValue(index))
            {
                case RED:
                    return BLUE;
                case BLUE:
                    return RED;
                default:
                    throw new ArgumentException();
            }
        }
    }
    public class AdjacencyListGraph : ISearchableGraph
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

        public IEnumerable<int> Neighbors(int i)
        {
            return _lists[i];
        }
        
        public void AddDirectedEdge(int left, int right)
        {
            _lists[left].Add(right);
        }

        public override string ToString()
        {
            return _lists.ToString();
        }
    }

    public class AdjacencyList : HashSet<int>
    {
        public bool IsSink { get { return !this.Any(); } }
    }

    public class AdjacencyListArray
    {
        private readonly AdjacencyList[] _hashSets;
        public AdjacencyListArray(int size)
        {
            _hashSets = new AdjacencyList[size];
        }
        public AdjacencyList this[int i]
        {
            get
            {
                return _hashSets[i] ?? (_hashSets[i] = new AdjacencyList());
            }
        }

        public int Length { get { return _hashSets.Length; } }

        public override string ToString()
        {
            return string.Join(Environment.NewLine,
                _hashSets.Select(
                    (item, i) =>  string.Format("[{0}]: {1}", i,
                        item == null
                            ? ""
                            : string.Join(",", item.Select(e => e.ToString()))))
                );
        }
    }
    public class SearchData
    {
        public static int NOT_VISITED = -1;
        private readonly int[] _values;

        public SearchData(int size)
        {
            _values = Enumerable.Repeat(NOT_VISITED, size).ToArray();
        }
        
        public bool Visited(int v)
        {
            return _values[v] != NOT_VISITED;
        }

        public ICollection<int> Values { get { return _values; } }
        public virtual void SetValue(int v, int value)
        {
            _values[v] = value;
        }
        public virtual int GetValue(int v)
        {
            return _values[v];
        }
    }
    
    public interface ISearchableGraph
    {
        int Size();

        IEnumerable<int> Neighbors(int i);
    }
    public class GraphInput
    {
        private int _lineCursor;
        private readonly IList<string> _inputs;

        public GraphInput(IList<string> inputs)
        {
            _inputs = inputs;
        }

        public class Edge
        {
            public int Left { get; set; }
            public int Right { get; set; }
        }

        public ISearchableGraph ToUndirectedAdjacencyGraph()
        {
            return ToAdjacencyGraph((g, l, r) =>
            {
                g.AddDirectedEdge(l, r);
                g.AddDirectedEdge(r,l);
            });
        }
        public ISearchableGraph ToDirectedAdjacencyGraph()
        {
            return ToAdjacencyGraph((g, l, r) => g.AddDirectedEdge(l, r));
        }

        public ISearchableGraph ToDirectedReverseAdjacencyGraph()
        {
            return ToAdjacencyGraph((g, l, r) => g.AddDirectedEdge(r, l));
        }

        private ISearchableGraph ToAdjacencyGraph(Action<AdjacencyListGraph, int, int> addEdge)
        {
            var line0 = ParseIntPair(_inputs[0]);
            var verticeCount = line0.Item1;
            var edgeCount = line0.Item2;
            var graph = new AdjacencyListGraph(verticeCount);
            foreach (var x in Enumerable.Range(1, edgeCount).Select(i => ParseEdge(_inputs[i])))
            {
                addEdge(graph, x.Left, x.Right);
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
            return new Edge {Left=ParseIndex(linePath[0]), Right = ParseIndex(linePath[1])};
        }
        public static int ParseIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }
        private static string ParseSource(int index)
        {
            return (index + 1).ToString(); //input is 1 based return zero based
        }
    }
}
