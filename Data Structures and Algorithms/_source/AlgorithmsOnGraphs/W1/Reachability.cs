using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W1
{
    public class Reachability
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
            var graphInputs = Inputs.AdjacencyListGraphLong(inputs);
            var graph = graphInputs.ToUndirectedAdjacencyGraph();
            //Console.WriteLine(graph.ToPrettyString());

            var points = graphInputs.NextAsEdge();
            var dsf = new DepthFirstSearchWithComponents(graph);
            dsf.Explore(points.Left);
            //Console.WriteLine(dsf.ToPrettyString());

            var hasPath = dsf.Visited(points.Right) ? "1" : "0";
            return new[] { hasPath };
        }

        private static int GetIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }
    }
}
