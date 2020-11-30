using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PluginContracts;
using PluginLoader;
using System.Drawing;




namespace BUTTER_Client
{


    

    public partial class MainForm : Form
    {
                        
        Dictionary<string, Plugin> _Plugins;

        ImageList PluginImageList = new ImageList();
        Dictionary<TreeNode, Plugin> PipelinePlugins;

        //context menu for when someone right-clicks a node in the analysis pipeline
        ContextMenuStrip AnalysisPipelineContextMenu = new ContextMenuStrip();
        ContextMenuStrip AvailablePluginsContextMenu = new ContextMenuStrip();


        public MainForm()
        {
            InitializeComponent();


            //https://stackoverflow.com/a/12445083
            float widthRatio = Screen.PrimaryScreen.Bounds.Width / 1920f;
            float heightRatio = Screen.PrimaryScreen.Bounds.Height / 1080f;
            SizeF scale = new SizeF(widthRatio, heightRatio);
            this.Scale(scale);
            foreach (Control control in this.Controls)
            {
                control.Font = new Font(control.Font.FontFamily, (float)System.Math.Floor(control.Font.SizeInPoints * ((widthRatio + heightRatio) / 2)));
                foreach (Control subcontrol in control.Controls) subcontrol.Font = new Font(subcontrol.Font.FontFamily, (float)System.Math.Floor(subcontrol.Font.SizeInPoints * ((widthRatio + heightRatio) / 2)));

            }




            ProgressLabel.Text = "[Ready]";

            //just make sure the label is set right from the start
            ProcessingPowerLabel.Text = "Processing Power:  " + (ProcessingPowerTrackbar.Value * 5).ToString() + "%";


            //initialize the tree list plugin
            InitializePluginTree();

            ClickLabel.ForeColor = MainForm.DefaultBackColor;

            //load up / list the plugins in the tree view

            //populate our list of plugins
            _Plugins = new Dictionary<string, Plugin>();
            PipelinePlugins = new Dictionary<TreeNode, Plugin>();


            ICollection<Plugin> plugins = GenericPluginLoader<Plugin>.LoadPlugins("Plugins");
            foreach (var item in plugins)
            {

                if (!_Plugins.ContainsKey(item.PluginName))
                {
                                   
                    _Plugins.Add(item.PluginName, item);

                    //we want to omit this specific plugin type from the available plugins list. it won't hurt to have it
                    //there, but it might just add some confusion for the end user.
                    if (item.PluginType != "Error")
                    {
                        PopulatePluginTreeList(item, item.GetPluginIcon);
                    }
                    else
                    {
                        PluginImageList.Images.Add(item.PluginName, item.GetPluginIcon);
                    }

                }

            }

            //make sure that the tree is expanded
            //AvailablePluginTreeList.ExpandAll();
            //make sure that the images for each plugin / node are set
            AvailablePluginTreeList.ImageList = PluginImageList;
            //also, make sure that the pipeline treelist has the same imagekey
            AnalysisPipelineTreeList.ImageList = PluginImageList;

            //add items to the available plugin context menu
            AvailablePluginsContextMenu.Items.Clear();
            AvailablePluginsContextMenu.Items.Add("Add to Pipeline");
            AvailablePluginsContextMenu.Items.Add("Plugin Help");
            AvailablePluginsContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(AvailablePluginContextMenu_ItemClicked);

            //add items to the analysis pipeline context menu
            AnalysisPipelineContextMenu.Items.Clear();
            AnalysisPipelineContextMenu.Items.Add("Copy Plugin(s)");
            AnalysisPipelineContextMenu.Items.Add("Paste Plugin(s)");
            AnalysisPipelineContextMenu.Items.Add("Plugin Settings");
            AnalysisPipelineContextMenu.Items.Add("Plugin Help");
            AnalysisPipelineContextMenu.Items.Add("Remove Plugin");
            AnalysisPipelineContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(AnalysisPipelineContextMenu_ItemClicked);



        }





        private void ProcessingPowerTrackbar_ValueChanged(object sender, EventArgs e)
        {
            ProcessingPowerLabel.Text = "Processing Power:  " + (ProcessingPowerTrackbar.Value * 5).ToString() + "%";
        }


        private void ExpandAllPluginsButton_Click(object sender, EventArgs e)
        {
            AvailablePluginTreeList.ExpandAll();
            AvailablePluginTreeList.Nodes[0].EnsureVisible();
        }

        

        private void BeginAnalysisButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(BeginAnalysisButton.Text, BeginAnalysisButton);
        }

        private void CancelAnalysisButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(CancelAnalysisButton.Text, CancelAnalysisButton);
        }

        private void LoadPipelineButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(LoadPipelineButton.Text, LoadPipelineButton);
        }

        private void SavePipelineButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(SavePipelineButton.Text, SavePipelineButton);
        }

        private void ExpandAllPluginsButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(ExpandAllPluginsButton.Text, ExpandAllPluginsButton);
        }

        private void AddPluginToPipelineButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(AddPluginToPipelineButton.Text, AddPluginToPipelineButton);
        }

        private void PluginSettingsButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(PluginSettingsButton.Text, PluginSettingsButton);
        }

        private void RemovePluginFromPipelineButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            tip.Show(RemovePluginFromPipelineButton.Text, RemovePluginFromPipelineButton);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.TopMost = true;
            this.TopMost = false;
        }
    }



    

}
