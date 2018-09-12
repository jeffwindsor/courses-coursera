using AlgorithmsOnStrings.W1;
using FluentAssertions;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class BwtTests: BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"SuffixTree\" + file, Suffix.Answer, false);
        }

        [TestCase("AA$","AA$")]
        [TestCase("ACACACAC$","CCCC$AAAA")]
        [TestCase("AGACATA$","ATG$CAAA")]
        public void BurrowsWheelerTransformTests(string input, string expected)
        {
            var actual = Bwt.Answer(new[] { input });
            actual.ShouldBeEquivalentTo(new [] { expected });
        }
    }
}
