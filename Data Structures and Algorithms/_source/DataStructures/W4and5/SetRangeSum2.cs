using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.W4and5
{
    public class SetRangeSum2
    {
        #region Given
        private void Update(Vertex v)
        {
            if (v == null) return;
            v.Sum = v.Key + (v.Left != null ? v.Left.Sum : 0) + (v.Right != null ? v.Right.Sum : 0);
            if (v.Left != null)
            {
                v.Left.Parent = v;
            }
            if (v.Right != null)
            {
                v.Right.Parent = v;
            }
        }

        private void SmallRotation(Vertex v)
        {
            var parent = v.Parent;
            if (parent == null)
            {
                return;
            }
            var grandparent = v.Parent.Parent;
            if (parent.Left == v)
            {
                var m = v.Right;
                v.Right = parent;
                parent.Left = m;
            }
            else {
                var m = v.Left;
                v.Left = parent;
                parent.Right = m;
            }
            Update(parent);
            Update(v);
            v.Parent = grandparent;
            if (grandparent != null)
            {
                if (grandparent.Left == parent)
                {
                    grandparent.Left = v;
                }
                else {
                    grandparent.Right = v;
                }
            }
        }

        private void BigRotation(Vertex v)
        {
            if (v.Parent.Left == v && v.Parent.Parent.Left == v.Parent)
            {
                // Zig-zig
                SmallRotation(v.Parent);
                SmallRotation(v);
            }
            else if (v.Parent.Right == v && v.Parent.Parent.Right == v.Parent)
            {
                // Zig-zig
                SmallRotation(v.Parent);
                SmallRotation(v);
            }
            else {
                // Zig-zag
                SmallRotation(v);
                SmallRotation(v);
            }
        }

        // Makes splay of the given vertex and returns the new root.
        private Vertex Splay(Vertex v)
        {
            if (v == null) return null;
            while (v.Parent != null)
            {
                if (v.Parent.Parent == null)
                {
                    SmallRotation(v);
                    break;
                }
                BigRotation(v);
            }
            return v;
        }

        // Searches for the given key in the tree with the given root
        // and calls splay for the deepest visited node after that.
        // Returns pair of the result and the new root.
        // If found, result is a pointer to the node with the given key.
        // Otherwise, result is a pointer to the node with the smallest
        // bigger key (next value in the order).
        // If the key is bigger than all keys in the tree,
        // then result is null.
        private VertexPair FindInRoot(Vertex root, int key)
        {
            Vertex v = root;
            Vertex last = root;
            Vertex next = null;
            while (v != null)
            {
                if (v.Key >= key && (next == null || v.Key < next.Key))
                {
                    next = v;
                }
                last = v;
                if (v.Key == key)
                {
                    break;
                }
                if (v.Key < key)
                {
                    v = v.Right;
                }
                else
                {
                    v = v.Left;
                }
            }
            root = Splay(last);
            return new VertexPair(next, root);
        }

        private VertexPair Split(Vertex root, int key)
        {
            VertexPair result = new VertexPair();
            VertexPair findAndRoot = FindInRoot(root, key);
            root = findAndRoot.Right;
            result.Right = findAndRoot.Left;
            if (result.Right == null)
            {
                result.Left = root;
                return result;
            }
            result.Right = Splay(result.Right);
            result.Left = result.Right.Left;
            result.Right.Left = null;
            if (result.Left != null)
            {
                result.Left.Parent = null;
            }
            Update(result.Left);
            Update(result.Right);
            return result;
        }

        private Vertex Merge(Vertex left, Vertex right)
        {
            if (left == null) return right;
            if (right == null) return left;
            while (right.Left != null)
            {
                right = right.Left;
            }
            right = Splay(right);
            right.Left = left;
            Update(right);
            return right;
        }
        #endregion
        
        public Vertex Root;
       
        public void Insert(int x)
        {
            var leftRight = Split(Root, x);
            var left = leftRight.Left;
            var right = leftRight.Right;
            Root = Merge(Merge(left,
                (right == null || right.Key != x)
                    ? new Vertex(x)
                    : null
                ), right);
        }

        public void Erase(int x)
        {
            var found = FindUpdateRoot(x);
            if (found == null || found.Key != x) return;

            //Delete
            var next = Next(found);
            Splay(next);
            Splay(found);

            if (found.Right == null)
            {
                var nl = found.Left;
                ReplaceChild(found.Parent, found, nl);
                Erase(found);

                Root = nl;
            }
            else
            {
                ReplaceChild(next.Parent, next, next.Right);
                ReplaceChild(found.Parent, found, next);
                next.Left = found.Left;
                next.Right = found.Right;
                Erase(found);

                Root = next;
            }
        }

        public bool Find(int x)
        {
            var found = FindUpdateRoot(x);
            return (found != null && found.Key == x);
        }

        public long Sum(int from, int to)
        {
            var leftMiddle = Split(Root, from);
            var left = leftMiddle.Left;
            var middle = leftMiddle.Right;
            var middleRight = Split(middle, to + 1);
            middle = middleRight.Left;
            var right = middleRight.Right;

            var result = middle != null ? middle.Sum : 0;

            Root = Merge(left, Merge(middle,right));
            return result;
        }
        
        private static SideOfParent SideOf(Vertex parent, Vertex child)
        {
            if (child == null || child.Parent == null)
                return SideOfParent.NO;

            return (parent.Right == child) ? SideOfParent.RIGHT
                : (parent.Left == child) ? SideOfParent.LEFT
                : SideOfParent.NO;
        }

        private static void ReplaceChild(Vertex parent, Vertex currentChild, Vertex newChild)
        {
            switch (SideOf(parent, currentChild))
            {
                case SideOfParent.LEFT:
                    if (newChild != null) newChild.Parent = parent;
                    parent.Left = newChild;
                    break;

                case SideOfParent.RIGHT:
                    if (newChild != null) newChild.Parent = parent;
                    parent.Right = newChild;
                    break;

                default:
                    if (newChild != null) newChild.Parent = null;
                    break;
            }
        }

        private Vertex FindUpdateRoot(int x)
        {
            if (Root == null) return null;

            var found = FindInRoot(Root, x);
            Root = found.Right;
            return found.Left;
        }

        private static Vertex Next(Vertex node)
        {
            return (node.Right != null) ? LeftDescendant(node.Right) : RightAncestor(node);
        }

        private static Vertex LeftDescendant(Vertex node)
        {
            //Recursion Replaced with iteration
            //return (node.Left == null) ? node : LeftDescendant(node.Left);
            while (true)
            {
                if ((node.Left == null)) return node;
                node = node.Left;
            }
        }

        private static Vertex RightAncestor(Vertex node)
        {
            //Recursion Replaced with iteration
            //return (node.Key < node.Parent.Key) ? node.Parent : RightAncestor(node.Parent);
            while (true)
            {
                if (node == null || node.Parent == null) return null;

                if ((node.Key < node.Parent.Key)) return node.Parent;
                node = node.Parent;
            }
        }

        private enum SideOfParent
        {
            NO = 0,
            LEFT = -1,
            RIGHT = 1,
        }

        private static void Erase(Vertex v)
        {
            if (v == null) return;
            v.Left = null;
            v.Right = null;
            v.Parent = null;
        }

        public override string ToString()
        {
            return new VertexPrinter(Root).Print();
        }

        #region Answer
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
            var n = int.Parse(inputs[0]);
            var queries = Enumerable.Range(1, n)
                .Select(i =>
                {
                    var items = inputs[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return new Input
                    {
                        Action = items[0],
                        Value = int.Parse(items[1]),
                        RangeValue = (items.Length == 3) ? int.Parse(items[2]) : 0
                    };
                });


            var outputLine = 0;
            var lastSum = 0;
            var o = new SetRangeSum2();
            var results = queries
                .Select((q, i) =>
                {
                    //*
                    Console.WriteLine("Current Sum: {0}", lastSum);
                    Console.WriteLine(o);

                    i += 2;  //for line number align in input file
                    if (q.Action == CommandSum)
                    {
                        outputLine++;
                        Console.WriteLine("[{5}:{6}] {0} [{1}:{2}] => [{3}:{4}]", q.Action, q.Value, q.RangeValue,
                            GetAdjustedValue(lastSum, q.Value), GetAdjustedValue(lastSum, q.RangeValue), i, outputLine);
                    }
                    else if (q.Action == CommandFind)
                    {
                        outputLine++;
                        Console.WriteLine("[{3}:{4}] {0} [{1}] => [{2}]", q.Action, q.Value, GetAdjustedValue(lastSum, q.Value),
                            i, outputLine);
                    }
                    else
                    {
                        Console.WriteLine("[{3}:-] {0} [{1}] => [{2}]", q.Action, q.Value, GetAdjustedValue(lastSum, q.Value),
                            i);
                    }
                    //*/

                    switch (q.Action)
                    {
                        case CommandAdd:
                            o.Insert(GetAdjustedValue(lastSum, q.Value));
                            return string.Empty;
                        case CommandDel:
                            o.Erase(GetAdjustedValue(lastSum, q.Value));
                            return string.Empty;
                        case CommandFind:
                            return o.Find(GetAdjustedValue(lastSum, q.Value)) ? "Found" : "Not found";
                        case CommandSum:
                            var res = o.Sum(GetAdjustedValue(lastSum, q.Value), GetAdjustedValue(lastSum, q.RangeValue));
                            lastSum = (int)(res % M);
                            return res.ToString();
                        default:
                            throw new ArgumentException("Action Unknown");
                    }
                })
                .Where(r => !string.IsNullOrEmpty(r));

            return results.ToArray();
        }

        private static int GetAdjustedValue(int lastSum, int value)
        {
            return (lastSum + value) % M;
        }
        #endregion 

        #region Inner Classes
        private const string CommandAdd = "+";
        private const string CommandDel = "-";
        private const string CommandFind = "?";
        private const string CommandSum = "s";
        private const int M = 1000000001;
        private class Input
        {
            public string Action { get; set; }
            public int Value { get; set; }
            public int RangeValue { get; set; }
        }
        public class Vertex
        {
            public int Key { get; private set; }
            // Sum of all the keys in the subtree - remember to update
            // it after each operation that changes the tree.
            public long Sum { get; set; }
            public Vertex Parent { get; set; }

            public Vertex(int key)
            {
                this.Key = key;
                this.Sum = key;
            }

            private Vertex _left;
            
            public Vertex Left
            {
                get { return _left; }
                set
                {
                    _left = value;
                    if (value != null)
                        value.Parent = this;
                }
            }
            private Vertex _right;
            public Vertex Right
            {
                get { return _right; }
                set
                {
                    _right = value;
                    if (value != null)
                        value.Parent = this;
                }
            }

            //public Vertex(int key, long sum, Vertex left, Vertex right, Vertex parent)
            //{
            //    this.Key = key;
            //    this.Sum = sum;
            //    this.Left = left;
            //    this.Right = right;
            //    this.Parent = parent;
            //}
            public override string ToString()
            {
                return string.Format("{0}:{1}", Key, Sum);
            }
        }
        private class VertexPair
        {
            public Vertex Left { get; set; }
            public Vertex Right { get; set; }
            public VertexPair() { }
            public VertexPair(Vertex left, Vertex right)
            {
                Left = left;
                Right = right;
            }
        }
        private class VertexPrinter
        {
            private readonly Vertex _original;
            private readonly StringBuilder _sb;

            public VertexPrinter(Vertex node)
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

            private void Print(Vertex node)
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

            private void PrintNodeValue(Vertex node)
            {
                _sb.AppendLine((node == null)
                    ? "<null>"
                    : (_original.Key == node.Key)
                        ? "[o]" + node.ToString()
                        : node.ToString()
                    );
            }

            // use string and not stringbuffer on purpose as we need to change the indent at each recursion
            private void Print(Vertex node, bool isRight, string indent)
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

            private static Vertex GetRoot(Vertex node)
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
