using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W2
{
    public class Acyclicity
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
            
            var answer = IsCyclic(graph) ? "1" : "0";

            return new[] { answer };
        }
        private static int GetIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }

        private static bool IsCyclic(ISearchableGraph g)
        {
            try
            {
                var dfe = new CyclicSearch(g);
                dfe.Search();
                return false;
            }
            catch (GraphCycleException)
            {
                return true;
            }
        }
    }
    public class CyclicSearch
    {
        private readonly ISearchableGraph _g;
        
        public CyclicSearch(ISearchableGraph g)
        {
            _g = g;
        }

        public virtual void Search()
        {
            for (var v = 0; v < _g.Size(); v++)
                Explore(v, new HashSet<int>());
        }
        
        protected virtual void Explore(int v, HashSet<int> ancestory)
        {
            if (ancestory.Contains(v))
                throw new GraphCycleException();

            ancestory.Add(v);
            foreach (var w in _g.Neighbors(v))
            {
                Explore(w, ancestory);
            }
            ancestory.Remove(v);
        }
    }

    public interface ISearchableGraph
    {
        int Size();

        IEnumerable<int> Neighbors(int i);
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
    public class GraphCycleException : Exception{}
}
