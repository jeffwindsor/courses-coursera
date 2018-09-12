using FluentAssertions;

namespace DataStructures.Tests
{
    public static class BinarySearchTreeNodeExtensions
    {
        public static void IsRoot(this BinarySearchTreeNode node)
        {
            node.Parent.Should().BeNull();
        }
        public static void IsLeftChildOf(this BinarySearchTreeNode child, BinarySearchTreeNode parent)
        {
            parent.Left.Should().Be(child);
            child.Parent.Should().Be(parent);
        }
        public static void IsRightChildOf(this BinarySearchTreeNode child, BinarySearchTreeNode parent)
        {
            parent.Right.Should().Be(child);
            child.Parent.Should().Be(parent);
        }
    }
}