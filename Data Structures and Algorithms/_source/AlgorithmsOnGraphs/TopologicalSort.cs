using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs
{
    public class TopologicalSort : DepthFirstSearchWithComponents
    {
        private readonly List<int> _order = new List<int>();

        public TopologicalSort(AdjacencyListGraph<long> g) : base(g) {}
        
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
}
