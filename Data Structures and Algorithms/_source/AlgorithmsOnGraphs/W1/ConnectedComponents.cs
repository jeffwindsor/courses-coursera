using System.Collections.Generic;

namespace AlgorithmsOnGraphs.W1
{
    public class ConnectedComponents
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
            var graph = Inputs.AdjacencyListGraphLong(inputs).ToUndirectedAdjacencyGraph();
            //Console.WriteLine(graph.ToPrettyString());
            
            var dsf = new DepthFirstSearchWithComponents(graph);
            dsf.Search();
            //Console.WriteLine(dsf.ToPrettyString());
            
            return new[] { dsf.MaxComponent.ToString() };
        }

        private static int GetIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }
    }
}
