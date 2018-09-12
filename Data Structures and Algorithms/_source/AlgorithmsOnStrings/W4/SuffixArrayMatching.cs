using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W4
{
    public class SuffixArrayMatching
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
            var patternCount = int.Parse(inputs[1]);
            var patterns = inputs[2].Trim().Split(' ');

            var sa = new SuffixArray(text, SuffixArray.NucleotideAlphabet);

            var answers = patterns.SelectMany(p => sa.Match(p));

            //Spaced Values
            var answer = string.Join(" ", answers.Select(i => i.ToString()));
            return (string.IsNullOrEmpty(answer))
                ? Enumerable.Empty<string>().ToArray()
                : new[] { answer };
        }
    }
}