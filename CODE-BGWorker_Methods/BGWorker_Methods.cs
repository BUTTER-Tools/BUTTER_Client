using System;
using System.Windows.Forms;
using PluginContracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using OutputHelperLib;
using TSOutputWriter;
using System.IO;
using System.Drawing;
using System.Linq;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {



        private void BGWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            //First, we take what was passed to us as an argument and make it more readable
            BGWorkerData BGData = (BGWorkerData)e.Argument;
            TimeSpan reportPeriod = TimeSpan.FromMinutes(0.01);

            //we going to use this to report the "UserStatus" to the UI thread during the reportprogress bits
            int CurrentTopLevelNodeNumber = 0;

            int ParallelismPower = 1;
            if ((int)System.Math.Floor((double)(Environment.ProcessorCount * ((double)BGData.MaxParallelism / 100.0))) > 1)
                ParallelismPower = (int)System.Math.Floor((double)(Environment.ProcessorCount * ((double)BGData.MaxParallelism / 100.0)));


            #region Initial Check of ALL settings
            BGWorker.ReportProgress(0, "Inspecting settings for entire pipeline...");
            bool settingsPass = true;
            //before we do anything, let's inspect all settings:
            //first, we start by iterating over the top-level nodes one at a time
            foreach (TreeNode RootNode in BGData.TreeNodes)
            {
                
                if (!((Plugin)PipelinePlugins[RootNode]).InspectSettings())
                {
                    settingsPass = false;
                    MessageBox.Show("The following plugin appears to have invalid settings:" + Environment.NewLine +
                        ((Plugin)PipelinePlugins[RootNode]).PluginName + Environment.NewLine +
                        "Please check the settings for this plugin to make sure that it is properly configured.",
                        "Plugin Failed Inspection", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                 }

                foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                {
                    if (!((Plugin)PipelinePlugins[node]).InspectSettings())
                    {
                        settingsPass = false;
                        MessageBox.Show("The following plugin appears to have invalid settings:" + Environment.NewLine +
                        ((Plugin)PipelinePlugins[node]).PluginName + Environment.NewLine +
                        "Please check the settings for this plugin to make sure that it is properly configured.",
                        "Plugin Failed Inspection", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    }
                }

            }

            if (!settingsPass)
            {
                e.Cancel = true;
                return;
            }
            #endregion






            //first, we start by iterating over the top-level nodes one at a time
            foreach (TreeNode RootNode in BGData.TreeNodes)
            {

                CurrentTopLevelNodeNumber += 1;

                if (BGWorker.CancellationPending) break;

                //for input nodes, we first initialize so that they are set up with the number of texts
                //to be analyzed. this will be used for reporting progress later on.
                //but we only do that if they pass the plugin's internal inspection.
                if (((Plugin)PipelinePlugins[RootNode]).InspectSettings())
                {
                    BGWorker.ReportProgress(0, "Initializing Plugin Chain #" + CurrentTopLevelNodeNumber.ToString() + " (" + ((Plugin)PipelinePlugins[RootNode]).PluginName + ")");
                    ((Plugin)PipelinePlugins[RootNode]).Initialize();
                }
                else
                {
                    MessageBox.Show("The following plugin appears to have invalid settings:" + Environment.NewLine +
                        ((Plugin)PipelinePlugins[RootNode]).PluginName + Environment.NewLine +
                        "Please check the settings for this plugin to make sure that it is properly configured.",
                        "Plugin Failed Inspection", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    e.Cancel = true;
                    break;
                }

                




                //this keeps track of the active text being processed
                //is incremented within the parallel.foreach loop
                int TextCount = 0;


                //initialize each input stream
                #region Prep and Open Input Readers that need to remain open
                if ((PipelinePlugins[RootNode] is InputPlugin) &&
                    (((InputPlugin)PipelinePlugins[RootNode]).KeepStreamOpen) && 
                    (!string.IsNullOrWhiteSpace(((InputPlugin)PipelinePlugins[RootNode]).IncomingTextLocation)))
                {
                    ((InputPlugin)PipelinePlugins[RootNode]).InputStream =
                                                                new StreamReader(File.OpenRead(((InputPlugin)PipelinePlugins[RootNode]).IncomingTextLocation),
                                                                encoding: Encoding.GetEncoding(((InputPlugin)PipelinePlugins[RootNode]).SelectedEncoding));
                }
                #endregion

                 #region Prep and Open Output Writers that need to remain open
                 //setting up our writers before we do anything else
                foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                {

                    //open streams
                    if ((PipelinePlugins[node] is OutputPlugin) &&
                        (((OutputPlugin)PipelinePlugins[node]).KeepStreamOpen) &&
                        (!string.IsNullOrWhiteSpace(((OutputPlugin)PipelinePlugins[node]).OutputLocation))
                        )
                    {

                        try
                        { 
                        ((OutputPlugin)PipelinePlugins[node]).headerWritten = false;
                        ((OutputPlugin)PipelinePlugins[node]).Writer = new ThreadsafeOutputWriter(((OutputPlugin)PipelinePlugins[node]).OutputLocation,
                                                                                                      Encoding.GetEncoding(((OutputPlugin)PipelinePlugins[node]).SelectedEncoding),
                                                                                                      ((OutputPlugin)PipelinePlugins[node]).fileMode);
                        }
                        catch
                        {
                            MessageBox.Show("There was a problem initializing one of your Output Writer plugins:"  + Environment.NewLine +
                            ((Plugin)PipelinePlugins[node]).PluginName + Environment.NewLine + Environment.NewLine +
                            "Most often, this issue is caused by the file still being open in another application. In some cases, this can also be caused by a failed Analysis Pipeline. If you are absolutely sure that your output file is not open in another program, try closing and reopening BUTTER. You may want to save your analysis pipeline beforehand to make things easier to set up after reopening.",
                            "Plugin Failed Initialization", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                            e.Cancel = true;
                            break;
                        }


                    }


                    if (BGWorker.CancellationPending) return;

                    //header inheritance
                    if (((Plugin)PipelinePlugins[node]).InheritHeader) ((Plugin)PipelinePlugins[node]).OutputHeaderData = ((Plugin)PipelinePlugins[node.Parent]).OutputHeaderData;




                    //make sure to initialize all plugins
                    if (((Plugin)PipelinePlugins[node]).InspectSettings())
                    {
                        BGWorker.ReportProgress(0, "Initializing Plugin Chain #" + CurrentTopLevelNodeNumber.ToString() + " (" + ((Plugin)PipelinePlugins[node]).PluginName + ")");
                        ((Plugin)PipelinePlugins[node]).Initialize();
                    }
                    else
                    {
                        MessageBox.Show("The following plugin appears to have invalid settings:" + Environment.NewLine +
                            ((Plugin)PipelinePlugins[node]).PluginName + Environment.NewLine +
                            "Please check the settings for this plugin to make sure that it is properly configured.",
                            "Plugin Failed Inspection", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        e.Cancel = true;
                        return;
                    }

                    
                    //as a note, this initialization is also where headers get written

                }
                #endregion


                if (BGWorker.CancellationPending) return;



                //  _   _  ___  ____  __  __    _    _       ____ ___ ____  _____ _     ___ _   _ _____ 
                // | \ | |/ _ \|  _ \|  \/  |  / \  | |     |  _ \_ _|  _ \| ____| |   |_ _| \ | | ____|
                // |  \| | | | | |_) | |\/| | / _ \ | |     | |_) | || |_) |  _| | |    | ||  \| |  _|  
                // | |\  | |_| |  _ <| |  | |/ ___ \| |___  |  __/| ||  __/| |___| |___ | || |\  | |___ 
                // |_| \_|\___/|_| \_\_|  |_/_/   \_\_____| |_|  |___|_|   |_____|_____|___|_| \_|_____|
                //                                                                                      

                #region Standard Input Plugins
                if (BGData.PipelinePlugins[RootNode] is InputPlugin)
                {




                    #region What to do if root node is indeed a TextInputPlugin
                    


                    //use this time to report the worker's progress
                    using (new System.Threading.Timer(
                            _ => BGWorker.ReportProgress(
                                 (int)Math.Ceiling(((TextCount - 1) / (double)((InputPlugin)BGData.PipelinePlugins[RootNode]).TextCount) * 10000),
                                 "Running Plugin Chain #" + CurrentTopLevelNodeNumber.ToString()),
                                 null, reportPeriod, reportPeriod)) { 






                        Parallel.ForEach((IEnumerable<object>)((InputPlugin)BGData.PipelinePlugins[RootNode]).TextEnumeration(),
                        new ParallelOptions { MaxDegreeOfParallelism = ParallelismPower }, (TextInputItem, state) =>
                        {

                            if (BGWorker.CancellationPending) state.Stop();

                            //set up the data for this item
                            Dictionary<TreeNode, Payload> data_store = new Dictionary<TreeNode, Payload>();
                            data_store.Add(RootNode, new Payload());
                            foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes)) data_store.Add(node, new Payload());

                            //increment the counter for the file that we're working on
                            data_store[RootNode].TextNumber = Interlocked.Increment(ref TextCount);
                            //make sure to pass the initial object that we're going to read
                            data_store[RootNode].ObjectList.Add(TextInputItem);

                            //Make sure to actually grab the text. If there's nothing to grab, it just moves forward with nothing
                            try
                            {
                                data_store[RootNode] = ((InputPlugin)BGData.PipelinePlugins[RootNode]).RunPlugin(data_store[RootNode].DeepCopy());
                            }
                            catch
                            {
                                data_store[RootNode] = new Payload();
                            }

                            //now that the first "input" node has been handled, we've got to make sure that we
                            //pass along the output type
                            data_store[RootNode].Type = BGData.PipelinePlugins[RootNode].OutputType;

                            // now we iterate over all child nodes
                            foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                            {

                                if (BGWorker.CancellationPending) state.Stop();

                                //loop through each node, pass the data as necessary,
                                //but don't run the node if the hand-shaking process doesn't match
                                if (BGData.PipelinePlugins[node].InputType.Contains(BGData.PipelinePlugins[node.Parent].OutputType))
                                {

                                    data_store[node] = BGData.PipelinePlugins[node].RunPlugin(data_store[node.Parent].DeepCopy());
                                    data_store[node].Type = BGData.PipelinePlugins[node].OutputType;

                                }
                                    


                            }



                            #endregion

                        });

                    }
                }
                #endregion

                //  _     ___ _   _ _____    _    ____    ____ ___ ____  _____ _     ___ _   _ _____ 
                // | |   |_ _| \ | | ____|  / \  |  _ \  |  _ \_ _|  _ \| ____| |   |_ _| \ | | ____|
                // | |    | ||  \| |  _|   / _ \ | |_) | | |_) | || |_) |  _| | |    | ||  \| |  _|  
                // | |___ | || |\  | |___ / ___ \|  _ <  |  __/| ||  __/| |___| |___ | || |\  | |___ 
                // |_____|___|_| \_|_____/_/   \_\_| \_\ |_|  |___|_|   |_____|_____|___|_| \_|_____|
                //                                                                                   

                else if (BGData.PipelinePlugins[RootNode] is LinearPlugin)
                {
                    Dictionary<TreeNode, Payload> data_store = new Dictionary<TreeNode, Payload>();
                    data_store.Add(RootNode, new Payload());
                    foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes)) data_store.Add(node, new Payload());






                    //run the top level root node here
                    using (new System.Threading.Timer(
                            _ => BGWorker.ReportProgress(
                                 0, // percent
                                 "[Chain #" + CurrentTopLevelNodeNumber.ToString() + "] | " + 
                                 ((LinearPlugin)BGData.PipelinePlugins[RootNode]).PluginName + " | " + 
                                 ((LinearPlugin)BGData.PipelinePlugins[RootNode]).StatusToReport),
                                 null, reportPeriod, reportPeriod))
                    {

                        try
                        {
                            data_store[RootNode] = ((LinearPlugin)BGData.PipelinePlugins[RootNode]).RunPlugin(data_store[RootNode].DeepCopy(), ParallelismPower);
                        }
                        catch
                        {
                            data_store[RootNode] = new Payload();
                        }


                    }

                        




                    
                    data_store[RootNode].Type = BGData.PipelinePlugins[RootNode].OutputType;

                    foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                    {

                        if (BGWorker.CancellationPending) break;

                        //loop through each node, pass the data as necessary,
                        //but don't run the node if the hand-shaking process doesn't match
                        if (BGData.PipelinePlugins[node].InputType.Contains(BGData.PipelinePlugins[node.Parent].OutputType))
                        {
                            data_store[node] = BGData.PipelinePlugins[node].RunPlugin(data_store[node.Parent].DeepCopy());
                            data_store[node].Type = BGData.PipelinePlugins[node].OutputType;
                        }
                    }
                }




                //  _____ ___ _   _ ___ ____  _   _ _____ ____  
                // |  ___|_ _| \ | |_ _/ ___|| | | | ____|  _ \ 
                // | |_   | ||  \| || |\___ \| |_| |  _| | | | |
                // |  _|  | || |\  || | ___) |  _  | |___| |_| |
                // |_|   |___|_| \_|___|____/|_| |_|_____|____/ 
                //                                              
                //loop through each node and run its "Finalize" method
                Payload finalPayload = BGData.PipelinePlugins[RootNode].FinishUp(new Payload()); //pass the root node an empty payload; it shouldn't need anything
                finalPayload.Type = BGData.PipelinePlugins[RootNode].OutputType;

                foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                {
                    if (BGWorker.CancellationPending) break;
                    BGWorker.ReportProgress(0, "Finalizing Plugin Chain #" + CurrentTopLevelNodeNumber.ToString() + " (" + ((Plugin)PipelinePlugins[node]).PluginName + ")");
                    finalPayload = BGData.PipelinePlugins[node].FinishUp(finalPayload);
                    //make sure that the payload "type" property is maintained
                    finalPayload.Type = BGData.PipelinePlugins[node].OutputType;
                }


                //we report 10000 because the progresschanged method will divide by 100,
                //so, we're effective reporting 100%
                BGWorker.ReportProgress(10000, CurrentTopLevelNodeNumber);

                #region Dispose of all Input Readers and Output Writers that were of the "KeepStreamOpen" persuasion
                //setting up our writers before we do anything else
                foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                {
                    if ((PipelinePlugins[node] is OutputPlugin) && 
                        (((OutputPlugin)PipelinePlugins[node]).KeepStreamOpen) &&
                        (!string.IsNullOrWhiteSpace(((OutputPlugin)PipelinePlugins[node]).OutputLocation)))
                    {
                        ((OutputPlugin)PipelinePlugins[node]).Writer.Dispose();
                    }
                }

                if ((PipelinePlugins[RootNode] is InputPlugin) && 
                    (((InputPlugin)PipelinePlugins[RootNode]).KeepStreamOpen) &&
                    (!string.IsNullOrWhiteSpace(((InputPlugin)PipelinePlugins[RootNode]).IncomingTextLocation)))
                        try { ((InputPlugin)PipelinePlugins[RootNode]).InputStream.Dispose(); } catch { }

                #endregion

                if (BGWorker.CancellationPending) break;

            }



        }





        private void BGWorker_Finished(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            BGWorker = new System.ComponentModel.BackgroundWorker();
            ProgressLabel.Text = "[Ready]";
            ProgressLabel.BackColor = SystemColors.ControlDark;
            CancelAnalysisButton.Text = "Cancel";
            BUTTERBusyImagebox.Image = Properties.Resources.butter;
            EnableControls();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Refresh();
            Application.DoEvents();
            MessageBox.Show("BUTTER has finished running your Analysis Pipeline.", "Finished!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }



        private void BGWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

            if (e.UserState.ToString().Contains("Running Plugin Chain #"))
            {
                ProgressLabel.Text = e.UserState.ToString() + ": " +
                                    string.Format("{0:0.00}", (e.ProgressPercentage / (double)100)) + "%";
            }
            else
            {
                ProgressLabel.Text = e.UserState.ToString();
            }

            
        }






    }


}






