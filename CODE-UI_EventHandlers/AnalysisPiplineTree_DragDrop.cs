using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System;
using PluginContracts;
using System.Collections.Generic;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {

        //https://www.codeproject.com/Articles/6184/TreeView-Rearrange

        #region Private Fields
        //private int NodeCount, FolderCount;
        private string NodeMap;
        private const int MAPSIZE = 128;
        private StringBuilder NewNodeMap = new StringBuilder(MAPSIZE);
        #endregion




        private void treeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ((TreeView)sender).SelectedNode = ((TreeView)sender).GetNodeAt(e.X, e.Y);
        }
        private void treeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void treeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {

            
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false) && !String.IsNullOrEmpty(this.NodeMap))
            {

               
                    TreeNode MovingNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                    
                    string[] NodeIndexes = this.NodeMap.Split('|');
                    TreeNodeCollection InsertCollection = ((TreeView)sender).Nodes;
                    for (int i = 0; i < NodeIndexes.Length - 1; i++)
                    {
                        InsertCollection = InsertCollection[Int32.Parse(NodeIndexes[i])].Nodes;
                    }

                    if (InsertCollection != null && !InsertCollection.Contains(MovingNode))
                    {

                        TreeNode NodeToAdd = (TreeNode)MovingNode.Clone();

                        InsertCollection.Insert(Int32.Parse(NodeIndexes[NodeIndexes.Length - 1]), NodeToAdd);
                        ((TreeView)sender).SelectedNode = InsertCollection[Int32.Parse(NodeIndexes[NodeIndexes.Length - 1])];

                        MovingNode.Remove();

                        //ADDED these lines so that moving things into subtrees, etc., doesn't screw up our PipelinePlugins dictionary


                        Plugin TempPlugin = PipelinePlugins[MovingNode];

                        List<Plugin> PluginListToRestore = new List<Plugin>();
                        foreach (var node in TreeNodeRecursiveCollection(MovingNode.Nodes))
                        {
                            PluginListToRestore.Add(PipelinePlugins[node]);
                            PipelinePlugins.Remove(node);
                        }
                      
                        PipelinePlugins.Remove(MovingNode);

                        PipelinePlugins.Add(NodeToAdd, TempPlugin);

                        int NodeToRestore = 0;
                        foreach (var node in TreeNodeRecursiveCollection(NodeToAdd.Nodes))
                        {
                            //TempPlugin = (Plugin)Activator.CreateInstance(_Plugins[node.Text].GetType());
                            TempPlugin = PluginListToRestore[NodeToRestore];
                            NodeToRestore++;
                            PipelinePlugins.Add(node, TempPlugin);
                        }

                        
                    


                    }
                    else
                    {
                        ((TreeView)sender).Refresh();
                    }

            }
        }

        private void treeView_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNode NodeOver = ((TreeView)sender).GetNodeAt(((TreeView)sender).PointToClient(Cursor.Position));
            TreeNode NodeMoving = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");


            // A bit long, but to summarize, process the following code only if the nodeover is null
            // and either the nodeover is not the same thing as nodemoving UNLESSS nodeover happens
            // to be the last node in the branch (so we can allow drag & drop below a parent branch)
            if (NodeOver != null && (NodeOver != NodeMoving || (NodeOver.Parent != null && NodeOver.Index == (NodeOver.Parent.Nodes.Count - 1))))
            {
                int OffsetY = ((TreeView)sender).PointToClient(Cursor.Position).Y - NodeOver.Bounds.Top;
                int NodeOverImageWidth = _Plugins[NodeOver.Text].GetPluginIcon.Width;
                Graphics g = ((TreeView)sender).CreateGraphics();

                //this was all code for top-level vs child nodes
                //they were doing a "folder" versus "item" thing, which made sense for them
                //but not for this app
                // Image index of 1 is the non-folder icon
                //if (NodeOver.Level > 0)
                //{
                //    #region Standard Node
                //    if (OffsetY < (NodeOver.Bounds.Height / 2))
                //    {
                //        //this.lblDebug.Text = "top";

                //        #region If NodeOver is a child then cancel
                //        TreeNode tnParadox = NodeOver;
                //        while (tnParadox.Parent != null)
                //        {
                //            if (tnParadox.Parent == NodeMoving)
                //            {
                //                this.NodeMap = "";
                //                return;
                //            }

                //            tnParadox = tnParadox.Parent;
                //        }
                //        #endregion
                //        #region Store the placeholder info into a pipe delimited string
                //        SetNewNodeMap(NodeOver, false);
                //        if (SetMapsEqual() == true)
                //            return;
                //        #endregion
                //        #region Clear placeholders above and below
                //        this.Refresh();
                //        #endregion
                //        #region Draw the placeholders
                //        this.DrawLeafTopPlaceholders(sender, NodeOver);
                //        #endregion
                //    }
                //    else
                //    {
                //        //this.lblDebug.Text = "bottom";

                //        #region If NodeOver is a child then cancel
                //        TreeNode tnParadox = NodeOver;
                //        while (tnParadox.Parent != null)
                //        {
                //            if (tnParadox.Parent == NodeMoving)
                //            {
                //                this.NodeMap = "";
                //                return;
                //            }

                //            tnParadox = tnParadox.Parent;
                //        }
                //        #endregion
                //        #region Allow drag drop to parent branches
                //        TreeNode ParentDragDrop = null;
                //        // If the node the mouse is over is the last node of the branch we should allow
                //        // the ability to drop the "nodemoving" node BELOW the parent node
                //        if (NodeOver.Parent != null && NodeOver.Index == (NodeOver.Parent.Nodes.Count - 1))
                //        {
                //            int XPos = ((TreeView)sender).PointToClient(Cursor.Position).X;
                //            if (XPos < NodeOver.Bounds.Left)
                //            {
                //                ParentDragDrop = NodeOver.Parent;

                //                if (XPos < (ParentDragDrop.Bounds.Left - ((TreeView)sender).ImageList.Images[ParentDragDrop.ImageIndex].Size.Width))
                //                {
                //                    if (ParentDragDrop.Parent != null)
                //                        ParentDragDrop = ParentDragDrop.Parent;
                //                }
                //            }
                //        }
                //        #endregion
                //        #region Store the placeholder info into a pipe delimited string
                //        // Since we are in a special case here, use the ParentDragDrop node as the current "nodeover"
                //        SetNewNodeMap(ParentDragDrop != null ? ParentDragDrop : NodeOver, true);
                //        if (SetMapsEqual() == true)
                //            return;
                //        #endregion
                //        #region Clear placeholders above and below
                //        this.Refresh();
                //        #endregion
                //        #region Draw the placeholders
                //        DrawLeafBottomPlaceholders(sender, NodeOver, ParentDragDrop);
                //        #endregion
                //    }
                //    #endregion
                //}
                //else
                //{
                    #region Folder Node
                    if (OffsetY < (NodeOver.Bounds.Height / 3))
                    {
                        //this.lblDebug.Text = "folder top";

                        #region If NodeOver is a child then cancel
                        TreeNode tnParadox = NodeOver;
                        while (tnParadox.Parent != null)
                        {
                            if (tnParadox.Parent == NodeMoving)
                            {
                                this.NodeMap = "";
                                return;
                            }

                            tnParadox = tnParadox.Parent;
                        }
                        #endregion
                        #region Store the placeholder info into a pipe delimited string
                        SetNewNodeMap(NodeOver, false);
                        if (SetMapsEqual() == true)
                            return;
                        #endregion
                        #region Clear placeholders above and below
                        this.Refresh();
                        #endregion
                        #region Draw the placeholders
                        this.DrawFolderTopPlaceholders(sender, NodeOver);
                        #endregion
                    }
                    else if ((NodeOver.Parent != null && NodeOver.Index == 0) && (OffsetY > (NodeOver.Bounds.Height - (NodeOver.Bounds.Height / 3))))
                    {
                        //this.lblDebug.Text = "folder bottom";

                        #region If NodeOver is a child then cancel
                        TreeNode tnParadox = NodeOver;
                        while (tnParadox.Parent != null)
                        {
                            if (tnParadox.Parent == NodeMoving)
                            {
                                this.NodeMap = "";
                                return;
                            }

                            tnParadox = tnParadox.Parent;
                        }
                        #endregion
                        #region Store the placeholder info into a pipe delimited string
                        SetNewNodeMap(NodeOver, true);
                        if (SetMapsEqual() == true)
                            return;
                        #endregion
                        #region Clear placeholders above and below
                        this.Refresh();
                        #endregion
                        #region Draw the placeholders
                        DrawFolderTopPlaceholders(sender, NodeOver);
                        #endregion
                    }
                    else
                    {
                        //this.lblDebug.Text = "folder over";

                        if (NodeOver.Nodes.Count > 0)
                        {
                            NodeOver.Expand();
                            //this.Refresh();
                        }
                        else
                        {
                            #region Prevent the node from being dragged onto itself
                            if (NodeMoving == NodeOver)
                                return;
                            #endregion
                            #region If NodeOver is a child then cancel
                            TreeNode tnParadox = NodeOver;
                            while (tnParadox.Parent != null)
                            {
                                if (tnParadox.Parent == NodeMoving)
                                {
                                    this.NodeMap = "";
                                    return;
                                }

                                tnParadox = tnParadox.Parent;
                            }
                            #endregion
                            #region Store the placeholder info into a pipe delimited string
                            SetNewNodeMap(NodeOver, false);
                            NewNodeMap = NewNodeMap.Insert(NewNodeMap.Length, "|0");

                            if (SetMapsEqual() == true)
                                return;
                            #endregion
                            #region Clear placeholders above and below
                            this.Refresh();
                            #endregion
                            #region Draw the "add to folder" placeholder
                            DrawAddToFolderPlaceholder(sender, NodeOver);
                            #endregion
                        }
                    }
                    #endregion
                //}
            }
        }



        #region Helper Methods
        private void DrawLeafTopPlaceholders(object sender, TreeNode NodeOver)
        {
            Graphics g = MainForm.ActiveForm.CreateGraphics();

            int NodeOverImageWidth = _Plugins[NodeOver.Text].GetPluginIcon.Width;
            int LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            int RightPos = ((TreeView)sender).Width - 4;

            Point[] LeftTriangle = new Point[5]{
                                                   new Point(LeftPos, NodeOver.Bounds.Top - 4),
                                                   new Point(LeftPos, NodeOver.Bounds.Top + 4),
                                                   new Point(LeftPos + 4, NodeOver.Bounds.Y),
                                                   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
                                                   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

            Point[] RightTriangle = new Point[5]{
                                                    new Point(RightPos, NodeOver.Bounds.Top - 4),
                                                    new Point(RightPos, NodeOver.Bounds.Top + 4),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Y),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
                                                    new Point(RightPos, NodeOver.Bounds.Top - 5)};


            g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
            g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Top), new Point(RightPos, NodeOver.Bounds.Top));

        }//eom

        private void DrawLeafBottomPlaceholders(object sender, TreeNode NodeOver, TreeNode ParentDragDrop)
        {
            Graphics g = MainForm.ActiveForm.CreateGraphics();

            int NodeOverImageWidth = _Plugins[NodeOver.Text].GetPluginIcon.Width;
            // Once again, we are not dragging to node over, draw the placeholder using the ParentDragDrop bounds
            int LeftPos, RightPos;
            if (ParentDragDrop != null)
                LeftPos = ParentDragDrop.Bounds.Left - (((TreeView)sender).ImageList.Images[ParentDragDrop.ImageIndex].Size.Width + 8);
            else
                LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            RightPos = ((TreeView)sender).Width - 4;

            Point[] LeftTriangle = new Point[5]{
                                                   new Point(LeftPos, NodeOver.Bounds.Bottom - 4),
                                                   new Point(LeftPos, NodeOver.Bounds.Bottom + 4),
                                                   new Point(LeftPos + 4, NodeOver.Bounds.Bottom),
                                                   new Point(LeftPos + 4, NodeOver.Bounds.Bottom - 1),
                                                   new Point(LeftPos, NodeOver.Bounds.Bottom - 5)};

            Point[] RightTriangle = new Point[5]{
                                                    new Point(RightPos, NodeOver.Bounds.Bottom - 4),
                                                    new Point(RightPos, NodeOver.Bounds.Bottom + 4),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Bottom),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Bottom - 1),
                                                    new Point(RightPos, NodeOver.Bounds.Bottom - 5)};


            g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
            g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Bottom), new Point(RightPos, NodeOver.Bounds.Bottom));
        }//eom

        private void DrawFolderTopPlaceholders(object sender, TreeNode NodeOver)
        {
            Graphics g = ((TreeView)sender).CreateGraphics();
            int NodeOverImageWidth = _Plugins[NodeOver.Text].GetPluginIcon.Width;

            int LeftPos, RightPos;
            LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            RightPos = ((TreeView)sender).Width - 4;

            Point[] LeftTriangle = new Point[5]{
                                                   new Point(LeftPos, NodeOver.Bounds.Top - 4),
                                                   new Point(LeftPos, NodeOver.Bounds.Top + 4),
                                                   new Point(LeftPos + 4, NodeOver.Bounds.Y),
                                                   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
                                                   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

            Point[] RightTriangle = new Point[5]{
                                                    new Point(RightPos, NodeOver.Bounds.Top - 4),
                                                    new Point(RightPos, NodeOver.Bounds.Top + 4),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Y),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
                                                    new Point(RightPos, NodeOver.Bounds.Top - 5)};


            g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
            g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Top), new Point(RightPos, NodeOver.Bounds.Top));

        }//eom
        private void DrawAddToFolderPlaceholder(object sender, TreeNode NodeOver)
        {
            Graphics g = ((TreeView)sender).CreateGraphics();
            int RightPos = NodeOver.Bounds.Right + 6;
            Point[] RightTriangle = new Point[5]{
                                                    new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
                                                    new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2)),
                                                    new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 1),
                                                    new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 5)};

            this.Refresh();
            g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
        }//eom

        private void SetNewNodeMap(TreeNode tnNode, bool boolBelowNode)
        {
            NewNodeMap.Length = 0;

            if (boolBelowNode)
                NewNodeMap.Insert(0, (int)tnNode.Index + 1);
            else
                NewNodeMap.Insert(0, (int)tnNode.Index);
            TreeNode tnCurNode = tnNode;

            while (tnCurNode.Parent != null)
            {
                tnCurNode = tnCurNode.Parent;

                if (NewNodeMap.Length == 0 && boolBelowNode == true)
                {
                    NewNodeMap.Insert(0, (tnCurNode.Index + 1) + "|");
                }
                else
                {
                    NewNodeMap.Insert(0, tnCurNode.Index + "|");
                }
            }
        }//oem

        private bool SetMapsEqual()
        {
            if (this.NewNodeMap.ToString() == this.NodeMap)
                return true;
            else
            {
                this.NodeMap = this.NewNodeMap.ToString();
                return false;
            }
        }//oem
        #endregion





    }



}






