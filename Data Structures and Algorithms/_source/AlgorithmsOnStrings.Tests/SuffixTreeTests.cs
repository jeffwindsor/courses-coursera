using AlgorithmsOnStrings.W1;
using FluentAssertions;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class SuffixTreeTests: BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        [TestCase("Sample4")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"SuffixTree\" + file, Suffix.Answer, false);
        }

        [Test]
        public void MergeFullOverlap()
        {
            var st = new SuffixTree("abab$");
            st.Merge(0, 2);
            st.Merge(2, 2);
            var actual = st.ToNodeText();
            
            actual.ShouldBeEquivalentTo(new [] { "ab"});
        }

        [TestCase(0, 2, 2, 4, TestName = "MergePartialOverlapOneSide Left")]
        [TestCase(2, 4, 0, 2, TestName = "MergePartialOverlapOneSide Right")]
        public void MergePartialOverlapOneSide(int rs, int rl, int ls, int ll)
        {
            var st = new SuffixTree("ababcd$");
            st.Merge(ls, ll);
            st.Merge(rs, rl);
            var actual = st.ToNodeText();
            actual.ShouldBeEquivalentTo(new[] { "ab", "cd"});
        }

        [TestCase(0, 3, 3, 4, TestName = "MergePartialOverlapBothSides Left")]
        [TestCase(3, 4, 0, 3, TestName = "MergePartialOverlapBothSides Right")]
        public void MergePartialOverlapBothSides(int rs, int rl, int ls, int ll)
        {
            var st = new SuffixTree("abzabcd$");
            st.Merge(ls, ll);
            st.Merge(rs, rl);
            var actual = st.ToNodeText();
            actual.ShouldBeEquivalentTo(new[] { "ab", "z", "cd" });
        }
    }
}
