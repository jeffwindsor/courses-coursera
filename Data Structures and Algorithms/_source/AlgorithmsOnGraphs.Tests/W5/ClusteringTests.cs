using AlgorithmsOnGraphs.W5;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W5
{
    [TestFixture]
    public class ClusteringTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Case2")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"Clustering\" + file, Clustering.Answer);
        }
    }
}
