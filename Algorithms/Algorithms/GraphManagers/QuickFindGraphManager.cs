namespace Algorithms.GraphManagers
{
    /// <remarks>
    /// Find is constant time
    /// Union is quadratic (~N^2)
    /// </remarks>
    public class QuickFindGraphManager: BaseUnionFindGraphManager 
    {
        public QuickFindGraphManager(int n) : base(n)
        {
        }

        public override void ConnectNodes(int a, int b)
        {
            var aRoot = FindNode(a);
            var bRoot = FindNode(b);

            //if not same root, adjust all indexes with an (a) roots to be (b) root
            if (aRoot != bRoot)
            {
                Nodes.ForEachIndex((s, i) => { if (s[i] == aRoot) s[i] = bRoot; });
            }
        }

        public override int FindNode(int a)
        {
            return Nodes[a];
        }
    }
}
