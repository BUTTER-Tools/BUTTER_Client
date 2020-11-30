using System;
using System.Windows.Forms;
using PluginContracts;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {





        private void AddPluginToPipelineButton_Click(object sender, EventArgs e)
        {
            AddPluginToPipeline(AvailablePluginTreeList.SelectedNode);
        }


        private void AddPluginToPipeline(TreeNode node)
        {
            if (node != null)
            {

                //check to make sure that the selected item is a plugin
                if (_Plugins.ContainsKey(node.Text))
                {

                    //create a clone of the original node that we want to copy over to the analysis pipeline
                    TreeNode TempNode = (TreeNode)node.Clone();
                    //TempNode.Plugin = _Plugins[AvailablePluginTreeList.SelectedNode.Text];

                    //if there isn't a selected node, then we add this as a top-level node
                    if (AnalysisPipelineTreeList.SelectedNode == null)
                    {

                        AnalysisPipelineTreeList.Nodes.Add(TempNode);
                        AnalysisPipelineTreeList.SelectedNode = TempNode;
                    }
                    //otherwise, we add this as a sub-node to the selected node
                    else
                    {
                        AnalysisPipelineTreeList.SelectedNode.Nodes.Add(TempNode);
                        AnalysisPipelineTreeList.SelectedNode = TempNode;
                        //make sure to expand the entire thing
                        AnalysisPipelineTreeList.SelectedNode.Expand();
                    }

                    //Adds the node plus a new instance of the plug-in to a dictionary
                    //this way we have each node associated with an instance of any given plugin
                    Plugin TempPlugin = (Plugin)Activator.CreateInstance(_Plugins[AvailablePluginTreeList.SelectedNode.Text].GetType());
                    PipelinePlugins.Add(TempNode, TempPlugin);

                }

            }
            else
            {
                MessageBox.Show("You must first select the plugin that you would like to add from your \"Available Plugins\" tree.", "No Plugin Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }




        

    }


}






