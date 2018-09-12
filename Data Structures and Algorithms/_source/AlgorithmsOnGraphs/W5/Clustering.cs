using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnGraphs.W5
{
    public class Clustering
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
            var gis = Inputs.AdjacencyListGraphDecimal(inputs);
            var pointsWithCount = gis.ToPoints();
            var k = gis.NextAsInt();

            var pointCount = pointsWithCount.Item1;
            var points = pointsWithCount.Item2.ToArray();
            var lines = PrimsAlgorithm.ConnectAllPoints(pointCount, points);

            var g = gis.ToUndirectedAdjacencyGraph(new Tuple<int, IEnumerable<Edge<decimal>>>(pointCount, lines));
            var primsResult = PrimsAlgorithm.Calculate(g);
            
            var d = primsResult.Cost.Values
                .OrderByDescending(i => i)
                .Take(k-1)
                .Last();


            var answer = d.ToString("0.0000000000");
            answer = answer.Remove(answer.Length - 1);  //Simulate truncate at 9
            return new[] { answer };

            //return new[] { answer.ToString("0.000000000") };
        }
        
    }
}
