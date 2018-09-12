using AlgorithmsOnStrings.W1;
using FluentAssertions;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class SharedStringTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        [TestCase("Sample4")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"sharedstring\" + file, SharedString.Answer, false);
        }
    }
}
