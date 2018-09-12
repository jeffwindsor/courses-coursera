using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
     public class Program
    {
        public static void Main(string[] args)
        {
            Process(Tree.Process);
        }
        private static void Process(Func<string[],string[]> process)
        {
            var input = new List<string>();
            string s;
            while ((s = Console.ReadLine()) != null)
            {
                input.Add(s);
            }

            foreach (var item in process(input.ToArray()))
            {
                Console.WriteLine(item);
            }
        }
    }
    public class Tree
    {
        public int RootId { get; set; }
        public TreeNode[] Nodes { get; set; }

        public Tree(int size, IReadOnlyList<int> parents)
        {
            Nodes = Enumerable
                .Range(0, size)
                .Select(i => new TreeNode())
                .ToArray();

            for (var childId = 0; childId < size; childId++)
            {
                var parentId = parents[childId];
                if (parentId == -1) {RootId = childId;}
                else
                {
                    Nodes[childId].Parent = parentId;
                    Nodes[parentId].Children.Add(childId);
                }
            }
        }

        public static string[] Process(string[] inputs)
        {
            var n = int.Parse(inputs[0]);
            var parents = inputs[1].Split(new[] { ' ' }).Select(int.Parse).ToArray();
            return new[] { ComputeHeight(n, parents).ToString() };
        }

        public static int ComputeHeight(int size, int[] parents)
        {
            if (parents.Length == 0) return 0;

            var tree = new Tree(size, parents);
            var queue = new Queue<TreeNode>();
            var height = 0;
            queue.Enqueue(tree.Nodes[tree.RootId]);

            while (true)
            {
                var nodesOnLevel = queue.Count;
                if (nodesOnLevel > 0)
                    height += 1;
                else
                    return height;

                while (nodesOnLevel > 0)
                {
                    var parent = queue.Dequeue();
                    foreach (var childId in parent.Children)
                    {
                        queue.Enqueue(tree.Nodes[childId]);
                    }
                    nodesOnLevel -= 1;
                }
            }
        }
    }
    public class TreeNode
    {
        public TreeNode()
        {
            Parent = new int?();
            Depth = new int?();
            Children = new HashSet<int>();
        }

        public int? Parent { get; set; }
        public int? Depth { get; set; }
        public HashSet<int> Children { get; set; }
    }
}