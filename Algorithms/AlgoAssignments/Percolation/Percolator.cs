using System;
using Algorithms.GraphManagers;
using FluentAssertions;

namespace AlgoAssignments.Percolation
{
    /// <summary>
    /// Percolation. Given a composite systems comprised of randomly distributed insulating and metallic materials: 
    /// what fraction of the materials need to be metallic so that the composite system is an electrical conductor?
    ///  Given a porous landscape with water on the surface (or oil below), under what conditions will the water be 
    /// able to drain through to the bottom (or the oil to gush through to the surface)? Scientists have defined an 
    /// abstract process known as percolation to model such situations.
    /// 
    /// The model. We model a percolation system using an N-by-N grid of sites. Each site is either open or blocked.
    ///  A full site is an open site that can be connected to an open site in the top row via a chain of neighboring
    ///  (left, right, up, down) open sites. We say the system percolates if there is a full site in the bottom row. 
    /// In other words, a system percolates if we fill all open sites connected to the top row and that process fills 
    /// some open site on the bottom row. (For the insulating/metallic materials example, the open sites correspond to 
    /// metallic materials, so that a system that percolates has a metallic path from top to bottom, with full sites 
    /// conducting. For the porous substance example, the open sites correspond to empty space through which water 
    /// might flow, so that a system that percolates lets water fill open sites, flowing from top to bottom.)
    /// </summary>
    public class Percolator
    {
        private readonly int _dimension;
        private bool[] _squareOpenStatus;
        private readonly Func<int, IGraphManager> _createGraphManager;
        private IGraphManager _graphManager;
        private int _capNodeIndex;
        private int _baseNodeIndex;
        private TimeSpan _runTime = TimeSpan.MinValue;

        public Percolator(int dimension, Func<int,IGraphManager> createGraphManager)
        {
            dimension.Should().BePositive();
            createGraphManager.Should().NotBeNull();

            _dimension = dimension;
            _createGraphManager = createGraphManager;
        }

        //Randomly open 
        public void Percolate()
        {
            var start = DateTime.Now;
            InitializeGraphManager();
            // Randomly open nodes, until percolated (what about already open nodes?)
            var r = new Random();
            do
            {

                Open(r.Next(1, _dimension), r.Next(1, _dimension));

            } while (IsPercolated() == false);
            _runTime = DateTime.Now.Subtract(start);
        }

        public int Rows { get { return _dimension; } }
        public int Columns { get { return _dimension; } }
        public TimeSpan RunTime { get { return _runTime; } }

        public bool IsOpen(int row, int col)
        {
            var square = GetIndex(row, col);
            return _squareOpenStatus[square];
        }
        
        public void Open(int row, int col)
        {
            var squareIndex = GetIndex(row, col);
            if (_squareOpenStatus[squareIndex] == false)
            {
                _squareOpenStatus[squareIndex] = true;

                //Up
                if (row > 1)
                    _graphManager.ConnectNodes(squareIndex, GetIndex(row - 1, col));

                //Down
                if (row < _dimension)
                    _graphManager.ConnectNodes(squareIndex, GetIndex(row + 1, col));

                //Left
                if (col > 1)
                    _graphManager.ConnectNodes(squareIndex, GetIndex(row, col - 1));

                //Right
                if (col < _dimension)
                    _graphManager.ConnectNodes(squareIndex, GetIndex(row, col + 1));
            }
        }

        public bool IsPercolated()
        {
            //If the cap and base nodes are connected in any way then the system percolates
            return _graphManager.AreNodesConnected(_capNodeIndex, _baseNodeIndex);
        }

        public void InitializeGraphManager()
        {
            var size = (_dimension*_dimension);
 
            //Initialize open status array
            _squareOpenStatus= new bool[size];

            //Total Node size is dimension squared plus two helper nodes for top and bottom rows
            _graphManager = _createGraphManager(size + 2);
            _capNodeIndex = size;
            _baseNodeIndex = size + 1;

            //Connect the top and bottom row to the extra elements
            //This will allow easy percolation test
            var bottomRowStartIndex = _dimension*(_dimension - 1);
            for (var i = 0; i < _dimension; ++i)
            {
                //Top row
                _graphManager.ConnectNodes(_capNodeIndex, i);
                //Bottom row
                _graphManager.ConnectNodes(_baseNodeIndex, bottomRowStartIndex + 1);
            }
        }

        //row and col are both one index, grpah is zero index
        //Validate row and col as well
        private int GetIndex(int row, int col)
        {
            row.Should().BePositive();
            row.Should().BeLessOrEqualTo(_dimension, "Row cannot be greater than dimensions");
            col.Should().BePositive();
            col.Should().BeLessOrEqualTo(_dimension, "Row cannot be greater than dimensions");

            return ( (row - 1) * _dimension) + (col - 1);
        }

    }
}
