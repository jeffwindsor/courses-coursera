using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W4
{
    public class Dijkstras
    {
        //public static void Main(string[] args)
        //{
        //    string s;
        //    var inputs = new List<string>();
        //    while ((s = Console.ReadLine()) != null)
        //        inputs.Add(s);

        //    foreach (var result in Answer(inputs.ToArray()))
        //        Console.WriteLine(result);
        //}

        public static IList<string> Answer(IList<string> inputs)
        {
            var gi = Inputs.AdjacencyListGraphLong(inputs);
            var graph = gi.ToDirectedAdjacencyGraph();
            var points = gi.NextAsEdge();
            //Console.WriteLine(graph);

            var s = new Dijkstras(graph);
            var cost = s.LowestCostPath(points.Left,points.Right);
            
            return new[] { cost.ToString() };
        }

        private readonly AdjacencyListGraph<long> _graph;
        private SearchData<int> _distance;
        private SearchData<int> _visitedFrom;
        private const int MaxDistance = int.MaxValue;

        public Dijkstras(AdjacencyListGraph<long> g)
        {
            _graph = g;
        }

        public int LowestCostPath(int from, int to)
        {
            //Search From to establish visited values
            Explore(from);
            //Return lowest cost for path
            var d = _distance.GetValue(to);
            return (d == MaxDistance) ? -1 : d;
        }

        private void Explore(int start)
        {
            _visitedFrom = new SearchData<int>(_graph.Size(),-1);
            _distance = new SearchData<int>(_graph.Size(), MaxDistance);
            _distance.SetValue(start, 0);

            //Make Prioirty Queue
            var pq = new MinPriorityQueue<long>(_graph.Size(), long.MaxValue);
            for (var i = 0; i < _distance.Length; i++)
            {
                pq.Enqueue(i, _distance.GetValue(i));
            }

            while (!pq.IsEmpty())
            {
                //Console.WriteLine("Queue: {0}", pq);

                var currentIndex = pq.Dequeue();

                //Console.WriteLine("Extract: {0}", currentIndex);

                foreach (var edge in _graph.Neighbors(currentIndex))
                {
                    var neighborIndex = edge.Right;
                    var d = _distance.GetValue(neighborIndex);
                    var dFromC = _distance.GetValue(currentIndex) == MaxDistance
                        ? MaxDistance
                        : _distance.GetValue(currentIndex) + (int)edge.Weight;

                    //Console.WriteLine("Edge {1} => {0} : Distance {2} : {3}",neighborIndex,currentIndex,dFromC,d);

                    if (d <= dFromC) continue;

                    //Set New Distance Values
                    _distance.SetValue(neighborIndex, dFromC);
                    _visitedFrom.SetValue(neighborIndex, currentIndex);
                    pq.ChangePriority(neighborIndex, dFromC);
                }
            }
        }
    }
}
