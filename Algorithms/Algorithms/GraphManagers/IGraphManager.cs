namespace Algorithms.GraphManagers
{
    public interface IGraphManager
    {
        void ConnectNodes(int a, int b);
        bool AreNodesConnected(int a, int b);
        int FindNode(int a);
        int NodeCount { get; }
    }
}
