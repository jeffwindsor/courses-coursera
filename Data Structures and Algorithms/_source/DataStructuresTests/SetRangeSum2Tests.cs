using System;
using DataStructures.W4and5;
using FluentAssertions;
using NUnit.Framework;

namespace DataStructures.Tests
{
    [TestFixture]
    public class SetRangeSum2Tests
    {
        //INSERT TESTS

        private class Context
        {
            public SetRangeSum2.Vertex N40 = new SetRangeSum2.Vertex(40);
            public SetRangeSum2.Vertex N45 = new SetRangeSum2.Vertex(45);
            public SetRangeSum2.Vertex N30 = new SetRangeSum2.Vertex(30);
            public SetRangeSum2.Vertex N35 = new SetRangeSum2.Vertex(35);
            public SetRangeSum2.Vertex N20 = new SetRangeSum2.Vertex(20);
            public SetRangeSum2.Vertex N25 = new SetRangeSum2.Vertex(25);
            public SetRangeSum2.Vertex N10 = new SetRangeSum2.Vertex(10);
            public SetRangeSum2.Vertex N15 = new SetRangeSum2.Vertex(15);

            public SetRangeSum2 X = new SetRangeSum2();
        }

        //[Test]
        //public void SumOutOfRange_Test(int left, int right)
        //{
            
        //}

        [Test]
        public void Delete_AllFromTop_Test()
        {
            var c = new Context();
            c.N40.Left = c.N35;
            c.N35.Left = c.N30;
            c.N30.Left = c.N25;
            c.N25.Left = c.N20;
            c.N20.Left = c.N15;
            c.X.Root = c.N40;
            
            c.X.Erase(40);
            c.X.Erase(35);
            c.X.Erase(30);
            c.X.Erase(25);
            c.X.Erase(20);
            c.X.Erase(15);

            Assert.IsNull(c.X.Root);
        }

        [Test]
        public void Delete_AllFromBottom_Test()
        {
            var c = new Context();
            c.N40.Left = c.N35;
            c.N35.Left = c.N30;
            c.N30.Left = c.N25;
            c.N25.Left = c.N20;
            c.N20.Left = c.N15;
            c.X.Root = c.N40;

            c.X.Erase(15);
            c.X.Erase(20);
            c.X.Erase(25);
            c.X.Erase(30);
            c.X.Erase(35);
            c.X.Erase(40);

            Assert.IsNull(c.X.Root);
        }
        
        [Test]
        public void Find_Zig_Zag_Left_Test()
        {
            var c = new Context();
            c.N15.Left = c.N10;
            c.N15.Right = c.N40;
            c.N40.Left = c.N30;
            c.N40.Right = c.N45;
            c.N30.Left = c.N20;
            c.N30.Right = c.N35;

            c.X.Root = c.N15;
            c.X.Find(30).Should().BeTrue();
            c.X.Root.Should().Be(c.N30);

            c.N30.IsRoot();
            c.N15.IsLeftChildOf(c.N30);
            c.N40.IsRightChildOf(c.N30);
            c.N10.IsLeftChildOf(c.N15);
            c.N20.IsRightChildOf(c.N15);
            c.N35.IsLeftChildOf(c.N40);
            c.N45.IsRightChildOf(c.N40);
        }

        [Test]
        public void Find_Zig_Zag_Right_Test()
        {
            var c = new Context();
            c.N40.Left = c.N20;
            c.N40.Right = c.N45;
            c.N20.Left = c.N10;
            c.N20.Right = c.N30;
            c.N30.Left = c.N25;
            c.N30.Right = c.N35;

            c.X.Root = c.N40;
            c.X.Find(30).Should().BeTrue();
            c.X.Root.Should().Be(c.N30);
            
            c.N30.IsRoot();
            c.N20.IsLeftChildOf(c.N30);
            c.N40.IsRightChildOf(c.N30);
            c.N10.IsLeftChildOf(c.N20);
            c.N25.IsRightChildOf(c.N20);
            c.N35.IsLeftChildOf(c.N40);
            c.N45.IsRightChildOf(c.N40);
        }

        [Test]
        public void Find_Zig_Zig_Left_Test()
        {
            var c = new Context();
            c.N40.Left = c.N30;
            c.N40.Right = c.N45;
            c.N30.Left = c.N20;
            c.N30.Right = c.N35;
            c.N20.Left = c.N10;
            c.N20.Right = c.N15;

            c.X.Root = c.N40;
            c.X.Find(20).Should().BeTrue();
            c.X.Root.Should().Be(c.N20);

            c.N20.IsRoot();
            c.N10.IsLeftChildOf(c.N20);
            c.N30.IsRightChildOf(c.N20);
            c.N15.IsLeftChildOf(c.N30);
            c.N40.IsRightChildOf(c.N30);
            c.N35.IsLeftChildOf(c.N40);
            c.N45.IsRightChildOf(c.N40);
        }

        [Test]
        public void Find_Zig_Zig_Right_Test()
        {
            var c = new Context();
            c.N40.Left = c.N35;
            c.N40.Right = c.N45;
            c.N30.Left = c.N15;
            c.N30.Right = c.N40;
            c.N20.Left = c.N10;
            c.N20.Right = c.N30;

            c.X.Root = c.N20;
            c.X.Find(40).Should().BeTrue();
            c.X.Root.Should().Be(c.N40);

            c.N40.IsRoot();
            c.N45.IsRightChildOf(c.N40);
            c.N30.IsLeftChildOf(c.N40);
            c.N35.IsRightChildOf(c.N30);
            c.N20.IsLeftChildOf(c.N30);
            c.N10.IsLeftChildOf(c.N20);
            c.N15.IsRightChildOf(c.N20);
        }

        [Test]
        public void Find_Zig_Left_Test()
        {
            var c = new Context();
            c.N30.Left = c.N20;
            c.N30.Right = c.N35;
            c.N20.Left = c.N10;
            c.N20.Right = c.N15;

            c.X.Root = c.N30;
            c.X.Find(20).Should().BeTrue();
            c.X.Root.Should().Be(c.N20);
            
            c.N20.IsRoot();
            c.N10.IsLeftChildOf(c.N20);
            c.N30.IsRightChildOf(c.N20);
            c.N15.IsLeftChildOf(c.N30);
            c.N35.IsRightChildOf(c.N30);
        }

        [Test]
        public void Find_Zig_Right_Test()
        {
            var c = new Context();

            c.N30.Left = c.N15;
            c.N30.Right = c.N35;
            c.N20.Left = c.N10;
            c.N20.Right = c.N30;

            c.X.Root = c.N20;

            Console.WriteLine(c.X);
            c.X.Find(30).Should().BeTrue();
            Console.WriteLine(c.X);

            c.X.Root.Should().Be(c.N30);
            c.N30.IsRoot();
            c.N20.IsLeftChildOf(c.N30);
            c.N35.IsRightChildOf(c.N30);
            c.N10.IsLeftChildOf(c.N20);
            c.N15.IsRightChildOf(c.N20);
        }



        [Test]
        public void MultiAddDoesNotChangeSet_Test()
        {
            var x = new SetRangeSum2();
            x.Insert(1);
            x.Insert(1);
            x.Insert(1);
            x.Insert(1);

            x.Root.Left.Should().BeNull();
            x.Root.Right.Should().BeNull();
            x.Root.Key.Should().Be(1);
        }

        [TestCase(-100, false)]
        [TestCase(0, false)]
        [TestCase(1000000000, false)]
        [TestCase(21, false)]
        [TestCase(20, true)]
        public void Exists_Test(int id, bool expected)
        {
            var x = new SetRangeSum2();
            x.Insert(10);
            x.Insert(20);
            x.Insert(41);
            x.Insert(42);
            x.Insert(43);
            x.Insert(39);
            x.Insert(38);
            x.Insert(37);

            x.Find(id).Should().Be(expected);
        }

        [Test]
        public void SumOverflow_Test()
        {
            var x = new SetRangeSum2();
            x.Insert(1000000000);
            x.Insert(999999999);
            x.Insert(999999998);
            x.Insert(999999997);
            x.Insert(999999996);
            x.Insert(999999995);
            x.Insert(15);

            x.Sum(0, 1000000000).Should().Be(6000000000);
        }

        [Test]
        public void Sums_Test()
        {
            var x = new SetRangeSum2();
            x.Insert(10);
            x.Insert(20);
            x.Insert(30);
            x.Insert(40);
            x.Insert(50);
            x.Insert(60);

            x.Sum(9, 21).Should().Be(30);
            x.Sum(20, 31).Should().Be(50);
            x.Sum(19, 41).Should().Be(90);

        }


        [TestCase(1, 4,0)]
        [TestCase(50, 1000000000, 0)]
        [TestCase(-2, 4, 0)]
        [TestCase(11, 19, 0)]
        [TestCase(-100, 100, 270)]
        [TestCase(10, 43, 270)]
        public void SumOutOfRange_Test(int left, int right, long expected)
        {
            var x = new SetRangeSum2();

            x.Insert(10);
            x.Insert(20);

            x.Insert(41);
            x.Insert(42);
            x.Insert(43);

            x.Insert(39);
            x.Insert(38);
            x.Insert(37);

            x.Sum(left, right).Should().Be(expected);
        }

        [Test]
        public void Delete_FromEmpty_Test()
        {
            var x = new SetRangeSum2();
            x.Erase(40);
            x.Erase(35);
            x.Erase(30);
            x.Erase(25);
            x.Erase(20);
        }

    }
    public static class VertexTestExtensions
    {
        public static void IsRoot(this SetRangeSum2.Vertex node)
        {
            node.Parent.Should().BeNull();
        }
        public static void IsLeftChildOf(this SetRangeSum2.Vertex child, SetRangeSum2.Vertex parent)
        {
            parent.Left.Should().Be(child);
            child.Parent.Should().Be(parent);
        }
        public static void IsRightChildOf(this SetRangeSum2.Vertex child, SetRangeSum2.Vertex parent)
        {
            parent.Right.Should().Be(child);
            child.Parent.Should().Be(parent);
        }
    }
}
