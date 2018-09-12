using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W2
{
    public class Acyclicity
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
            var graph = Inputs.AdjacencyListGraphLong(inputs).ToDirectedAdjacencyGraph();
            //Console.WriteLine(graph);
            
            var answer = IsCyclic(graph) ? "1" : "0";

            return new[] { answer };
        }
        private static bool IsCyclic(AdjacencyListGraph<long> g)
        {
            try
            {
                new DepthFirstSearchWithCycleDetection(g).Search();
                return false;
            }
            catch (GraphCycleException)
            {
                return true;
            }
        }
    }
}
