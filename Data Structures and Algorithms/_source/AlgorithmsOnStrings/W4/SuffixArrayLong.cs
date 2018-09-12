using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W4
{
    public class SuffixArrayLong
    {
        //public static void Main(string[] args)
        //{
        //    string s;
        //    var inputs = new List<string>();
        //    while ((s = Console.ReadLine()) != null) inputs.Add(s);
        //    foreach (var result in Answer(inputs.ToArray())) Console.WriteLine(result);
        //}

        public static IList<string> Answer(IList<string> inputs)
        {
            var text = inputs[0];
            var orders = SuffixArray.BuildSuffixArray(text, SuffixArray.NucleotideAlphabet);

            //Spaced Values
            var answer = string.Join(" ", orders.Select(i => i.ToString()));
            return (string.IsNullOrEmpty(answer))
                ? Enumerable.Empty<string>().ToArray()
                : new[] { answer };
        }
    }
}