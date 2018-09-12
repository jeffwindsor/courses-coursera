using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOnGraphs.W1
{
    public class ConnectedComponents
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

            var graph = new GraphAdjacentyList(verticeCount);
            foreach (var x in xs)
            {
                graph.AddUndirectedEdge(x.Left, x.Right);
            }

            //Console.WriteLine(graph.ToPrettyString());

            var dsf = new DepthFirstSearch(graph);
            dsf.Search();

            //Console.WriteLine(dsf.ToPrettyString());

            return new[] { dsf.ConnectedComponents.ToString() };
        }

        private static int GetIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }


        #region Classes

        public interface IGraph
        {
            int Size();

            IEnumerable<int> Neighbors(int i);
        }

        public class AdjacentyList
        {
            private readonly HashSet<int>[] _hashSets;
            public AdjacentyList(int size)
            {
                _hashSets = new HashSet<int>[size];
            }
            public HashSet<int> this[int i]
            {
                get
                {
                    return _hashSets[i] ?? (_hashSets[i] = new HashSet<int>());
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

        public class GraphAdjacentyList : IGraph
        {
            private readonly AdjacentyList _adjlist;
            public GraphAdjacentyList(int size)
            {
                _adjlist = new AdjacentyList(size);
            }

            public int Size()
            {
                return _adjlist.Length;
            }

            public IEnumerable<int> Neighbors(int i)
            {
                return _adjlist[i];
            }

            public void AddDirectedEdge(int left, int right)
            {
                _adjlist[left].Add(right);
            }
            public void AddUndirectedEdge(int left, int right)
            {
                AddDirectedEdge(left, right);
                AddDirectedEdge(right, left);
            }

            public override string ToString()
            {
                return _adjlist.ToString();
            }
        }

        public class DepthFirstSearch
        {
            private readonly IGraph _graph;
            private readonly int[] _connectedComponent;

            public DepthFirstSearch(IGraph g)
            {
                _graph = g;
                _connectedComponent = new int[g.Size()];
                ConnectedComponents = 0;
            }

            public int ConnectedComponents { get; private set; }

            public bool Visited(int v)
            {
                return _connectedComponent[v] != 0;
            }

            public virtual void Search()
            {
                for (var i = 0; i < _connectedComponent.Length; i++)
                    Explore(i);
            }

            public void Explore(int v)
            {
                if (!Visited(v))
                    Explore(v, ++ConnectedComponents);
            }

            private void Explore(int v, int connectedComponent)
            {
                _connectedComponent[v] = connectedComponent;
                PreVisitHook(v);
                foreach (var w in _graph.Neighbors(v))
                {
                    if (Visited(w) == false)
                        Explore(w, connectedComponent);
                }
                PostVisitHook(v);
            }

            protected virtual void PreVisitHook(int v) { }
            protected virtual void PostVisitHook(int v) { }

            public override string ToString()
            {
                return string.Join(Environment.NewLine,
                    _connectedComponent.Select(
                        (item, i) => string.Format("[{0}]: {1}", i, item))
                    );
            }
        }
        #endregion
    }
}
