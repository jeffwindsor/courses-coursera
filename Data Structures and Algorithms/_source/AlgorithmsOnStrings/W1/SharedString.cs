using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W1
{
    public class SharedString
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
            var builder = new SuffixTree.Builder(inputs);
            var one = builder.NextAsString();
            var two = builder.NextAsString();
            var text = string.Format("{0}#{1}$", one, two);
            var suffixTree = builder.ToSuffixTree(text);
            var shortest = Shortest(suffixTree);

            var answers = new [] { shortest };
            return answers;
        }

        private static string Shortest(SuffixTree tree)
        {
            var shortest = new Match (tree.Root, Enumerable.Empty<SuffixTree.Node>());

            var queue = new Queue<Match>();
            queue.Enqueue(shortest);
            while (queue.Any())
            {
                var current = queue.Dequeue();
                if (shortest.Node == tree.Root
                    || (tree.ToNodeText(current.Node).EndsWith("$") && current.Node.Length > 1 && current.Distance() <= shortest.Distance()))
                {
                    shortest = current;
                }
                var l = new List<SuffixTree.Node>(current.Lineage);
                l.Add(current.Node);

                foreach (var child in current.Node.Children)
                {
                    queue.Enqueue(new Match(child, l));
                }
            }
            var s = string.Format("{0}{1}", string.Join("", shortest.Lineage.Select(tree.ToText)), tree.ToText(shortest.Node)).Replace("$","");
            return s;
        }

        private class Match
        {
            public readonly SuffixTree.Node Node;
            public readonly List<SuffixTree.Node> Lineage;
            public int Distance() { return Node.Length + Lineage.Select(n => n.Length).Sum(); }
            public Match(SuffixTree.Node node, IEnumerable<SuffixTree.Node> lineage)
            {
                Lineage = new List<SuffixTree.Node>(lineage);
                Node = node;
            }
        }
    }
}
