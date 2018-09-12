using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs
{
    public class BreadthFirstSearchWithShortestPath
    {
        private readonly AdjacencyListGraph<long> _graph;
        private readonly SearchData<int> _distance;
        private readonly SearchData<int> _visitedFrom;

        public BreadthFirstSearchWithShortestPath(AdjacencyListGraph<long> g)
        {
            _graph = g;
            _visitedFrom = new SearchData<int>(g.Size(), -1);
            _distance = new SearchData<int>(g.Size(), -1);
        }
        
        public ICollection<int> ShortestPath(int from, int to)
        {
            //Search From to establish visited values
            Explore(from);
            //Return shortest path
            return GetShortestPath(from, to);
        }

        private void Explore(int start)
        {
            _distance.SetValue(start, 0);

            var queue = new Queue<int>();
            queue.Enqueue(start);
            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var neighbor in _graph.NeighborIndexes(current).Where(i=>i!=current))
                {
                    if (_distance.Visited(neighbor)) continue;

                    queue.Enqueue(neighbor);
                    _distance.SetValue(neighbor, _distance.GetValue(current) + 1);
                    _visitedFrom.SetValue(neighbor, current);
                }
            }
        }
        private List<int> GetShortestPath(int from, int to)
        {
            var result = new List<int>();
            while (to != from)
            {
                //No Path Found
                if(to == _visitedFrom.InitialValue)
                    return new List<int>();
                
                result.Add(to);
                to = _visitedFrom.GetValue(to);
            }
            result.Reverse();
            return result;
        }

    }
}
