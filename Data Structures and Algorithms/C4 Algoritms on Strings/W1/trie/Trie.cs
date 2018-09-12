using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W1
{
    public class Trie
    {
        public static void Main(string[] args)
        {
            string s;
            var inputs = new List<string>();
            while ((s = Console.ReadLine()) != null)
                inputs.Add(s);

            foreach (var result in Answer(inputs.ToArray()))
                Console.WriteLine(result);
        }

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
        public class PrefixTree<T>
    {
        private readonly PrefixTreeNode<T> _root;
        private readonly IPrefixTreeContext<T> _context;
        private int _id = 0;
        public PrefixTree(IPrefixTreeContext<T> context)
        {
            _context = context;
            _root = new PrefixTreeNode<T>(_id++, _context.AlphabetSize);
        }

        public void Add(IEnumerable<T> values, Action<int,int,T> logAddEdge)
        {
            var node = _root;
            foreach (var value in values)
            {
                var valueIndex = _context.GetValueIndex(value);
                if (!node.HasChild(valueIndex))
                {
                    var childId = _id++;
                    node.SetChild(valueIndex, new PrefixTreeNode<T>(childId,_context.AlphabetSize));
                    logAddEdge(node.Id, childId, value);
                }
                 
                node = node.GetChild(valueIndex);
            }
            node.IsEndNode = true;
        }

        //public bool MatchExact(IEnumerable<T> values)
        //{
        //    var node = _root;
        //    foreach (var value in values)
        //    {
        //        var valueIndex = _context.GetValueIndex(value);
        //        if (!node.HasChild(valueIndex))
        //            return false;

        //        node = node.GetChild(valueIndex);
        //    }
        //    return node.IsEndNode;
        //}

        public class Builder
        {
            private int _lineCursor;
            private readonly IList<string> _inputs;
            public Builder(IList<string> inputs)
            {
                _inputs = inputs;
            }

            public PrefixTree<char> ToTrie(int numberOfAdds, IPrefixTreeContext<char> context)
            {
                //with no op logger
                return ToTrie(numberOfAdds, context, (x, y, t) => { });
            }

            public PrefixTree<char> ToTrie(int numberOfAdds, IPrefixTreeContext<char> context, Action<int, int, char> logAddEdge)
            {
                var result = new PrefixTree<char>(context);
                for (var i = 0; i < numberOfAdds; i++)
                {
                    result.Add(NextAsString(), logAddEdge);
                }
                return result;
            }

            public int NextAsInt()
            {
                return int.Parse(NextAsString());
            }
            public string NextAsString()
            {
                return _inputs[_lineCursor++];
            }
        }

    }

    public class PrefixTreeNode<T>
    {
        private static readonly PrefixTreeNode<T> Empty = new PrefixTreeNode<T>(-1,0);
        private readonly PrefixTreeNode<T>[] _children;
        public PrefixTreeNode(int id, int alphabetSize)
        {
            Id = id;
            _children = Enumerable.Range(0,alphabetSize).Select(_ => Empty).ToArray();
        }

        public bool HasChild(int index)
        {
            return _children[index] != Empty;
        }

        public PrefixTreeNode<T> GetChild(int index)
        {
            return _children[index];
        }

        public void SetChild(int index, PrefixTreeNode<T> child)
        {
            _children[index] = child;
        }
        public bool IsEndNode { get; set; }
        public int Id { get; private set; }
    }

    public interface IPrefixTreeContext<in T>
    {
        int AlphabetSize { get; }
        int GetValueIndex(T value);
    }
    public class NucleotidePrefixTreeContext : IPrefixTreeContext<char>
    {
        public int AlphabetSize { get { return 4; } }

        public int GetValueIndex(char value)
        {
            switch (value)
            {
                case 'A': case 'a': return 0;
                case 'T': case 't': return 1;
                case 'C': case 'c': return 2;
                case 'G': case 'g': return 3;
                default: throw new ArgumentException(string.Format("Unknown Nucleotide [{0}]",value));
            }
        }
    }
}
