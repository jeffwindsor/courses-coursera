using System.Text;

namespace DataStructures
{
    public class BinarySearchTreeNodePrinter
    {
        private readonly BinarySearchTreeNode _original;
        private readonly StringBuilder _sb;

        public BinarySearchTreeNodePrinter(BinarySearchTreeNode node)
        {
            _original = node;
            _sb = new StringBuilder();
        }

        public string Print()
        {
            if (_original == null)
            {
                PrintNodeValue(_original);
            }
            else
            {
                Print(GetRoot(_original));
            }
            return _sb.ToString();
        }

        private void Print(BinarySearchTreeNode node)
        {

            if (node != null && node.Right != null)
            {
                Print(node.Right, true, "");
            }
            PrintNodeValue(node);
            if (node != null && node.Left != null)
            {
                Print(node.Left, false, "");
            }
        }

        private void PrintNodeValue(BinarySearchTreeNode node)
        {
            _sb.AppendLine((node == null) 
                ? "<null>" 
                : (_original.Key == node.Key)
                    ? "[o]" + node.ToString()
                    : node.ToString()
                );
        }

        // use string and not stringbuffer on purpose as we need to change the indent at each recursion
        private void Print(BinarySearchTreeNode node, bool isRight, string indent)
        {
            if (node.Right != null)
            {
                Print(node.Right, true, indent + (isRight ? "        " : " |      "));
            }
            _sb.Append(indent);
            _sb.Append(isRight ? " /" : " \\");
            _sb.Append("----- ");

            PrintNodeValue(node);
            if (node.Left != null)
            {
                Print(node.Left, false, indent + (isRight ? " |      " : "        "));
            }
        }

        private static BinarySearchTreeNode GetRoot(BinarySearchTreeNode node)
        {
            if (node == null) return null;

            while (node.Parent != null)
                node = node.Parent;
            return node;
        }
    }
}
