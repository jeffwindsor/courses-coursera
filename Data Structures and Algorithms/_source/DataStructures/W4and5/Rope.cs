using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.W4and5
{
    public class Rope
    {
        internal static Node Move(int startIndex, int endIndex, int insertIndex, Node node)
        {
            node = Reduce(node);
            var removed = Remove(startIndex, endIndex, node);

            //Console.WriteLine("Move - Remove {0}-{1}", startIndex, endIndex);
            //Console.WriteLine(new NodePrinter(removed.Left).Print());
            //Console.WriteLine(new NodePrinter(removed.Right).Print());

            var added = Add(insertIndex, removed.Right, removed.Left);

            //Console.WriteLine("     - Add {0}", insertIndex);
            //Console.WriteLine(new NodePrinter(added).Print());

            return added; 
        }

        internal static Node Add(int index, Node addition, Node node)
        {
            var split = Split(node, index);
            //Console.WriteLine("Add");
            //Console.WriteLine(new NodePrinter(split.Left).Print());
            //Console.WriteLine(new NodePrinter(split.Right).Print());
            return Node.CreateConcatenation(Node.CreateConcatenation(split.Left, addition), split.Right);
        }

        //Removes Section and Returns it
        internal static NodePair Remove(int startIndex, int endIndex, Node node)
        {
            var s1 = Split(node, endIndex);
            var right = s1.Right;
            var s2 = Split(s1.Left, startIndex - 1);
            var middle = s2.Right;
            var left = s2.Left;
            //Console.WriteLine("Remove");
            //Console.WriteLine(new NodePrinter(left).Print());
            //Console.WriteLine(new NodePrinter(middle).Print());
            //Console.WriteLine(new NodePrinter(right).Print());
            return new NodePair(Node.CreateConcatenation(left, right), middle);
        }

        internal static NodePair Split(Node node, int i)
        {
            //Edge Cases
            if (i < 1) return new NodePair(null, Reduce(node));
            if (i > node.TotalWeight) return new NodePair(Reduce(node), null);

            //Find leaf at index
            var result = Search(node, i);
            var leaf = result.Node;
            var parent = leaf.Parent;

            //Establish Case's Appropriate Parent Processing Starting Place (before Split) 
            var side = SideOfParent(leaf);
            var cursor = (side == Side.LEFT)
                    ? leaf.Parent
                    : (side == Side.RIGHT && (SideOfParent(leaf.Parent) == Side.LEFT))
                        ? leaf.Parent.Parent
                        : null;

            //Split and establish accumulator 
            var split = SplitLeaf(leaf, result.StringIndex);
            if (parent == null)
            {
                return split; //no parent just split here
            }

            var acc = split.Right;
            Replace(leaf, split.Left);

            //Walk up Parents starting at case's initial location
            //var iteration = 1;
            while (cursor != null)
            {
                //Console.WriteLine(iteration++);
                //Console.WriteLine(new NodePrinter(node).Print());
                //Console.WriteLine(new NodePrinter(acc).Print());

                var right = cursor.Right;
                Replace(cursor.Right, null);
                if(cursor.Right == null) cursor.UpdateWeight();
                acc = (acc == null) ? right : Node.CreateConcatenation(acc, right);
                cursor = (SideOfParent(cursor) == Side.LEFT) ? cursor.Parent : null;
            }

            //Update Weight Along chain
            while (parent != null)
            {
                parent.UpdateWeight();
                parent = parent.Parent;
            }

            //Rebalance-Compress
            return new NodePair(Reduce(node), Reduce(acc));
        }

        internal static NodePair SplitLeaf(Node leaf, int stringIndex)
        {
            if (!leaf.IsLeaf)
                throw new ArgumentException("leaf");

            if (stringIndex < 1)
                return new NodePair(null,Node.CreateLeaf(leaf.SubString));

            if (stringIndex == leaf.SubString.Length)
                return new NodePair(Node.CreateLeaf(leaf.SubString), null);

            var i = stringIndex;
            return new NodePair(
                Node.CreateLeaf(leaf.SubString.Substring(0, i)),
                Node.CreateLeaf(leaf.SubString.Substring(i, leaf.SubString.Length - i))
                );
        }

        private static void Replace(Node current, Node replacement)
        {
            if (current == null) return;
            switch (SideOfParent(current))
            {
                case Side.LEFT:
                    current.Parent.Left = replacement;
                    break;
                case Side.RIGHT:
                    current.Parent.Right = replacement;
                    break;
                case Side.NO:
                    replacement.Parent = null;
                    break;
            }
            current.Parent = null;
        }

        internal static Node Reduce(Node node)
        {
            var cursor = node;
            while (cursor != null && !cursor.IsLeaf && cursor.IsOneSided)
            {
                cursor = cursor.Left ?? cursor.Right;
            }
            if(node != cursor) Replace(node, cursor);

            if (cursor != null)
            {
                Reduce(cursor.Left);
                Reduce(cursor.Right);
            }

            return cursor;
        }

        private static IndexSearch Search(Node node, int i)
        {
            return (node.SearchWeight < i)
                ? Search(node.Right, i - node.SearchWeight)
                : node.Left != null 
                    ? Search(node.Left, i) 
                    : new IndexSearch {Node = node, StringIndex = i};
        }

        private static Side SideOfParent(Node child)
        {
            if (child == null || child.Parent == null)
                return Side.NO;

            return (child.Parent.Right == child) ? Side.RIGHT
                : (child.Parent.Left == child) ? Side.LEFT
                : Side.NO;
        }

        internal static void InOrderTraversal(Node node, StringBuilder results)
        {
            if (node == null) return;
            InOrderTraversal(node.Left, results);
            results.Append(node.SubString);
            InOrderTraversal(node.Right, results);
        }
        
        #region Main
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
            var s = inputs[0];
            var n = int.Parse(inputs[1]);
            var xs = Enumerable.Range(2, n)
                .Select(i =>
                {
                    var items = inputs[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return new
                    {
                        StartIndex = int.Parse(items[0]) + 1,
                        EndIndex = int.Parse(items[1]) + 1,
                        InsertIndex = int.Parse(items[2])
                    };
                });

            var node = Node.CreateLeaf(s);
            foreach (var x in xs)
            {
                node = Move(x.StartIndex, x.EndIndex, x.InsertIndex, node);
            }
            return new [] { node.ToInOrderString()};
        }
        #endregion

        #region Nested Classes
        internal enum Side
        {
            NO = 0,
            LEFT = -1,
            RIGHT = 1,
        }
        internal class Node
        {
            public static Node CreateLeaf(string s)
            {
                var result = new Node
                {
                    SubString = s
                };
                result.UpdateWeight();
                return result;
            }

            public static Node CreateConcatenation(Node left, Node right)
            {
                var result = new Node();
                result.Left = left;
                result.Right = right;
                result.UpdateWeight();
                return result;
            }

            private Node() { }

            public int SearchWeight { get; private set; }

            public int TotalWeight { get; private set; }

            public void UpdateWeight()
            {
                TotalWeight = IsLeaf
                    ? SubString.Length
                    : (Left == null ? 0 : Left.TotalWeight) + (Right == null ? 0 : Right.TotalWeight);

                SearchWeight = IsLeaf
                    ? TotalWeight
                    : (Left == null ? 0 : Left.TotalWeight);
            }

            public string SubString { get; set; }

            public Node Parent { get; set; }
            private Node _left;
            public Node Left
            {
                get { return _left; }
                set
                {
                    _left = value;
                    UpdateWeight();

                    if (value != null) _left.Parent = this;
                }
            }

            private Node _right;
            public Node Right
            {
                get { return _right; }
                set
                {
                    _right = value;
                    UpdateWeight();

                    if (value != null) _right.Parent = this;
                }
            }

            public bool IsOneSided {get { return (Left == null && Right != null) || (Left != null && Right == null); }}
            public bool IsLeaf { get { return Left == null && Right == null && SubString != null; } }
            public bool IsRoot { get { return Parent == null; } }

            public override string ToString()
            {
                return string.Format("[{0}] {1}", SearchWeight, SubString == null ? "" : SubString);
            }

            public string ToInOrderString()
            {
                var sb = new StringBuilder();
                InOrderTraversal(this, sb);
                return sb.ToString();
            }

            public string ToTreeString()
            {
                return new NodePrinter(this).Print();
            }
        }

        internal class NodePair
        {
            public NodePair(Node left, Node right)
            {
                Left = left;
                Right = right;
            }

            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        internal class IndexSearch
        {
            public Node Node { get; set; }
            public int StringIndex { get; set; }
        }

        private class NodePrinter
        {
            private readonly Node _original;
            private readonly StringBuilder _sb;

            public NodePrinter(Node node)
            {
                _original = node;
                _sb = new StringBuilder();
            }

            public string Print()
            {
                if (_original == null)
                {
                    PrintNodeValue(_original);
                }
                else
                {
                    Print(GetRoot(_original));
                }
                return _sb.ToString();
            }

            private void Print(Node node)
            {

                if (node != null && node.Right != null)
                {
                    Print(node.Right, true, "");
                }
                PrintNodeValue(node);
                if (node != null && node.Left != null)
                {
                    Print(node.Left, false, "");
                }
            }

            private void PrintNodeValue(Node node)
            {
                _sb.AppendLine(node == null ? "<null>" : node.ToString());
            }

            // use string and not stringbuffer on purpose as we need to change the indent at each recursion
            private void Print(Node node, bool isRight, string indent)
            {
                if (node.Right != null)
                {
                    Print(node.Right, true, indent + (isRight ? "        " : " |      "));
                }
                _sb.Append(indent);
                _sb.Append(isRight ? " /" : " \\");
                _sb.Append("----- ");

                PrintNodeValue(node);
                if (node.Left != null)
                {
                    Print(node.Left, false, indent + (isRight ? " |      " : "        "));
                }
            }

            private static Node GetRoot(Node node)
            {
                if (node == null) return null;

                while (node.Parent != null)
                    node = node.Parent;
                return node;
            }
        }
    #endregion
    }
}
