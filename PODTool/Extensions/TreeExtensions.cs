using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PODTool
{
    public static class TreeExtensions
    {
        public static TreeNode FindFirstItem(this TreeNode node, string itemText)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text.Equals(itemText, StringComparison.OrdinalIgnoreCase))
                    return child;
            }
            return null;
        }

        public static bool ContainsItem(this TreeNode node, string itemText)
        {
            foreach(TreeNode child in node.Nodes)
            {
                if (child.Text.Equals(itemText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static void AddSorted(this TreeNodeCollection collection, TreeNode node)
        {
            int index = 0;

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                var child = collection[i];
                if (child == node)
                {
                    continue;
                }

                int cResult = StringComparer.OrdinalIgnoreCase.Compare(node.Text, child.Text);
                if (cResult >= 0)
                {
                    index = i + 1;
                    break;
                }
            }

            collection.Insert(index, node);
        }

        private static void SortSingle_Internal(TreeNode node)
        {
            if (node.TreeView == null)
                return;

            var oldParent = node.Parent;
            bool wasSelected = node.TreeView.SelectedNode == node;

            int oldIndex = node.Index;
            int index = 0;
            for(int i=oldParent.Nodes.Count - 1; i >= 0; i--)
            {
                var child = oldParent.Nodes[i];
                if(child == node)
                {
                    continue;
                }

                int cResult = StringComparer.OrdinalIgnoreCase.Compare(node.Text, child.Text);
                if (cResult >= 0)
                {
                    index = i+1;
                    break;
                }
            }

            if (index != oldIndex)
            {
                // we're removing the node temporarily, any index above will be - 1
                if (index > oldIndex)
                    index--;

                // re-order node
                node.TreeView.BeginUpdate();
                node.Remove();
                oldParent.Nodes.Insert(index, node);

                // re-select
                if (wasSelected)
                {
                    node.TreeView.SelectedNode = node;
                    node.EnsureVisible();
                }
                node.TreeView.EndUpdate();
            }
        }

        public static void SortSingle(this TreeNode node)
        {
            // can only sort self if already parented, and not the only one in the node tree
            if(node.Parent == null || node.Parent.Nodes.Count == 1)
            {
                return;
            }

            // PROBLEM: Really should not need to start an entire thread just to sort
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(10);
                node.TreeView.Invoke((Action)(() => { SortSingle_Internal(node); }));
            });
        }
    }
}
