using System.Collections.Generic;
using System.Windows.Forms;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {

        ////https://stackoverflow.com/a/19691384
        IEnumerable<TreeNode> TreeNodeRecursiveCollection(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (var child in TreeNodeRecursiveCollection(node.Nodes))
                    yield return child;
            }
        }


    }


}






