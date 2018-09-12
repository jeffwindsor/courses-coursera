using System;
using DataStructures.W4and5;
using FluentAssertions;
using NUnit.Framework;

namespace DataStructures.Tests
{
    [TestFixture]
    public class RopeTests
    {
        [Test]
        public void ReduceTest()
        {
            var n = Rope.Node.CreateConcatenation(
                null, Rope.Node.CreateConcatenation(
                    Rope.Node.CreateConcatenation(
                        Rope.Node.CreateLeaf("X")
                        , null),
                    null)
                );

            Console.WriteLine(n.ToTreeString());

            n = Rope.Reduce(n);
            Console.WriteLine(n.ToTreeString());
            Assert.IsTrue(n.IsLeaf);
        }


        [TestCase("abcdefgh", 1, 3, "defgh", "abc", TestName = "Remove From Front")]
        public void RemoveTestCases(string s, int startIndex, int endIndex, string expectedLeft, string expectedRight)
        {
            var node = Rope.Node.CreateLeaf(s);
            var result = Rope.Remove(startIndex, endIndex, node);
            AssertNodePair(expectedLeft, expectedRight, result);
        }
        
        [TestCase(7, 13, 0, "my_nameHello__is_Simon", TestName = "Wiki Move to Front")]
        [TestCase(7, 13, 15, "Hello__is_Simonmy_name", TestName = "Wiki Move to End")]
        [TestCase(7, 13, 8, "Hello__imy_names_Simon", TestName = "Wiki Move forward")]
        [TestCase(7, 13, 4, "Hellmy_nameo__is_Simon", TestName = "Wiki Move back")]
        public void MoveWikiExampleTestCases(int startIndex, int endIndex, int insertIndex, string expected)
        {
            var result = Rope.Move(startIndex, endIndex, insertIndex, WikiTree());
            AssertNode(expected, result);
        }

        [TestCase(1, 9, "name_is_Simon", "Hello_my_", TestName = "Wiki Remove Front, Left and Right Children, No Splits")]
        [TestCase(1, 7, "y_name_is_Simon", "Hello_m", TestName = "Wiki Remove Front, Left and Right Children, Split Right")]
        [TestCase(1, 6, "my_name_is_Simon", "Hello_", TestName = "Wiki Remove Front, Left Child, No Splits")]
        [TestCase(1, 3, "lo_my_name_is_Simon", "Hel", TestName = "Wiki Remove Front, Left Child, Split")]
        [TestCase(07, 11, "Hello_me_is_Simon", "my_na", TestName = "Wiki Remove Middle, Left and Right Children, No Splits")]
        [TestCase(08, 11, "Hello_mme_is_Simon", "y_na", TestName = "Wiki Remove Middle, Left and Right Children, Split Left")]
        [TestCase(07, 10, "Hello_ame_is_Simon", "my_n", TestName = "Wiki Remove Middle, Left and Right Children, Split Right")]
        [TestCase(07, 09, "Hello_name_is_Simon", "my_", TestName = "Wiki Remove Middle, Left Child, No Split")]
        [TestCase(10, 11, "Hello_my_me_is_Simon", "na", TestName = "Wiki Remove Middle, Right Child, No Split")]
        [TestCase(15, 22, "Hello_my_name_", "is_Simon", TestName = "Wiki Remove End, Left and Right Children, No Splits")]
        [TestCase(17, 22, "Hello_my_name_is", "_Simon", TestName = "Wiki Remove End, Right Child, No Split")]
        [TestCase(19, 22, "Hello_my_name_is_S", "imon", TestName = "Wiki Remove End, Right Child, Split")]
        public void RemoveWikiExampleTestCases(int startIndex, int endIndex, string expectedLeft, string expectedRight)
        {
            var result = Rope.Remove(startIndex, endIndex, WikiTree());
            AssertNodePair(expectedLeft, expectedRight, result);
        }


        [TestCase(11, "Hello_my_na", "me_is_Simon", TestName = "Wiki Split : End of Left")]
        [TestCase(10, "Hello_my_n", "ame_is_Simon", TestName = "Wiki Split : Middle of Left")]
        [TestCase(09, "Hello_my_", "name_is_Simon", TestName = "Wiki Split : End of Right")]
        [TestCase(12, "Hello_my_nam", "e_is_Simon", TestName = "Wiki Split : Middle of Right")]
        [TestCase(22, "Hello_my_name_is_Simon", "", TestName = "Wiki Split last Char")]
        [TestCase(100, "Hello_my_name_is_Simon", "", TestName = "Wiki Split right out of bounds Char")]
        [TestCase(-1, "", "Hello_my_name_is_Simon", TestName = "Wiki Split first Char")]
        [TestCase(-100, "", "Hello_my_name_is_Simon",  TestName = "Wiki Split left out of bounds Char")]
        public void SplitWikiExampleTestCases(int index, string expectedLeft, string expectedRight)
        {
            var result = Rope.Split(WikiTree(), index);
            AssertNodePair(expectedLeft, expectedRight, result);
        }

        [TestCase("111222", 3,"111","222", TestName = "Middle")]
        [TestCase("111222", 6, "111222", "", TestName = "End")]
        [TestCase("111222", 1, "1", "11222", TestName = "Front")]
        [TestCase("111222", 5, "11122", "2", TestName = "Five")]
        public void SplitLeafTestCases(string original, int splitIndex, string expectedLeft, string expectedRight)
        {
            var node = Rope.Node.CreateLeaf(original);
            var result = Rope.SplitLeaf(node, splitIndex);

            AssertLeafSubString(result.Left, expectedLeft);
            AssertLeafSubString(result.Right, expectedRight);
        }

        private static void AssertLeafSubString(Rope.Node node, string expectedRight)
        {
            if (node == null)
                expectedRight.Should().Be("");
            else
            {
                node.IsLeaf.Should().BeTrue();
                node.SubString.Length.Should().BeGreaterThan(0);
                node.SubString.Should().Be(expectedRight);
            }
        }

        private static void AssertNodePair(string expectedLeft, string expectedRight, Rope.NodePair result)
        {
            AssertNode(expectedLeft, result.Left);
            AssertNode(expectedRight, result.Right);
        }

        private static void AssertNode(string expected, Rope.Node result)
        {
            Console.WriteLine("{0} => {1}", expected, result == null ? "Null String" : result.ToInOrderString());
            Console.WriteLine(result == null ? "<null>" : result.ToTreeString());
            (result == null ? "" : result.ToInOrderString()).Should().Be(expected);

            if (result != null)
            {
                var expectedWeight = expected.Length;
                result.TotalWeight.Should().Be(expectedWeight);

                result.SearchWeight.Should().Be(result.Right == null ? expectedWeight : expectedWeight - result.Right.TotalWeight);
            }
        }

        private static Rope.Node WikiTree()
        {
            var results = 
                Rope.Node.CreateConcatenation( //22
                    Rope.Node.CreateConcatenation( //9
                        Rope.Node.CreateConcatenation( //6
                            Rope.Node.CreateLeaf("Hello_"), //6
                            Rope.Node.CreateLeaf("my_") //3
                            ),
                        Rope.Node.CreateConcatenation( //6
                            Rope.Node.CreateConcatenation( //2
                                Rope.Node.CreateLeaf("na"), //6
                                Rope.Node.CreateLeaf("me_i") //3
                                ),
                            Rope.Node.CreateConcatenation( //1
                                Rope.Node.CreateLeaf("s"), //1
                                Rope.Node.CreateLeaf("_Simon") //6
                                )
                            )
                        ),
                    null
                    );
            return results;
        }
    }
}


//var node = Rope.Node.CreateConcatenation(
//                Rope.Node.CreateLeaf("1"),
//                Rope.Node.CreateConcatenation(
//                    Rope.Node.CreateConcatenation(
//                        Rope.Node.CreateConcatenation(
//                            Rope.Node.CreateLeaf("2"),
//                            Rope.Node.CreateLeaf("3")
//                            ),
//                        Rope.Node.CreateLeaf("4")
//                        ),
//                    Rope.Node.CreateLeaf("5")
//                    )
//                );