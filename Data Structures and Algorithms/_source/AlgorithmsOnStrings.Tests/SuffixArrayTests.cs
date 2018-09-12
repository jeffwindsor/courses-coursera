using AlgorithmsOnStrings.W4;
using FluentAssertions;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class SuffixArrayTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        public void KmpSamples(string file)
        {
            TestFromRelativeFilePath(@"kmp\" + file, Kmp.Answer, false);
        }

        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        [TestCase("Sample4")]
        public void SuffixArrayLongSamples(string file)
        {
            TestFromRelativeFilePath(@"Suffix_Array_Long\" + file, SuffixArrayLong.Answer, false);
        }

        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        public void SuffixArrayMatchingSamples(string file)
        {
            TestFromRelativeFilePath(@"Suffix_Array_Matching\" + file, SuffixArrayMatching.Answer, false);
        }
        
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        [TestCase("Sample4")]
        public void SuffixTreeFromArraySamples(string file)
        {
            TestFromRelativeFilePath(@"Suffix_Tree_From_Array\" + file, SuffixTreeFromArray.Answer, false);
        }


        [Test]
        public void TestSortCharacters1()
        {
            const string input = "ababaa$";
            var expected = new[] { 6,5,4,2,0,3,1 };
            var actual = SuffixArray.SortCharacters(input, "$ab");
            actual.ShouldBeEquivalentTo(expected);
        }


        [Test]
        public void TestSortCharacters2()
        {
            const string input = "AAA$";
            var expected = new[] { 3,2,1,0 };
            var actual = SuffixArray.SortCharacters(input, SuffixArray.NucleotideAlphabet);
            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void TestComputePrefixFunction()
        {
            const string input = "abra$abracadabra";
            var expected = new[] {0,0,0,1,0,1,2,3,4,0,1,0,1,2,3,4};
            var actual = Kmp.ComputePrefixFunction(input);
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
