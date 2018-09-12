using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W4
{
    public class NegativeCycle
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
            var gis = Inputs.AdjacencyListGraphLong(inputs);
            var g = gis.ToEdges();
            
            var answer = HasNegativeCycle(g.Item1, g.Item2.ToList()) ? "1" : "0";
            return new[] { answer };
        }

        private static bool HasNegativeCycle(int size, List<Edge<long>> edges)
        {
            var result = BellmanFord(size, edges);
            return edges.Any(e => Relax(e.Left, e.Right, e.Weight, result));
        }

        private class BellmanFordResult
        {
            public BellmanFordResult(int size)
            {
                VisitedFrom = new SearchData<int>(size, -1);
                Distance = new SearchData<long>(size, 0);
            }
            public SearchData<long> Distance { get; private set; }
            public SearchData<int> VisitedFrom { get; private set; }
        }

        private static BellmanFordResult BellmanFord(int size, List<Edge<long>> edges)
        {
            var result = new BellmanFordResult(size);
            //Console.WriteLine("Initial: {0}",result.Distance);
            //IEnumerable<Edge> workingEdges = edges;
            for (var i = 0; i < size; i++)
            {
                foreach (var e in edges){ Relax(e.Left,e.Right,e.Weight,result); }
                //workingEdges = workingEdges.Where(e => Relax(e, result) e.Weight == 0);
                //Console.WriteLine("{1}: {0}", result.Distance,i);
            }
            return result;
        }
     
        private static bool Relax(int left, int right, long weight, BellmanFordResult r)
        {
            var leftDistance = r.Distance.GetValue(left);
            var relaxedDistance = leftDistance + weight;
            var currentDistance = r.Distance.GetValue(right);

            if (currentDistance <= relaxedDistance) return false;

            r.Distance.SetValue(right, relaxedDistance);
            r.VisitedFrom.SetValue(right, left);
            return true;
        }
    }
}
