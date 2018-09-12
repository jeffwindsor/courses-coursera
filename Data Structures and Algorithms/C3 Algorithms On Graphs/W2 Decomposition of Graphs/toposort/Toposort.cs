using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOnGraphs.W2
{
    public class Topsort
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
            var chars = new[] { ' ' };
            var line0 = inputs[0].Split(chars);
            var verticeCount = int.Parse(line0[0]);
            var edgeCount = int.Parse(line0[1]);

            var xs = Enumerable.Range(1, edgeCount)
                .Select(i =>
                {
                    var items = inputs[i].Trim().Split(chars, StringSplitOptions.RemoveEmptyEntries);
                    return new
                    {
                        Left = GetIndex(items[0]),
                        Right = GetIndex(items[1])
                    };
                });

            var graph = new AdjacencyListGraph(verticeCount);
            foreach (var x in xs)
            {
                graph.AddDirectedEdge(x.Left, x.Right);
            }

            //Console.WriteLine(graph);
            var s = new TopologicalSort(graph);
            s.Search();
            var answer = string.Join(" ", s.Order.Select(GetSource));
            
            return new[] { answer };
        }
        private static int GetIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }
        private static string GetSource(int index)
        {
            return (index + 1).ToString(); //input is 1 based return zero based
        }
    }
    
    public interface ISearchableGraph
    {
        int Size();

        IEnumerable<int> Neighbors(int i);
    }

    public class TopologicalSort : DepthFirstSearch
    {
        private readonly List<int> _order = new List<int>();

        public TopologicalSort(ISearchableGraph g) : base(g) {}
        
        protected override void Explore(int v, int connectedComponent)
        {
            base.Explore(v, connectedComponent);
            _order.Add(v);
        }

        public IEnumerable<int> Order
        {
            get { return ((IEnumerable<int>)_order).Reverse(); }
        }

        public override string ToString()
        {
            return string.Join(" ", Order.Select(i => i.ToString()));
        }
    }

    public class DepthFirstSearch
    {
        protected readonly ISearchableGraph Graph;
        protected int[] ConnectedComponent { get; set; }
        public int ConnectedComponents { get; protected set; }

        public DepthFirstSearch(ISearchableGraph g)
        {
            Graph = g;
            ConnectedComponent = new int[g.Size()];
            ConnectedComponents = 0;
        }

        public bool Visited(int v)
        {
            return ConnectedComponent[v] != 0;
        }

        public virtual void Search()
        {
            for (var i = 0; i < ConnectedComponent.Length; i++)
                Explore(i);
        }

        public void Explore(int v)
        {
            if (!Visited(v))
                Explore(v, ++ConnectedComponents);
        }

        protected virtual void Explore(int v, int connectedComponent)
        {
            ConnectedComponent[v] = connectedComponent;
            foreach (var w in Graph.Neighbors(v))
            {
                if (Visited(w) == false)
                    Explore(w, connectedComponent);
            }
        }
        
        public override string ToString()
        {
            return string.Join(Environment.NewLine,
                ConnectedComponent.Select(
                    (item, i) => string.Format("[{0}]: {1}", i, item))
                );
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

        public void AddEdge(int left, int right)
        {
            AddDirectedEdge(left, right);
            AddDirectedEdge(right, left);
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
                    (item, i) => item == null
                        ? ""
                        : string.Format("[{0}]: {1}", i, string.Join(",", item.Select(e => e.ToString()))))
                );
        }
    }
}
