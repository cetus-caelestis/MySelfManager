using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUtility
{
    partial class Utility
    {

        static public void ForEach(TreeNodeCollection treeNode, Action<TreeNode> callback)
        {
            // Print the node.
            foreach (TreeNode n in treeNode)
            {
                callback(n);
                ForEach(n.Nodes, callback);
            }
        }
        static public void ForEach(TreeView treeView, Action<TreeNode> callback)
        {
            ForEach(treeView.Nodes, callback);
        }

        //http://stackoverflow.com/questions/1155977/populate-treeview-from-a-list-of-path
        static public TreeNode PopulateTreeView(TreeView treeView, string path)
        {
            TreeNode lastNode = null;
            string subPathAgg = string.Empty;

            foreach (string subPath in path.Split(treeView.PathSeparator.ToCharArray()))
            {
                subPathAgg += subPath + treeView.PathSeparator;
                TreeNode[] nodes = treeView.Nodes.Find(subPathAgg, true);
                if (nodes.Length == 0)
                {
                    if (lastNode == null)
                        lastNode = treeView.Nodes.Add(subPathAgg, subPath);
                    else
                        lastNode = lastNode.Nodes.Add(subPathAgg, subPath);
                }
                else
                {
                    lastNode = nodes[0];
                }
            }
            return lastNode;
        }

        // http://dobon.net/vb/dotnet/control/tvdraganddrop.html
        private static bool IsChildNode(TreeNode parentNode, TreeNode childNode)
        {
            if (childNode.Parent == parentNode)
                return true;
            else if (childNode.Parent != null)
                return IsChildNode(parentNode, childNode.Parent);
            else
                return false;
        }
    }
}
