using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings
{
    public class PrefixTree<T>
    {
        private readonly Node<T> _root;
        private readonly IPrefixTreeContext<T> _context;
        private int _id = 0;
        public PrefixTree(IPrefixTreeContext<T> context)
        {
            _context = context;
            _root = new Node<T>(_id++, _context.AlphabetSize);
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
                    node.SetChild(valueIndex, new Node<T>(childId,_context.AlphabetSize));
                    logAddEdge(node.Id, childId, value);
                }
                 
                node = node.GetChild(valueIndex);
            }
            node.IsEndNode = true;
        }
        
        public bool Match(IEnumerable<T> values)
        {
            var node = _root;
            foreach (var value in values)
            {
                var valueIndex = _context.GetValueIndex(value);
                if (!node.HasChild(valueIndex)) break;

                //Move to child
                node = node.GetChild(valueIndex);
                //if child is end node = match
                if (node.IsEndNode) return true;
            }
            return false;
        }

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

        private class Node<TValue>
        {
            private static readonly Node<TValue> Empty = new Node<TValue>(-1, 0);
            private readonly Node<TValue>[] _children;
            public Node(int id, int alphabetSize)
            {
                Id = id;
                _children = Enumerable.Range(0, alphabetSize).Select(_ => Empty).ToArray();
            }

            public bool HasChild(int index)
            {
                return _children[index] != Empty;
            }

            public Node<TValue> GetChild(int index)
            {
                return _children[index];
            }

            public void SetChild(int index, Node<TValue> child)
            {
                _children[index] = child;
            }
            public bool IsEndNode { get; set; }
            public int Id { get; private set; }
        }
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
