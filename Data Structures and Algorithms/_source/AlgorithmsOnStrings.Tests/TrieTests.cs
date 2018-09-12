using System;
using AlgorithmsOnStrings.W1;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class TrieTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"Trie\" + file, Trie.Answer, false);
        }
    }
}
