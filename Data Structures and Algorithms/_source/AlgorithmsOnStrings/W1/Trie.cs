using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W1
{
    public class Trie
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
            var addList = new List<Tuple<int,int,char>>();
            Action<int,int,char> addLogger = (x, y, c) => addList.Add(new Tuple<int, int, char>(x, y, c));

            var builder = new PrefixTree<char>.Builder(inputs);
            var n = builder.NextAsInt();
            builder.ToTrie(n, new NucleotidePrefixTreeContext(), addLogger);

            var answers = addList.Select(t => string.Format("{0}->{1}:{2}", t.Item1, t.Item2, t.Item3)).ToArray();
            return answers;
        }
        
    }
}
