using System;
using System.Windows.Forms;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {


        private void RemovePluginFromPipelineButton_Click(object sender, EventArgs e)
        {
            //make sure that a node exists in the first place, and check to make sure that there is one
            //selected before trying to remove a node
            if ((AnalysisPipelineTreeList.Nodes.Count > 0) && (AnalysisPipelineTreeList.SelectedNode != null))
            {

                TreeNode NodeToDrop = AnalysisPipelineTreeList.SelectedNode;

                PipelinePlugins.Remove(NodeToDrop);
                AnalysisPipelineTreeList.Nodes.Remove(NodeToDrop);


                foreach (var node in TreeNodeRecursiveCollection(NodeToDrop.Nodes)) PipelinePlugins.Remove(node);

                //I've got to be honest, I have no idea why this works
                //If we don't do something here, we end up with a screwed up nodemap.
                //So, as an example, if we add 20 nodes and move some around, then remove
                //a bunch of nodes down to (for example) 3, there's a problem.
                //when you try to move one of those 3 to the end (or just drag/drop it beneath the treeview nodes)
                //an error gets thrown.
                //
                //so, thus far, the behavior of just setting the nodemap to a blank string seems to solve the problem.
                //hopefully, this will continue to work.
                NodeMap = "";

                if (AnalysisPipelineTreeList.SelectedNode == null)
                {
                    AvailablePluginTreeList.SelectedNode = AvailablePluginTreeList.TopNode;
                }
                else
                {
                    AnalysisPipelineTreeList.SelectedNode.EnsureVisible();
                }

                

            }


        }



    }


}






