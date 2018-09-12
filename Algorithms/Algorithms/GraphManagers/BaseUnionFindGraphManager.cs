using System;

namespace Algorithms.GraphManagers
{
    /// <summary>
    /// Maintain an array where index is the node id
    /// and value at index is the node's connection set id
    /// </summary>
    public abstract class BaseUnionFindGraphManager: IGraphManager
    {
        private readonly int[] _nodes;
        protected BaseUnionFindGraphManager(int n)
        {
            _nodes = CreateInitializedArray(n, i => i);
        }
        
        protected int[] Nodes { get { return _nodes; } }
        public abstract void ConnectNodes(int a, int b);
        public abstract int FindNode(int a);
        public bool AreNodesConnected(int a, int b){ return FindNode(a) == FindNode(b);}
        public int NodeCount{get { return _nodes.Length; }}
        
        protected static int[] CreateInitializedArray(int n, Func<int,int> getValueForIndex )
        {
            var result = new int[n];
            result.ForEachIndex((s, i) => s[i] = getValueForIndex(i));
            return result;
        }

        
    }
}
