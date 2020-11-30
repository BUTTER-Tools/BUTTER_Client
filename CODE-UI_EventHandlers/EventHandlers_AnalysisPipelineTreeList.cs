using PluginContracts;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System;

namespace BUTTER_Client
{


    public partial class MainForm : Form
    {



        //this makes sure that it's always highlighted when selected
        private void PluginsToRunTreeList_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var unfocused = !e.Node.TreeView.Focused;

            //sets up the color to use if the node isn't properly connected
            Color HighlighColor = Color.Transparent;
            Color TextColor = SystemColors.HighlightText;



            if (e.Node.Level > 0 && !_Plugins[e.Node.Text].InputType.Contains(_Plugins[e.Node.Parent.Text].OutputType))
            {
                HighlighColor = Color.Red;
                TextColor = Color.Black;
                e.Node.ToolTipText = "This plugin is not connected to the correct type of parent plugin." + Environment.NewLine +
                                     "This node accepts one of the following types of input from its parent:" + Environment.NewLine + Environment.NewLine +
                                      "\t" + string.Join(", ", _Plugins[e.Node.Text].InputType) + Environment.NewLine + Environment.NewLine +
                                      "However, its parent plugin is trying to give it the \"" + _Plugins[e.Node.Parent.Text].OutputType + "\"" + Environment.NewLine +
                                      "type of data as an input.";
            }
            else if (e.Node.Level == 0 && _Plugins[e.Node.Text].TopLevel == false)
            {
                HighlighColor = Color.Red;
                TextColor = Color.Black;
                e.Node.ToolTipText = "This plugin is not able to be a top-level plugin. It needs to be" + Environment.NewLine +
                                     "connected to another plugin, such as an input-type plugin that" + Environment.NewLine +
                                     "reads .txt or .csv files";
            }
            else if (e.Node.Level > 0 && _Plugins[e.Node.Text].TopLevel == true)
            {
                HighlighColor = Color.Red;
                TextColor = Color.Black;
                e.Node.ToolTipText = "This plugin is a top-level plugin. It needs to be" + Environment.NewLine +
                                     "situated at the top of a plugin chain.";
            }
            else
            {
                e.Node.ToolTipText = string.Empty;
            }


            

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, TextColor, HighlighColor, TextFormatFlags.GlyphOverhangPadding);
            }
            else if (selected)
            {
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, TextColor, HighlighColor, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {

                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, e.Node.ForeColor, HighlighColor, TextFormatFlags.GlyphOverhangPadding);
            }
            
        }



        private void AnalysisPipelineTreeList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateDescription(AnalysisPipelineTreeList.SelectedNode);
        }
        private void AnalysisPipelineTreeList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            UpdateDescription(AnalysisPipelineTreeList.SelectedNode);
        }


        //private bool AnalysisPipelineNodesClicked = false;
        //go to the settings of a node if we doubleclick it
        private void AnalysisPipelineTreeList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
          // after adding the contextmenustrip, I don't know if we really want this behavior anymore
          // but... I kept trying to do it after removing it, so it's back in for now.
          PipelinePlugins[AnalysisPipelineTreeList.SelectedNode].ChangeSettings();
        }

        private void AnalysisPipelineTreeList_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            //if (AnalysisPipelineNodesClicked)
            //{
            //    e.Cancel = true;
            //AnalysisPipelineNodesClicked = false;
            //}


        }


    }



}






