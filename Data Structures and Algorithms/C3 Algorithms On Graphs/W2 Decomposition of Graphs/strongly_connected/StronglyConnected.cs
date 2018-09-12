using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W2
{
    public class StronglyConnected
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

            var g = new AdjacencyListGraph(verticeCount);
            var rg = new AdjacencyListGraph(verticeCount);
            foreach (var x in xs)
            {
                g.AddDirectedEdge(x.Left, x.Right);
                rg.AddDirectedEdge(x.Right,x.Left);
            }

            //Console.WriteLine(g);
            //Console.WriteLine(rg);

            var s = StronglyConnectedComponents(g,rg);
            
            var answer = s.Count();

            return new[] { answer.ToString() };
        }
        private static int GetIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }
        private static string GetSource(int index)
        {
            return (index + 1).ToString(); //input is 1 based return zero based
        }

        public static IEnumerable<int> StronglyConnectedComponents(ISearchableGraph graph, ISearchableGraph reverse)
        {
            //run dfs of reverse graph
            var srg = new TopologicalSort(reverse);
            srg.Search();

            //look for v in graph in reverse post order
            var sg = new DepthFirstSearch(graph);
            var order = srg.Order;
            foreach (var v in order)
            {
                //if not visited => explore and mark visted vertices as new component    
                sg.Explore(v);
            }
            return sg.Components;
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
                    (item, i) =>  string.Format("[{0}]: {1}", i,
                        item == null
                            ? ""
                            : string.Join(",", item.Select(e => e.ToString()))))
                );
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

        public IEnumerable<int> Components
        {
            get { return ConnectedComponent.Distinct(); }
        }

        public override string ToString()
        {
            return string.Join(" ",
                ConnectedComponent.Select(
                    (item, i) => string.Format("[{0}:{1}]", i, item))
                );
        }
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

    public interface ISearchableGraph
    {
        int Size();

        IEnumerable<int> Neighbors(int i);
    }

}
