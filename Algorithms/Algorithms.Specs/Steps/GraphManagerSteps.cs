using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using Algorithms.GraphManagers;

namespace Algorithms.Specs.Steps
{
    [Binding]
    public class GraphManagerSteps
    {
        private  IGraphManager  _gm;

        [Given(@"I have a (.*) with (.*) nodes")]
        public void GivenIHaveAGraphManagerWithNumberOfNodes(string typeName, int n)
        {
            //HACK: may want to improve on this later, string to type mapping is messy
            switch (typeName)
            {
                case "QuickFindGraphManager":
                    _gm = new QuickFindGraphManager(n);
                    break;

                case "QuickWeigthedUnionGraphManager":
                    _gm = new QuickWeightedUnionGraphManager(n);
                    break;

                default:
                    throw new Exception("Graph Manager Type Unknown, cannot create");
            }
        }

        [Then(@"each node should be connected to itself")]
        public void ThenEachNodeShouldBeConnectedToItself()
        {
            var n = _gm.NodeCount;
            for (var i = 0; i < n; ++i)
            {
                _gm.FindNode(i).Should().Be(i, "Node should initialize its value (set id) to the nodes index");
            }
        }

        [When(@"I connect node (.*) with node (.*)")]
        public void WhenIConnectNodeWithNode(int p0, int p1)
        {
            _gm.ConnectNodes(p0,p1);
        }

        [Then(@"node (.*) should be connected to node (.*)")]
        public void ThenNodeShouldBeConnectedToNode(int p0, int p1)
        {
            _gm.AreNodesConnected(p0, p1).Should().BeTrue();
        }

        [Then(@"number of nodes equals (.*)")]
        public void ThenNumberOfNodesEquals(int p0)
        {
            _gm.NodeCount.Should().Be(p0);
        }
        
    }
}
