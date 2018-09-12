using AlgorithmsOnGraphs.W2;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W2
{
    [TestFixture]
    public class AcyclicityTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"Acyclicity\" + file, Acyclicity.Answer);
        }

    }
}
