using FluentAssertions;
using NUnit.Framework;

namespace DataStructures.Tests
{
    [TestFixture]
    public class SplayTreeTests
    {
        //PARENTS MAINTAINEWD 
        private class Context
        {
            public BinarySearchTreeNode N40 = new BinarySearchTreeNode { Key = 40 };
            public BinarySearchTreeNode N45 = new BinarySearchTreeNode { Key = 45 };
            public BinarySearchTreeNode N30 = new BinarySearchTreeNode { Key = 30 };
            public BinarySearchTreeNode N35 = new BinarySearchTreeNode { Key = 35 };
            public BinarySearchTreeNode N20 = new BinarySearchTreeNode { Key = 20 };
            public BinarySearchTreeNode N25 = new BinarySearchTreeNode { Key = 25 };
            public BinarySearchTreeNode N10 = new BinarySearchTreeNode { Key = 10 };
            public BinarySearchTreeNode N15 = new BinarySearchTreeNode { Key = 15 };
        }

        [Test]
        public void SumOutOfRanfe_Test(int left, int right)
        {
            BinarySearchTreeNode root = null;
            root = SplayTree.Delete(40, root);
            root = SplayTree.Delete(35, root);
            root = SplayTree.Delete(30, root);
            root = SplayTree.Delete(25, root);
            root = SplayTree.Delete(20, root);
            root = SplayTree.Delete(15, root);

            Assert.IsNull(root);
        }

        [Test]
        public void Delete_AllFromTop_Test()
        {
            var c = new Context();
            c.N40.Left = c.N35;
            c.N35.Left = c.N30;
            c.N30.Left = c.N25;
            c.N25.Left = c.N20;
            c.N20.Left = c.N15;

            var root = c.N40;
            root = SplayTree.Delete(40, root);
            root = SplayTree.Delete(35, root);
            root = SplayTree.Delete(30, root);
            root = SplayTree.Delete(25, root);
            root = SplayTree.Delete(20, root);
            root = SplayTree.Delete(15, root);

            Assert.IsNull(root);
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

            var root = c.N40;
            root = SplayTree.Delete(15, root);
            root = SplayTree.Delete(20, root);
            root = SplayTree.Delete(25, root);
            root = SplayTree.Delete(30, root);
            root = SplayTree.Delete(35, root);
            root = SplayTree.Delete(40, root);

            Assert.IsNull(root);
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

            var actual = SplayTree.Find(30, c.N15);
            actual.Should().Be(c.N30);

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

            var actual = SplayTree.Find(30, c.N40);
            actual.Should().Be(c.N30);

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

            var actual = SplayTree.Find(20, c.N40);
            actual.Should().Be(c.N20);

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

            var actual = SplayTree.Find(40, c.N20);
            actual.Should().Be(c.N40);

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

            var actual = SplayTree.Find(20, c.N30);
            actual.Should().Be(c.N20);

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

            var actual = SplayTree.Find(30, c.N20);
            actual.Should().Be(c.N30);

            c.N30.IsRoot();
            c.N20.IsLeftChildOf(c.N30);
            c.N35.IsRightChildOf(c.N30);
            c.N10.IsLeftChildOf(c.N20);
            c.N15.IsRightChildOf(c.N20);
            
        }
        
    }
}
