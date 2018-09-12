using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.W4and5
{
    public class TreeOrder
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

        //Use this for test input
        public static IList<string> Answer(IList<string> inputs)
        {
            var n = int.Parse(inputs[0]);
            var nodes = Enumerable.Range(1, n)
                .Select(i =>
                {
                    var items = inputs[i].Split(new[] {' '});
                    return new Node
                    {
                        Key = long.Parse(items[0]),
                        Left = int.Parse(items[1]),
                        Right = int.Parse(items[2])
                    };
                });

            var results = new List<string>();
            var o = new TreeOrder(nodes);
            results.Add(string.Join(" ", o.InOrder()));
            results.Add(string.Join(" ", o.PreOrder()));
            results.Add(string.Join(" ", o.PostOrder()));
            return results;
        }

        const int NullId = -1;
        private readonly Node[] _nodes;
        public TreeOrder(IEnumerable<Node> nodes){ _nodes = nodes.ToArray();}

        public IEnumerable<long> InOrder()
        {
            return Traverse(InOrderTraversal);
        }
        public IEnumerable<long> PreOrder()
        {
            return Traverse(PreOrderTraversal);
        }

        public IEnumerable<long> PostOrder()
        {
            return Traverse(PostOrderTraversal);
        }

        public IEnumerable<long> Traverse(Action<int, List<long>> traversalAction)
        {
            var results = new List<long>();
            traversalAction(0, results);
            return results;
        }
        private void InOrderTraversal(int index, List<long> results)
        {
            if (index == NullId) return;

            var node = _nodes[index];
            InOrderTraversal(node.Left, results);
            results.Add(node.Key);
            InOrderTraversal(node.Right, results);
        }
        private void PreOrderTraversal(int index, List<long> results)
        {
            if (index == NullId) return;

            var node = _nodes[index];
            results.Add(node.Key);
            PreOrderTraversal(node.Left, results);
            PreOrderTraversal(node.Right, results);
        }
        private void PostOrderTraversal(int index, List<long> results)
        {
            if (index == NullId) return;

            var node = _nodes[index];
            PostOrderTraversal(node.Left, results);
            PostOrderTraversal(node.Right, results);
            results.Add(node.Key);
        }

        public class Node
        {
            public long Key { get; set; }
            public int Left { get; set; }
            public int Right { get; set; }
        }
    }
}
