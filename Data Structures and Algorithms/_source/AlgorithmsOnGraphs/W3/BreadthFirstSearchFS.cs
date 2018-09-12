using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W3
{
    public class BreadthFirstSearchFS
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
            var graph = gi.ToUndirectedAdjacencyGraph();
            //Console.WriteLine(graph);

            var points = gi.NextAsEdge();
            var path = new BreadthFirstSearchWithShortestPath(graph)
                .ShortestPath(points.Left, points.Right);
            
            var answer = path.Any() ? path.Count.ToString() : "-1";

            return new[] { answer };
        }
    }
}
