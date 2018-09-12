using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsOnGraphs.W2
{
    public class Topsort
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

            var s = new TopologicalSort(graph);
            s.Search();
            var answer = string.Join(" ", s.Order.Select(GetSource));
            return new[] { answer };
        }
        private static int GetIndex(string source)
        {
            return int.Parse(source) - 1; //input is 1 based return zero based
        }
        private static string GetSource(int index)
        {
            return (index + 1).ToString(); //input is 1 based return zero based
        }
    }
}
