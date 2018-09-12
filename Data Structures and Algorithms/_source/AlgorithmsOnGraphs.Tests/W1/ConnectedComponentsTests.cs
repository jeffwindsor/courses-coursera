using AlgorithmsOnGraphs.W1;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests.W1
{
    [TestFixture]
    public class ConnectedComponentsTests : BaseTests
    {
        [TestCase("Sample1")]
        public void TestFiles(string file)
        {
            TestFromRelativeFilePath(@"ConnectedComponents\" + file, ConnectedComponents.Answer);
        }

        //Find
        //NotFound
        //Circles
        //ShortCircuit of Visited

    }
}
