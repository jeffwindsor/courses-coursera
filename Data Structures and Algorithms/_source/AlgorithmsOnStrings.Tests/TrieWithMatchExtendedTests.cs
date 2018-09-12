using System;
using AlgorithmsOnStrings.W1;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class TrieWithMatchExtendedTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"TrieWithMatchExtended\" + file, TrieWithMatchExtended.Answer);
        }
    }
}
