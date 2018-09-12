using System;
using System.Collections.Generic;

namespace AlgorithmsOnGraphs
{
    public class GraphCycleException : Exception { }
    public class DepthFirstSearchWithCycleDetection
    {
        private readonly AdjacencyListGraph<long> _g;
        
        public DepthFirstSearchWithCycleDetection(AdjacencyListGraph<long> g)
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
            foreach (var w in _g.NeighborIndexes(v))
            {
                Explore(w, ancestory);
            }
            ancestory.Remove(v);
        }
    }
}