namespace Algorithms.GraphManagers
{
    ///<summary>
    /// Adds a size array for node sets
    ///</summary>
    /// <remarks>
    /// Find is constant time
    /// Union is quadratic (~N^2)
    /// </remarks>
    public class QuickWeightedUnionGraphManager: BaseUnionFindGraphManager
    {
        private readonly int[] _nodeSetSize;
        public QuickWeightedUnionGraphManager(int n)
            : base(n)
        {
            _nodeSetSize = CreateInitializedArray(n, i => 1); 
        }
        
        public override void ConnectNodes(int a, int b)
        {
            var aSetIndex = FindNode(a);
            var bSetIndex = FindNode(b);

            //If they are in the same root set do nothing, they aer already connected
            if (aSetIndex == bSetIndex) return;

            //Call weighted union based on which root set is larger
            if (_nodeSetSize[aSetIndex] < _nodeSetSize[bSetIndex])
                UnionWeighted(aSetIndex, bSetIndex);
            else
                UnionWeighted(bSetIndex, aSetIndex);
        }

        // Weighted quick union algo
        //   Set root of small set to be root of larger set
        //   Update the larger sets size
        private void UnionWeighted(int smallerSetIndex, int largerSetIndex)
        {
            Nodes[smallerSetIndex] = largerSetIndex;
            _nodeSetSize[largerSetIndex] += _nodeSetSize[smallerSetIndex];
        }

        /// <summary>
        /// Walk up the connections until you find a root set
        /// A root set is defined when the index = value
        /// </summary>
        public override int FindNode(int a)
        {
            while (a != Nodes[a])
                a = Nodes[a];
            return a;
        }
    }
}
