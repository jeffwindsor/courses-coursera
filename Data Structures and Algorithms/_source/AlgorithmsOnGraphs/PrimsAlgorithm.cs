using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs
{
    public class PrimsAlgorithm
    {
        public static IEnumerable<Edge<decimal>> ConnectAllPoints(int pointCount, Point[] points)
        {
            //make edge for all points to all other points except self
            //  with weight = SQRT( SQR(x1 - x2) + SQR(y1 - y2))
            //  left = index of point
            //  right = index of to point
            var lines =
                from x in Enumerable.Range(0, pointCount)
                from y in Enumerable.Range(0, pointCount)
                where x != y
                let xp = points[x]
                let yp = points[y]
                select new Edge<decimal> { Left = x, Right = y, Weight = GetDistance(xp, yp) };
            return lines;
        }

        private static decimal GetDistance(Point a, Point b)
        {
            var value = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            return Convert.ToDecimal(value);
        }

        public static PrimsResult Calculate(AdjacencyListGraph<decimal> graph)
        {
            var size = graph.Size();

            //Initialize result with 0 as first vertex
            var result = new PrimsResult(size);
            result.Cost.SetValue(0, 0);

            //priority queue of costs
            var q = new MinPriorityQueue<decimal>(size, decimal.MaxValue);
            for (var i = 0; i < size; i++)
            {
                q.Enqueue(i, result.Cost.GetValue(i));
            }

            //Walk
            while (!q.IsEmpty())
            {
                //Console.WriteLine("Queue: {0}", q);
                var currentIndex = q.Dequeue();

                //Console.WriteLine("Extract: {0}", currentIndex);
                foreach (var edge in graph.Neighbors(currentIndex))
                {
                    var z = edge.Right;
                    var w = edge.Weight;
                    if (!q.Contains(z) || result.Cost.GetValue(z) <= w) continue;

                    result.Cost.SetValue(z, w);
                    result.Parent.SetValue(z, currentIndex);
                    q.ChangePriority(z, w);
                }
            }
            return result;
        }
        
        public class PrimsResult
        {
            public PrimsResult(int size)
            {
                Parent = new SearchData<int>(size, -1);
                Cost = new SearchData<decimal>(size, decimal.MaxValue);
            }
            public SearchData<decimal> Cost { get; private set; }
            public SearchData<int> Parent { get; private set; }
        }
    }
}
