using AlgorithmsOnGraphs.W3;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W3
{
    [TestFixture]
    public class BipartiteTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"Bipartite\" + file, Bipartite.Answer);
        }

    }
}
