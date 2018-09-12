using AlgorithmsOnGraphs.W2;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W2
{
    [TestFixture]
    public class TopsortTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"Topsort\" + file, Topsort.Answer);
        }

    }
}
