using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W1
{
    public class TrieWithMatchExtended
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
            var builder = new PrefixTree<char>.Builder(inputs);
            var text = builder.NextAsString();
            var n = builder.NextAsInt();
            var trie = builder.ToTrie(n, new NucleotidePrefixTreeContext());

            var matcheIndexes = Enumerable.Range(0, text.Length)
                .Where(i =>
                {
                    var t = text.Substring(i);
                    var match = trie.Match(t);
                    Console.WriteLine("{0} {1} {2}", match ,i, t);
                    return match;
                })
                .Select(i => i.ToString());

            return new [] { string.Join(" ",matcheIndexes)};
        }
        
    }
}
