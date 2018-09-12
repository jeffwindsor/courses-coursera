using AlgorithmsOnGraphs.W2;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W2
{
    [TestFixture]
    public class StronglyConnectedTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"StronglyConnected\" + file, StronglyConnected.Answer);
        }

    }
}
