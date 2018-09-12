using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs
{
    public class DepthFirstSearchWithComponents
    {
        public int MaxComponent { get; private set; }
        private readonly AdjacencyListGraph<long> _graph;
        private readonly SearchData<int> _component;

        public DepthFirstSearchWithComponents(AdjacencyListGraph<long> g)
        {
            _graph = g;
            _component = new SearchData<int>(g.Size(), -1);
        }

        public bool Visited(int v)
        {
            return _component.Visited(v);
        }

        public virtual void Search()
        {
            for (var i = 0; i < _component.Values.Count; i++)
                Explore(i);
        }

        public void Explore(int v)
        {
            if (!_component.Visited(v))
                Explore(v, ++MaxComponent);
        }

        protected virtual void Explore(int v, int connectedComponent)
        {
            _component.SetValue(v, connectedComponent);
            foreach (var w in _graph.NeighborIndexes(v))
            {
                if (!_component.Visited(w))
                    Explore(w, connectedComponent);
            }
        }

        public IEnumerable<int> Components
        {
            get { return _component.Values.Distinct(); }
        }

        public override string ToString()
        {
            return string.Join(" ", _component.Values.Select((item, i) => string.Format("[{0}:{1}]", i, item)));
        }
    }
}