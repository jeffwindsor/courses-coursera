using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W1
{
    public class Suffix
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
            var builder = new SuffixTree.Builder(inputs);
            var suffixTree = builder.ToSuffixTree();

            var answers = suffixTree.ToText().ToArray();
            return answers;
        }
    }
    public class SuffixTree
    {
        private readonly IReadOnlyList<char> _text;
        private readonly Node _root;
        private readonly List<Node> _nodes;
        public SuffixTree(string text)
        {
            _text = text.ToCharArray();
            _root = new Node(0, 0, Enumerable.Empty<Node>());
            _nodes = new List<Node>();
        }
        
        public void Merge(int start, int length)
        {
            var node = new Node(start, length, Enumerable.Empty<Node>());
            foreach (var child in _root.Children)
            {
                if (Merge(child, node))
                    return;
            }
            _root.Children.Add(node);
            _nodes.Add(node);
        }

        private bool Merge(Node root, Node node)
        {
            var i = MatchIndex(root, node);
            //NO OVERLAP
            if (i == -1) return false;
            //OVERLAP
            Branch(root, i, node);

            return true;
        }

        private bool MergeFirst(IEnumerable<Node> nodes, Node node)
        {
            foreach (var root in nodes)
            {
                if (Merge(root, node)) return true;
            }
            return false;
        }

        private void Branch(Node root, int i, Node node)
        {
            var remainderRoot = CopyAfter(i, root);
            var remainderNode = CopyAfter(i, node);
            root.Length = i + 1;
            
            if (remainderRoot.IsEmpty())
            {
                //Merge down or add
                if (remainderNode.IsEmpty()) return;
                if(!MergeFirst(root.Children, remainderNode))
                    AddNode(root, remainderNode);
            }
            else
            {
                //Split and take children with the split
                root.Children.Clear();
                AddNode(root, remainderRoot);
                //Add remainder as child
                if (!remainderNode.IsEmpty())
                    AddNode(root, remainderNode);
            }
        }

        private void AddNode(Node root, Node node)
        {
            root.Children.Add(node);
            _nodes.Add(node);
        }

        private static Node CopyAfter(int i, Node source)
        {
            var l = i + 1;
            return new Node(source.Start + l, source.Length - l, source.Children);
        }
        
        private int MatchIndex(Node s1, Node s2)
        {
            int i;
            for (i = 0; i < Math.Min(s1.Length, s2.Length); i++)
            {
                if (!IsSameChar(s1.Start + i, s2.Start + i))
                    break;
            }
            return i - 1;
        }

        private bool IsSameChar(int i, int j)
        {
            return _text[i] == _text[j];
        }


        private IEnumerable<string> ToDebugText()
        {
            return _nodes.Select(ToDebugText);
        }

        private string ToDebugText(Node node)
        {
            return string.Format("{0} [{1}]", ToText(node),string.Join(", ", node.Children.Select(ToText)));
        }
        public IEnumerable<string> ToText()
        {
            return _nodes.Select(ToText);
        }
        private string ToText(Node node)
        {
            return new string(_text.Skip(node.Start).Take(node.Length).ToArray());
        }
        

        public class Builder
        {
            private int _lineCursor;
            private readonly IList<string> _inputs;
            public Builder(IList<string> inputs)
            {
                _inputs = inputs;
            }

            public SuffixTree ToSuffixTree()
            {
                return ToSuffixTree(NextAsString());
            }

            public static SuffixTree ToSuffixTree(string text)
            {
                var result = new SuffixTree(text);
                //Console.WriteLine(string.Join(", ", result.ToDebugText()));
                for (var i = 0; i < text.Length; i++)
                {
                    result.Merge(i, text.Length - i);
                    //Console.WriteLine(string.Join(", ", result.ToDebugText()));
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

        public class Node
        {
            public int Start { get; set; }
            public int Length { get; set; }
            public readonly List<Node> Children;

            public bool IsEmpty()
            {
                return Length <= 0;
            }

            public Node(int start, int length, IEnumerable<Node> children)
            {
                Start = start;
                Length = length;
                Children = children.ToList();
            }
            
        }
    }
}
