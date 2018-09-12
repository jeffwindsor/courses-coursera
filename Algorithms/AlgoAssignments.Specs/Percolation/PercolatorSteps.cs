using System;
using System.Linq;
using AlgoAssignments.Percolation;
using Algorithms.GraphManagers;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AlgoAssignments.Specs.Percolation
{
    [Binding]
    public class PercolatorSteps
    {
        private Percolator _peroclator;
        private int _dimension;

        [Given(@"I have a (.*) peroclator with a dimension of (.*)")]
        public void GivenIHaveAPeroclatorWithADimensionOf(string graphManagerTypeName, int dimension)
        {
            //HACK: may want to improve on this later, string to type mapping is messy
            switch (graphManagerTypeName)
            {
                case "QuickFindGraphManager":
                    _peroclator = new Percolator(dimension, (n) => new QuickFindGraphManager(n));
                    break;

                case "QuickWeigthedUnionGraphManager":
                    _peroclator = new Percolator(dimension, (n) => new QuickWeightedUnionGraphManager(n));
                    break;

                default:
                    throw new Exception("Graph Manager Type Unknown, cannot create");
            }
            _dimension = dimension;
        }
        
        [When(@"I connect a line of squares from top to bottom")]
        public void WhenIConnectALineOfSquaresFromTopToBottom()
        {
            //Open Column
            for(var row = 1; row <= _dimension; ++row)
            {
                _peroclator.Open(row, 1);
            }
        }

        [When(@"it percolates")]
        public void WhenItPercolates()
        {
            _peroclator.Percolate();
        }
        [Given(@"I initialize the percolator")]
        public void GivenIInitializeThePercolator()
        {
            _peroclator.InitializeGraphManager();
        }

        [Then(@"run time is greater than zero")]
        public void ThenRunTimeIsGreaterThanZero()
        {
            _peroclator.RunTime.Should().BeGreaterThan(TimeSpan.MinValue);
        }

        [Then(@"all squares are open")]
        public void ThenAllSquaresAreOpen()
        {
            var results = from row in Enumerable.Range(1, _peroclator.Rows)
                          from col in Enumerable.Range(1, _peroclator.Columns)
                          select _peroclator.IsOpen(row, col);
            results.All(i => true).Should().BeTrue();
        }
        
        [Then(@"Peroclator does not percolate")]
        public void ThenPeroclatorDoesNotPercolate()
        {
            _peroclator.IsPercolated().Should().BeFalse();
        }
        
        [Then(@"Peroclator does percolate")]
        public void ThenPeroclatorDoesPercolate()
        {
            _peroclator.IsPercolated().Should().BeTrue();
        }
    }
}
