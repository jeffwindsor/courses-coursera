using System;

namespace DataStructures
{
    public class SplayTree
    {
        public static BinarySearchTreeNode Find(long key, BinarySearchTreeNode root)
        {
            var found = BinarySearchTree.Find(key, root);
            Splay(found);
            return found;
        }

        public static BinarySearchTreeNode Insert(long key, BinarySearchTreeNode root)
        {
            BinarySearchTree.Insert(key, root);
            return Find(key, root);
        }

        public static BinarySearchTreeNode Delete(long key, BinarySearchTreeNode root)
        {
            var node = Find(key, root);
            return (node.Key == key) ? Delete(node) : node;
        }

        private static BinarySearchTreeNode Delete(BinarySearchTreeNode node)
        {
            Splay(BinarySearchTree.Next(node));
            Splay(node);
            return BinarySearchTree.Delete(node);
        }

        public static long Sum(long leftKey, long rightKey, BinarySearchTreeNode root)
        {
            long results = 0;
            var node = Find(leftKey, root);
            while (node != null && node.Key <= rightKey)
            {
                if (node.Key >= leftKey)
                    results += node.Key;

                node = BinarySearchTree.Next(node);
            }
            return results;
        }

        public static void Splay(BinarySearchTreeNode node)
        {
            //Recursion Replaced with iteration
            //      if (node.Parent != null)
            //          Splay(node);
            //
            while (true)
            {
                if (node == null || node.Parent == null) return; //bad

                var nodesSideOfParent = BinarySearchTreeNode.SideOf(node.Parent, node);
                if (node.Parent.Parent == null)
                {
                    Zig(node, nodesSideOfParent);
                }
                else if (nodesSideOfParent == BinarySearchTreeNode.SideOf(node.Parent.Parent, node.Parent))
                {
                    ZigZig(node, nodesSideOfParent);
                }
                else
                {
                    ZigZag(node, nodesSideOfParent);
                }

                if (node.Parent != null)
                    continue;
                break;
            }
        }

        private static void Zig(BinarySearchTreeNode node, BinarySearchTreeNode.SideOfParent side)
        {
            var p = node.Parent;
            BinarySearchTreeNode.ReplaceChild(p.Parent, p, node);
            
            if (side == BinarySearchTreeNode.SideOfParent.LEFT)
            {
                var nr = node.Right;
                node.Right = p;
                p.Left = nr;
            }
            else
            {
                var nl = node.Left;
                node.Left = p;
                p.Right = nl;
            }
        }

        private static void ZigZig(BinarySearchTreeNode node, BinarySearchTreeNode.SideOfParent side)
        {
            var p = node.Parent;
            var q = node.Parent.Parent;

            BinarySearchTreeNode.ReplaceChild(q.Parent, q, node);
            if (side == BinarySearchTreeNode.SideOfParent.LEFT)
            {
                var nr = node.Right;
                var pr = p.Right;
                node.Right = p;
                p.Right = q;
                p.Left = nr;
                q.Left = pr;
            }
            else
            {
                var nl = node.Left;
                var pl = p.Left;
                node.Left = p;
                p.Left = q;
                p.Right = nl;
                q.Right = pl;
            }
        }

        private static void ZigZag(BinarySearchTreeNode node, BinarySearchTreeNode.SideOfParent side)
        {
            var p = node.Parent;
            var q = node.Parent.Parent;

            BinarySearchTreeNode.ReplaceChild(q.Parent, q, node);
            p.Parent = node;
            q.Parent = node;

            var nl = node.Left;
            var nr = node.Right;
            if (side == BinarySearchTreeNode.SideOfParent.LEFT)
            {
                node.Left = q;
                node.Right = p;
                p.Left = nr;
                q.Right= nl;
            }
            else
            {
                node.Left = p;
                node.Right = q;
                p.Right = nl;
                q.Left = nr;
            }
        }
        
    }
}
