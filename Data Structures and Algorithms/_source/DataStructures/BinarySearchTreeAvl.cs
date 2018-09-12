using System;
using System.Collections.Generic;

namespace DataStructures
{
    public class BinarySearchTreeAvl //<long> where long : IComparable<long>
    {
        public static void Insert(long key, BinarySearchTreeNode root)
        {
            BinarySearchTree.Insert(key, root);
            Rebalance(root);
        }

        public static void Delete(BinarySearchTreeNode node)
        {
            BinarySearchTree.Delete(node);
            Rebalance(node);
        }

        public static Tuple<BinarySearchTreeNode, BinarySearchTreeNode> Split(long key, BinarySearchTreeNode root)
        {
            if (key < root.Right.Key)
            {
                var s = Split(key, root.Left);
                var three = TreeMergeWithRoot(s.Item2, root.Right, root);
                return new Tuple<BinarySearchTreeNode, BinarySearchTreeNode>(s.Item1, three);
            }
            else
            {
                var s = Split(key, root.Right);
                var three = TreeMergeWithRoot(s.Item2, root.Left, root);
                return new Tuple<BinarySearchTreeNode, BinarySearchTreeNode>(s.Item1, three);
            }
        }

        private static BinarySearchTreeNode Merge(BinarySearchTreeNode with, BinarySearchTreeNode root)
        {
            var max = BinarySearchTree.Find(long.MaxValue, root);
            Delete(max);
            TreeMergeWithRoot(root, with, max);
            return max;
        }

        private static BinarySearchTreeNode TreeMergeWithRoot(BinarySearchTreeNode left, BinarySearchTreeNode right, BinarySearchTreeNode root)
        {
            if (Math.Abs(left.Rank - right.Rank) <= 1)
            {
                MergeAsRoot(left, right, root);
                AdjustHeight(root);
                //return this;
            }
            else if (left.Rank > right.Rank)
            {
                var rightPrime = TreeMergeWithRoot(left.Right, right, root);
                left.Right = rightPrime;
                rightPrime.Parent = left;
                Rebalance(left);
                //return this;
            }
            else if (left.Rank < right.Rank)
            {
                var leftPrime = TreeMergeWithRoot(left, right.Right, root);
                right.Right = leftPrime;
                leftPrime.Parent = right;
                Rebalance(right);
                //return this;
            }
            return root;
        }

        private static void MergeAsRoot(BinarySearchTreeNode left, BinarySearchTreeNode right, BinarySearchTreeNode root)
        {
            root.Left = left;
            root.Right = right;
            left.Parent = root;
            right.Parent = root;
        }

        private static void Rebalance(BinarySearchTreeNode node)
        {
            if (node.Left.Rank > node.Right.Rank + 1)
                RebalanceRight(node);
            if (node.Right.Rank > node.Left.Rank + 1)
                RebalanceLeft(node);

            AdjustHeight(node);

            if (node.Parent != null)
                Rebalance(node.Parent);
        }

        private static void RebalanceLeft(BinarySearchTreeNode node)
        {
            if (node.Left.Left.Rank > node.Left.Right.Rank)
                RotateRight(node.Left);

            RotateLeft(node);
        }

        private static void RebalanceRight(BinarySearchTreeNode node)
        {
            if (node.Left.Right.Rank > node.Left.Left.Rank)
                RotateLeft(node.Left);

            RotateRight(node);
        }

        private static void RotateRight(BinarySearchTreeNode node)
        {
            var currentParent = node.Parent;
            var currentGrandParent = currentParent.Parent;
            var currentRight = node.Right;

            //Rotate
            node.Parent = currentGrandParent;
            node.Right = currentParent;
            currentParent.Parent = node;
            currentParent.Left = currentRight;

            //Adjust Heights - Bottom to Top
            AdjustHeight(node.Right);
            AdjustHeight(node);
            AdjustHeight(node.Parent);
        }

        private static void RotateLeft(BinarySearchTreeNode node)
        {
            var currentParent = node.Parent;
            var currentGrandParent = currentParent.Parent;
            var currentLeft = node.Left;

            //Rotate
            node.Parent = currentGrandParent;
            node.Left = currentParent;
            currentParent.Parent = node;
            currentParent.Right = currentLeft;

            //Adjust Heights - Bottom to Top
            AdjustHeight(node.Left);
            AdjustHeight(node);
            AdjustHeight(node.Parent);
        }

        private static void AdjustHeight(BinarySearchTreeNode node)
        {
            node.Rank = 1 + Math.Max(node.Left.Rank, node.Right.Rank);
        }
    }
}
