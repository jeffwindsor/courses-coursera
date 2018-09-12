using AlgorithmsOnGraphs.W1;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W1
{
    [TestFixture]
    public class ReachabilityTests : BaseTests
    {
        [TestCase("Sample1")]
        [TestCase("Sample2")]
        [TestCase("Sample10")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"Reachability\" + file, Reachability.Answer);
        }

        //Find
        //NotFound
        //Circles
        //ShortCircuit of Visited

    }
}
