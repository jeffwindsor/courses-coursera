using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithms.GraphManagers;

namespace AlgoAssignments.Percolation
{
    public class PercolationRunnner
    {
        private int _dimension;
        private int _numberOfTrails;
        private Func<int, IGraphManager> _createGraphManager;
        public PercolationRunnner(int dimension, int numberOfTrails, Func<int, IGraphManager> createGraphManager)
        {

        }

        public void Run()
        {

            //Print Mean, StdDev, ConfLo, ConfHi
        }

        public double Mean { get; private set; }
        public double StandardDeviation { get; private set; }
        public double ConfidenceLo { get; private set; }
        public double ConfidenceHi { get; private set; }

        public override string ToString()
        {
            return string.Format("", _dimension, _numberOfTrails, Mean, StandardDeviation, ConfidenceLo, ConfidenceHi);
        }
    }
}
