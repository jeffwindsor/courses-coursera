namespace DataStructures
{
    public class BinarySearchTreeNode
    {
        private BinarySearchTreeNode _left;
        private BinarySearchTreeNode _right;

        public long Key { get; set; }
        public BinarySearchTreeNode Parent { get; set; }

        public BinarySearchTreeNode Left {
            get { return _left; }
            set
            {
                _left = value;
                if(value != null)
                    value.Parent = this;
            }
        }
        public BinarySearchTreeNode Right {
            get { return _right; }
            set
            {
                _right = value;
                if (value != null)
                    value.Parent = this;
            }
        }

        public int Rank { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Key); //string.Format("{0}:{1}", Key, Rank);
        }

        public enum SideOfParent
        {
            NO = 0,
            LEFT = -1,
            RIGHT = 1,
        }
        
        public static SideOfParent SideOf(BinarySearchTreeNode parent, BinarySearchTreeNode child)
        {
            if (child == null || child.Parent == null)
                return SideOfParent.NO;

            return (parent.Right == child) ? SideOfParent.RIGHT
                : (parent.Left == child) ? SideOfParent.LEFT
                : SideOfParent.NO;
        }

        public static void ReplaceChild(BinarySearchTreeNode parent, BinarySearchTreeNode currentChild, BinarySearchTreeNode newChild)
        {
            switch (SideOf(parent, currentChild))
            {
                case SideOfParent.LEFT:
                    currentChild.Parent = null;
                    parent.Left = newChild;
                    return;
                case SideOfParent.RIGHT:
                    currentChild.Parent = null;
                    parent.Right = newChild;
                    return;
                default:
                    if(newChild != null)
                        newChild.Parent = null;
                    return;
            }
        }

        public void Erase()
        {
            _left = null;
            _right = null;
            Parent = null;
        }
    }
}