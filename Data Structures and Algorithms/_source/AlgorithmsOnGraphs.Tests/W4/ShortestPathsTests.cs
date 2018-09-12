using AlgorithmsOnGraphs.W4;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W4
{
    [TestFixture]
    public class ShortestPathsTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sully1")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"ShortestPaths\" + file, ShortestPaths.Answer);
        }
    }
}
