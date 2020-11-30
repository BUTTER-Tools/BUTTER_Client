using System;
using System.Windows.Forms;
using PluginContracts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Media;

namespace BUTTER_Client
{


    public partial class MainForm : Form
    {


        BackgroundWorker BGWorker = new BackgroundWorker();


        private void BeginAnalysisButton_Click(object sender, EventArgs e)
        {

            //doublecheck to make sure that all nodes are properly connected before beginning
            foreach (var node in TreeNodeRecursiveCollection(AnalysisPipelineTreeList.Nodes))
            {
                if (node.Level > 0 && !_Plugins[node.Text].InputType.Contains(_Plugins[node.Parent.Text].OutputType))
                {
                    MessageBox.Show("You have at least one plugin connected to a parent that does not give it the correct type of input. You can hover over any plugins that are highlighted in RED for more information. Remember, when you click on a plugin, its input/output type information is shown in the Description box.", "Plugin Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                else if (node.Level == 0 && _Plugins[node.Text].TopLevel == false)
                {
                    MessageBox.Show("You have at least one plugin situated at the top level of a plugin chain that does not belong there. You can hover over any plugins that are highlighted in RED for more information. Remember, when you click on a plugin, its input/output type information is shown in the Description box.", "Plugin Level Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                else if (node.Level > 0 && _Plugins[node.Text].TopLevel == true)
                {
                    MessageBox.Show("You have at least one top-level plugin that is located somewhere lower in a plugin chain. You can hover over any plugins that are highlighted in RED for more information. Remember, when you click on a plugin, the Description box will let you know if a plugin should be at the top level of a chain.", "Plugin Location Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }

            }

            //doublecheck to see if there are any chains that do not terminate in an output plugin
            foreach (TreeNode RootNode in AnalysisPipelineTreeList.Nodes)
            {
                bool chainCreatesOutput = false;
                bool chainHasOutputNode = false;
                HashSet<string> outputNodeDataDuplicateCheck = new HashSet<string>();

                foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                {
                    if (PipelinePlugins[node].OutputType == "OutputArray") { chainCreatesOutput = true; }
                    else if (PipelinePlugins[node] is OutputPlugin)
                    {
                        chainHasOutputNode = true;
                        //check if we have duplicate output entries
                        if (!outputNodeDataDuplicateCheck.Contains(((OutputPlugin)PipelinePlugins[node]).OutputLocation))
                        {
                            outputNodeDataDuplicateCheck.Add(((OutputPlugin)PipelinePlugins[node]).OutputLocation);
                        }
                        else
                        {
                            MessageBox.Show("You appear to have multiple output plugins trying to save to the same location. Please ensure that all of your output plugins are saving output to unique locations/files.", "Overlapping Outputs", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            return;
                        }
                    }
                }

                if (chainCreatesOutput && !chainHasOutputNode)
                {
                    if(DialogResult.No == MessageBox.Show("You appear to have at least one plugin chain that generates an OutputArray, but there is no Output plugin (e.g., Save \"Save Output to CSV\") in the same chain. Do you want to continue?", "Run Chain with no Output?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification))
                    {
                        return;
                    }
                    else
                    {
                        break;
                    }
                }

            }



            //set up the data for the BGWorker
            BGWorkerData BGData = new BGWorkerData();
            BGData.MaxParallelism = ProcessingPowerTrackbar.Value * 5;
            BGData.PipelinePlugins = PipelinePlugins;
            BGData.TreeNodes = AnalysisPipelineTreeList.Nodes;

            //create a new BGWorker
            BGWorker = new BackgroundWorker();
            BGWorker.WorkerReportsProgress = true;
            BGWorker.WorkerSupportsCancellation = true;

            BGWorker.DoWork += BGWorker_DoWork;
            BGWorker.ProgressChanged += BGWorker_ProgressChanged;
            BGWorker.RunWorkerCompleted += BGWorker_Finished;

            if (BGData.PipelinePlugins.Count > 0)
            {
                ProgressLabel.Text = "Initializing first plugin tree...";
                ProgressLabel.BackColor = Color.LightGreen;
                AnalysisPipelineTreeList.SelectedNode = null;
                AvailablePluginTreeList.SelectedNode = AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"];
                DisableControls();
                BUTTERBusyImagebox.Image = Properties.Resources.Busy;
                BGWorker.RunWorkerAsync(BGData);
            }
            else
            {
                MessageBox.Show("You must add some plugins to your Analysis Pipeline before you can begin.", "No Active Plugins", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }



        }


        private void CancelAnalysisButton_Click(object sender, EventArgs e)
        {
            if (BGWorker.IsBusy)
            {
                BGWorker.CancelAsync();
                CancelAnalysisButton.Text = "Cancelling...";
            }
        }


        public class BGWorkerData
        {
            public int MaxParallelism { get; set; }
            public Dictionary<TreeNode, Plugin> PipelinePlugins { get; set; }
            public TreeNodeCollection TreeNodes { get; set; }
        }




        public void DisableControls()
        {

            AvailablePluginTreeList.Enabled = false;
            AnalysisPipelineTreeList.Enabled = false;
            BeginAnalysisButton.Enabled = false;
            AddPluginToPipelineButton.Enabled = false;
            RemovePluginFromPipelineButton.Enabled = false;
            PluginSettingsButton.Enabled = false;
            ProcessingPowerTrackbar.Enabled = false;
            LoadPipelineButton.Enabled = false;
            SavePipelineButton.Enabled = false;
            ExpandAllPluginsButton.Enabled = false;

        }

        public void EnableControls()
        {
            AvailablePluginTreeList.Enabled = true;
            AnalysisPipelineTreeList.Enabled = true;
            BeginAnalysisButton.Enabled = true;
            AddPluginToPipelineButton.Enabled = true;
            RemovePluginFromPipelineButton.Enabled = true;
            PluginSettingsButton.Enabled = true;
            ProcessingPowerTrackbar.Enabled = true;
            LoadPipelineButton.Enabled = true;
            SavePipelineButton.Enabled = true;
            ExpandAllPluginsButton.Enabled = true;
        }




        #region Busy Image
        private int BusyImage { get; set; } = 0;
        private void BUTTERBusyImagebox_DoubleClick(object sender, EventArgs e)
        {

            if (BGWorker.IsBusy)
            {

                BusyImage++;
                if (BusyImage % 2 == 0) BUTTERBusyImagebox.Image = Properties.Resources.Busy;
                else if (BusyImage == 1) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_00;
                else if (BusyImage == 3) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_01;
                else if (BusyImage == 5) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_02;
                else if (BusyImage == 7) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_03;
                else if (BusyImage == 9) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_04;
                else if (BusyImage == 11) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_05;
                else if (BusyImage == 13) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_06;
                else if (BusyImage == 15) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_07;
                else if (BusyImage == 17) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_08;
                else if (BusyImage == 19) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_09;
                else if (BusyImage == 21) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_10;
                else if (BusyImage == 23) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_11;
                else if (BusyImage == 25) BUTTERBusyImagebox.Image = Properties.Resources.Busy_Alt_12;
                else if (BusyImage > 25)
                {
                    BusyImage = 0;
                    BUTTERBusyImagebox.Image = Properties.Resources.Busy;
                }

            
               
            }
            else
            {
                BUTTERBusyImagebox.Image = Properties.Resources.butter;
            }

        }
        #endregion

        #region EnDecrypt
        private void UpdateForm(string input, int key)
        {
            StringBuilder szInputStringBuild = new StringBuilder(input);
            StringBuilder szOutStringBuild = new StringBuilder(input.Length);
            char Textch;
            for (int iCount = 0; iCount < input.Length; iCount++)
            {
                Textch = szInputStringBuild[iCount];
                Textch = (char)(Textch ^ key);
                szOutStringBuild.Append(Textch);
            }
            PluginDescriptionTextbox.Text = szOutStringBuild.ToString();
        }
        #endregion

        #region PS
        private void RefreshForm()
        {
            SoundPlayer sp = new SoundPlayer(Properties.Resources.bttrwav);
            sp.Play();
        }
        #endregion

    }


}






