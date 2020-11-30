using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;
using PluginContracts;

namespace BUTTER_Client
{


    public partial class MainForm : Form
    {




        //we use this to hold what node we want to copy
        private TreeNode CopiedNode { get; set; } = null;
        private Dictionary<TreeNode, Plugin> CopiedNodeDict { get; set; }




        #region Analysis Pipeline Context Menus
        //makes sure that we use this contextmenustrip when someone rightclicks *specifically* on a node
        private void AnalysisPipelineTreeList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode ClickNode = AnalysisPipelineTreeList.GetNodeAt(ClickPoint);

                // Convert from Tree coordinates to Screen coordinates    
                Point ScreenPoint = AnalysisPipelineTreeList.PointToScreen(ClickPoint);
                // Convert from Screen coordinates to Form coordinates    
                Point FormPoint = this.PointToClient(ScreenPoint);

                //what we do if a node was right-clicked
                if (ClickNode != null)
                {
                    //just auto-enable them all by default, then we can get into the logic of which ones are on and off
                    for (int i = 0; i < AnalysisPipelineContextMenu.Items.Count; i++) AnalysisPipelineContextMenu.Items[i].Enabled = true;


                    //make sure that we have a node on the "clipboard" if we're going
                    //to allow pasting
                    if (CopiedNode == null)
                    {
                        AnalysisPipelineContextMenu.Items[1].Enabled = false;
                    }  

                    //test if URL is valid for tutorial/help
                    if (ValidateURL(_Plugins[ClickNode.Text].PluginTutorial))
                    {
                        AnalysisPipelineContextMenu.Items[3].Text = "Plugin Help";
                    }
                    else
                    {
                        AnalysisPipelineContextMenu.Items[3].Enabled = false;
                        AnalysisPipelineContextMenu.Items[3].Text = "Plugin Help (Unavailable)";
                    }
                }
                //what we do if a right-click was made, but *not* on a node
                else
                {
                    //just auto-disable all of them by default, then we can get into the logic of which ones are on and off
                    for (int i = 0; i < AnalysisPipelineContextMenu.Items.Count; i++) AnalysisPipelineContextMenu.Items[i].Enabled = false;

                    if (CopiedNode != null)
                    {
                        AnalysisPipelineContextMenu.Items[1].Enabled = true;
                    }

                }
                

                // Show context menu   
                AnalysisPipelineContextMenu.Show(this, FormPoint);
            }
        }

        private void AnalysisPipelineContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            AnalysisPipelineContextMenu.Hide();

            ToolStripItem item = e.ClickedItem;
            if (item.Text == "Plugin Settings")
            {
                PluginSettingsButton_Click(AnalysisPipelineContextMenu, null);
            }
            else if (item.Text == "Remove Plugin")
            {
                RemovePluginFromPipelineButton_Click(AnalysisPipelineContextMenu, null);
            }
            else if (item.Text == "Plugin Help")
            {
                LaunchPluginHelp(AnalysisPipelineTreeList.SelectedNode);
            }
            else if (item.Text == "Copy Plugin(s)")
            {
                CopyPlugin(AnalysisPipelineTreeList.SelectedNode);
            }
            else if (item.Text == "Paste Plugin(s)")
            {
                PastePlugin(AnalysisPipelineTreeList.SelectedNode);
            }


        }

        #endregion







        #region Available Plugin Context Menus
        //makes sure that we use this contextmenustrip when someone rightclicks *specifically* on a node
        private void AvailablePluginTreeList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode ClickNode = AvailablePluginTreeList.GetNodeAt(ClickPoint);
                if (ClickNode == null || !_Plugins.ContainsKey(ClickNode.Text)) return;
                // Convert from Tree coordinates to Screen coordinates    
                Point ScreenPoint = AvailablePluginTreeList.PointToScreen(ClickPoint);
                // Convert from Screen coordinates to Form coordinates    
                Point FormPoint = this.PointToClient(ScreenPoint);

                //test if URL is valid for tutorial/help
                if (ValidateURL(_Plugins[ClickNode.Text].PluginTutorial))
                {
                    AvailablePluginsContextMenu.Items[1].Enabled = true;
                    AvailablePluginsContextMenu.Items[1].Text = "Plugin Help";
                }
                else
                {
                    AvailablePluginsContextMenu.Items[1].Enabled = false;
                    AvailablePluginsContextMenu.Items[1].Text = "Plugin Help (Unavailable)";
                }


                // Show context menu   
                AvailablePluginsContextMenu.Show(this, FormPoint);
            }
        }

        private void AvailablePluginContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            AvailablePluginsContextMenu.Hide();

            ToolStripItem item = e.ClickedItem;
            if (item.Text == "Add to Pipeline")
            {
                AddPluginToPipeline(AvailablePluginTreeList.SelectedNode);
            }
            else if (item.Text == "Plugin Help")
            {
                LaunchPluginHelp(AvailablePluginTreeList.SelectedNode);
            }



        }


        #endregion




        private void LaunchPluginHelp(TreeNode selectedNode)
        {
            string helpLink = _Plugins[selectedNode.Text].PluginTutorial;
            string author = _Plugins[selectedNode.Text].PluginAuthor;

            bool result = ValidateURL(helpLink);

            if (result)
            {
                System.Diagnostics.Process.Start(helpLink);
            }
            else
            {
                MessageBox.Show("The help/tutorial for this plugin is either unavailable or is not a valid URL. If you believe this is a mistake, please contact the author of this plugin:" + Environment.NewLine + Environment.NewLine + author,
                                "Unable to Open Help URL", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
            

        }


        private bool ValidateURL(string helpLink)
        {

            Uri uriResult;
            bool result = Uri.TryCreate(helpLink, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return (result);
        }


        private void CopyPlugin(TreeNode selectedNode)
        {

            //first thing to do is make sure that this is empty
            CopiedNodeDict = new Dictionary<TreeNode, Plugin>();

            //second thing to do is clone the root node
            CopiedNode = (TreeNode)selectedNode.Clone();
            //...and add a copy of the plugin

            
            CopiedNodeDict.Add(CopiedNode, CopyPluginFromNode(selectedNode, PipelinePlugins));


            //third thing to do is create a list that will hold all of our plugin settings
            //for everything BELOW the root node that we're looking at
            List<Plugin> PluginsToCopyList = new List<Plugin>();
            //now we create a copy of the plugins from the original root
            foreach (TreeNode node in TreeNodeRecursiveCollection(selectedNode.Nodes))
            {
                PluginsToCopyList.Add(CopyPluginFromNode(node, PipelinePlugins));
            }

            int nodeCountTracker = 0;
            foreach (TreeNode node in TreeNodeRecursiveCollection(CopiedNode.Nodes))
            {
                CopiedNodeDict.Add(node, PluginsToCopyList[nodeCountTracker]);
                nodeCountTracker++;
            }
            
            
        }

        private void PastePlugin(TreeNode selectedNode)
        {

            //we have to kind of go through the whole "copy the node and all of its points in the plugin dictionary"
            //process again, otherwise we'll run into problems where the exact same node can't exist twice in the
            //same TreeListView. 

            //so we start with copying the node itself from the "clipboard"
            TreeNode NodeToPaste = (TreeNode)CopiedNode.Clone();

            //then we add in the copied node, either by dropping it into a new place on the pipeline
            //(if no node is currently selected)
            if (selectedNode == null)
            {
                AnalysisPipelineTreeList.Nodes.Add(NodeToPaste);
                AnalysisPipelineTreeList.SelectedNode = NodeToPaste;
                AnalysisPipelineTreeList.SelectedNode.ExpandAll();
            }
            //otherwise, we add this as a sub-node to the selected node
            else
            {
                selectedNode.Nodes.Add(NodeToPaste);
                AnalysisPipelineTreeList.SelectedNode = NodeToPaste;
                //make sure to expand the entire thing
                AnalysisPipelineTreeList.SelectedNode.ExpandAll();
            }



            //now we create a new dictionary to hold the settings/plugins that we're copying
            //out of the clipboard version of the node
            Dictionary<TreeNode, Plugin> PastedNodeDict = new Dictionary<TreeNode, Plugin>();
            PastedNodeDict.Add(NodeToPaste, CopyPluginFromNode(CopiedNode, CopiedNodeDict));


            //this is basically the exact same process that we went through
            //when we copy a node to the clipboard, except now we're copying it
            //*from* the clipboard to a new, temporary List/Dictionary so that 
            //we can bring these copies into the main Plugin dictionary
            List<Plugin> PluginsToPaste = new List<Plugin>();
            //now we create a copy of the plugins from the CopiedNode "clipboard"
            foreach (TreeNode node in TreeNodeRecursiveCollection(CopiedNode.Nodes))
            {
                PluginsToPaste.Add(CopyPluginFromNode(node, CopiedNodeDict));
            }

            int nodeCountTracker = 0;
            foreach (TreeNode node in TreeNodeRecursiveCollection(NodeToPaste.Nodes))
            {
                PastedNodeDict.Add(node, PluginsToPaste[nodeCountTracker]);
                nodeCountTracker++;
            }


            //finally, we bring over all of the plugins to the main "PipelinePlugins" dictionary
            foreach (TreeNode node in PastedNodeDict.Keys)
            {
                PipelinePlugins.Add(node, CopyPluginFromNode(node, PastedNodeDict));
            }

        }









        //basically a half-assed way of cloning a plugin. we just create a new instance,
        //then import the settings from the original's ExportSettings() method
        private Plugin CopyPluginFromNode (TreeNode inputNode, Dictionary<TreeNode, Plugin> dictToCopyFrom)
        {
            Plugin TempPlugin = (Plugin)Activator.CreateInstance(_Plugins[inputNode.Text].GetType());
            TempPlugin.ImportSettings(dictToCopyFrom[inputNode].ExportSettings(suppressWarnings: true));
            return TempPlugin;
        }


    }


}






