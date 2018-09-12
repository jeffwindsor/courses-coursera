using AlgorithmsOnStrings.W2;
using FluentAssertions;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class BwtInvertTests : BaseTests
    {
        //[TestCase("Sample1")]
        //[TestCase("Sample2")]
        //[TestCase("Sample3")]
        //public void TestFiles(string file)
        //{
        //    TestFromRelativeFilePath(@"SuffixTree\" + file, Suffix.Answer, false);
        //}

        [TestCase("AC$A","ACA$")]
        [TestCase("AGGGAA$","GAGAGA$")]
        [TestCase("$", "$")]
        public void BurrowsWheelerInvertTransformTests(string input, string expected)
        {
            var actual = BwtInvert.Answer(new[] { input });
            actual.ShouldBeEquivalentTo(new [] { expected });
        }
    }
}
