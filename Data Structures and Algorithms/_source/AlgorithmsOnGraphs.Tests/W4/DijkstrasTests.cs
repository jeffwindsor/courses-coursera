using AlgorithmsOnGraphs.W4;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W4
{
    [TestFixture]
    public class DijkstrasTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample3")]
        [TestCase("Case2")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"Dijkstras\" + file, Dijkstras.Answer);
        }

    }
}
