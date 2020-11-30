using System.Windows.Forms;
using System.Drawing;
using System;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {



        //public TreeNode previousSelectedNode = null;


        ////prevents collapsing of the tree
        //private void PluginTreeList_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        //{
        //    //this would prevent the user from collapsing the tree. given how many plugins there will be, I'm thinking that collapsable trees are a good thing.
        //    //e.Cancel = true;
        //}




        #region Different Ways of interacting with the AvailablePluginTreeList to get nodes into the Analysis Pipeline

        private void AvailablePluginTreeList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            AddPluginToPipeline(AvailablePluginTreeList.SelectedNode);
        }
        #endregion





        //load the information for the selected plugin
        private void AvailablePluginTreeList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateDescription(AvailablePluginTreeList.SelectedNode);
        }
        private void AvailablePluginTreeList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            AvailablePluginTreeList.SelectedNode = e.Node;
            UpdateDescription(AvailablePluginTreeList.SelectedNode);
        }




        private void AvailablePluginTreeList_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var unfocused = !e.Node.TreeView.Focused;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }
        }






    }








}






