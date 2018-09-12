using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W4
{
    public class Kmp
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
            var pattern = inputs[0];
            var text = inputs[1];
            var matchIndexes = KnuthMorrisPrathOccuranceStartIndexes(pattern, text);
            var answer = string.Join(" ", matchIndexes.Select(i => i.ToString()));
            return (string.IsNullOrEmpty(answer))
                ? Enumerable.Empty<string>().ToArray()
                : new[] {answer};
        }

        public static IEnumerable<int> KnuthMorrisPrathOccuranceStartIndexes(string pattern, string text)
        {
            var pl = pattern.Length;
            var indexAdjustment = 1 - pl; 
            var ps = ComputePrefixFunction(string.Format("{0}${1}", pattern, text));

            var occurances = ps
                //skip the pattern and $
                .Skip(pl + 1)
                //Match is at length = prefix number
                .Select((n, i) => new {Index = i, Match = n == pl})
                //return indexes of matches
                .Where(a => a.Match)
                .Select(a => a.Index + indexAdjustment);
                
            return occurances;
        }

        public static IEnumerable<int> ComputePrefixFunction(string pattern)
        { 
            var length = pattern.Length;
            var s = Enumerable.Range(0, length).ToArray();
            var border = 0;

            for (var i = 1; i < length; i++)
            {
                while (border > 0 && pattern[i] != pattern[border])
                {
                    border = s[border - 1];
                }

                if (pattern[i] == pattern[border]){ border = border + 1; }
                else { border = 0; }

                s[i] = border;
            }
            return s;
        }
    }
}