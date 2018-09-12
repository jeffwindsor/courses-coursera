//using DataStructures.W4and5;
//using FluentAssertions;
//using NUnit.Framework;

//namespace DataStructures.Tests
//{
//    [TestFixture]
//    public class SetRangeSumTests
//    {

//        [Test]
//        public void MultiAddDoesNotChangeSet_Test()
//        {
//            var x = new SetRangeSum();
//            x.Insert(1);
//            x.Insert(1);
//            x.Insert(1);
//            x.Insert(1);

//            x.Tree.Left.Should().BeNull();
//            x.Tree.Right.Should().BeNull();
//            x.Tree.Key.Should().Be(1);
//        }

//        [TestCase(-100, false)]
//        [TestCase(0, false)]
//        [TestCase(1000000000, false)]
//        [TestCase(21, false)]
//        [TestCase(20, true)]
//        public void Exists_Test(long id, bool expected)
//        {
//            var x = new SetRangeSum();
//            x.Insert(10);
//            x.Insert(20);
//            x.Insert(41);
//            x.Insert(42);
//            x.Insert(43);
//            x.Insert(39);
//            x.Insert(38);
//            x.Insert(37);

//            x.Exists(id).Should().Be(expected);
//        }

//        [Test]
//        public void SumOverflow_Test()
//        {
//            var x = new SetRangeSum();
//            x.Insert(1000000000);
//            x.Insert(999999999);
//            x.Insert(999999998);
//            x.Insert(999999997);
//            x.Insert(999999996);
//            x.Insert(999999995);
//            x.Insert(15);

//            x.Sum(0, 1000000000).Should().Be(6000000000);
//        }

//        [Test]
//        public void Sums_Test()
//        {
//            var x = new SetRangeSum();
//            x.Insert(10);
//            x.Insert(20);
//            x.Insert(30);
//            x.Insert(40);
//            x.Insert(50);
//            x.Insert(60);

//            x.Sum(9, 21).Should().Be(30);
//            x.Sum(20, 31).Should().Be(50);
//            x.Sum(19, 41).Should().Be(90);

//        }


//        [TestCase(1, 4,0)]
//        [TestCase(50, 1000000000, 0)]
//        [TestCase(-2, 4, 0)]
//        [TestCase(11, 19, 0)]
//        [TestCase(-100, 100, 270)]
//        [TestCase(10, 43, 270)]
//        public void SumOutOfRange_Test(long left, long right, long expected)
//        {
//            var x = new SetRangeSum();

//            x.Insert(10);
//            x.Insert(20);

//            x.Insert(41);
//            x.Insert(42);
//            x.Insert(43);

//            x.Insert(39);
//            x.Insert(38);
//            x.Insert(37);

//            x.Sum(left, right).Should().Be(expected);
//        }

//        [Test]
//        public void Delete_FromEmpty_Test()
//        {
//            var x = new SetRangeSum();
//            x.Delete(40);
//            x.Delete(35);
//            x.Delete(30);
//            x.Delete(25);
//            x.Delete(20);
//        }

//    }    
//}
