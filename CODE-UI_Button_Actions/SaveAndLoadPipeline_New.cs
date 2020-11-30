using System;
using System.Windows.Forms;
using System.Collections.Generic;
using PluginContracts;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace BUTTER_Client
{


    public partial class MainForm : Form
    {

        //Right now, none of this actually works, so we're going to just leave it alone for time being.
        private void SavePipelineButton_Click(object sender, EventArgs e)
        {


            if (AnalysisPipelineTreeList.Nodes.Count > 0)
            {



                using (var dialog = new SaveFileDialog())
                {
                    dialog.Title = "Please choose the output location for your Analysis Pipeline";
                    dialog.FileName = "BUTTER Pipeline.btr";
                    dialog.Filter = "BUTTER Pipeline File (*.btr)|*.btr";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {

                        List<PipelineStorageObject> PipelineExport = new List<PipelineStorageObject>();
                        PipelineExport.Add(new PipelineStorageObject("BUTTER", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                                                                      null, null, true, null));

                        int NodeCount = 0;

                        //iterate over all root notes in the pipeline
                        foreach (TreeNode RootNode in AnalysisPipelineTreeList.Nodes)
                        {
                            NodeCount++;
                            RootNode.Tag = NodeCount.ToString();
                            PipelineExport.Add(PipelineStorageObjBuilder(RootNode, true, NodeCount.ToString()));

                            foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                            {
                                NodeCount++;
                                node.Tag = NodeCount.ToString();
                                PipelineExport.Add(PipelineStorageObjBuilder(node, false, NodeCount.ToString()));
                            }

                        }

                        SaveTree(PipelineExport, dialog.FileName);

                    }
                }


            }
            else
            {
                MessageBox.Show("You do not have any plugins in your Analysis Pipeline to save.", "No Pipeline to Save", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }


        }




        private void LoadPipelineButton_Click(object sender, EventArgs e)
        {

            if (AnalysisPipelineTreeList.Nodes.Count > 0)
            {
                DialogResult res = MessageBox.Show("Your current Analysis Pipeline will be cleared when you load an Analysis Pipeline from a file. Are you sure that you want to continue?", "Clear Existing Pipeline?",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

                if (res == DialogResult.No) return;

            }
            using (var dialog = new OpenFileDialog())
            {

                dialog.Title = "Please choose the your Analysis Pipeline file";
                dialog.FileName = "BUTTER Pipeline.btr";
                dialog.Filter = "BUTTER Pipeline File (*.btr)|*.btr";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadTree(AnalysisPipelineTreeList, dialog.FileName);
                }
            }
        }








        //https://stackoverflow.com/a/5868931
        public static void SaveTree(List<PipelineStorageObject> pipelineSave, string filename)
        {

            try {

                //https://stackoverflow.com/a/2340953
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.NewLineHandling = NewLineHandling.Entitize;
                ws.Encoding = System.Text.Encoding.Unicode;
                ws.NewLineChars = Environment.NewLine;
                ws.Indent = true;
                ws.IndentChars = "\t";
                ws.NewLineOnAttributes = true;
                ws.CloseOutput = true;


                XmlSerializer serializer = new XmlSerializer(typeof(List<PipelineStorageObject>));
                //using (TextWriter writer = new StreamWriter(
                //                                            new FileStream(filename, FileMode.Create, FileAccess.Write), System.Text.Encoding.Unicode)
                //                                            )
                //

                using (XmlWriter writer = XmlWriter.Create(new FileStream(filename, FileMode.Create, FileAccess.Write), ws))
                {
                    serializer.Serialize(writer, pipelineSave);
                }
                MessageBox.Show("Your pipeline has been saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
            catch
            {
                MessageBox.Show("There was a problem saving your pipeline. Is your pipeline file being used by another application?", "Problem Saving Pipeline", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            

        }





        public void LoadTree(BufferedTreeView tree, string filename)
        {

            //wipe the existing pipeline
            PipelinePlugins = new Dictionary<TreeNode, Plugin>();
            tree.Nodes.Clear();

            

            //set up the object that we're importing the data into
            List<PipelineStorageObject> pipelineSave = new List<PipelineStorageObject>();

            //here, we deserialize the pipeline
            XmlSerializer serializer = new XmlSerializer(typeof(List<PipelineStorageObject>));


            //XmlReaderSettings rs = new XmlReaderSettings();
            try
            {
                using (TextReader reader = new StreamReader(
                                                            new FileStream(filename, FileMode.Open, FileAccess.Read), System.Text.Encoding.Unicode)
                                                            )
                {
                    pipelineSave = (List<PipelineStorageObject>)serializer.Deserialize(reader);
                }
            }
            catch
            {
                MessageBox.Show("There was a problem reading/deserializing your pipeline file. Is the file being used by another application?", "Problem Opening Pipeline", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }



            //now we iterate over the list of nodes/plugins within the deserialized pipeline
            foreach (PipelineStorageObject pipelineNode in pipelineSave)
            {

                if (pipelineNode.NodeParentID == null) continue;

                //find a copy of the plugin from the available plugins to start
                //Right now, we're looking in the "available plugins" to create copies and rebuild the pipeline from the save file.
                //This is a "kind of smart" solution here -- it makes sure that the plugin actually exists and is loaded up by the software before
                //trying to add it into the pipeline. Future revisions will include a lot of error handling around this.
                TreeNode[] treenodesearch = AvailablePluginTreeList.Nodes.Find(pipelineNode.PluginName, true);


                TreeNode TempNode;
                Plugin TempPlugin;
                //if it exists, then we clone it
                try
                { 
                    TempNode = (TreeNode)treenodesearch[0].Clone();
                    TempPlugin = (Plugin)Activator.CreateInstance(_Plugins[pipelineNode.PluginName].GetType());
                }
                //if it doesn't we add in the error node
                catch
                {
                    TempNode = new TreeNode("Error");
                    TempNode.ImageKey = "Error";
                    TempNode.SelectedImageKey = "Error";
                    TempPlugin = (Plugin)Activator.CreateInstance(_Plugins["Error"].GetType());
                }
                //and we make sure to tag it with the NodeID so that it's easier to add its children later
                TempNode.Tag = pipelineNode.NodeID;
                //lastly, create a new instance of the plugin class that will be associated with this specific node
                
                

                //add in the node. if it's a root node, then we just add it to the top
                if (pipelineNode.RootNode)
                {
                    tree.SelectedNode = null;
                    tree.Nodes.Add(TempNode);
                    tree.SelectedNode = TempNode;
                }
                //if it's not a root node, things are a bit more complicated
                else
                {
                    //we go recursively crawl through each node and find the parent node
                    foreach (TreeNode RootNode in tree.Nodes)
                    {
                        //if we find the appropriate parent (based on the tag), we append it
                        if ((string)RootNode.Tag == pipelineNode.NodeParentID)
                        {
                            tree.SelectedNode = RootNode;
                            tree.SelectedNode.Nodes.Add(TempNode);
                            tree.SelectedNode = TempNode;
                            break;
                        }

                        foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
                        {
                            if ((string)node.Tag == pipelineNode.NodeParentID)
                            {
                                //again, if we find the appropriate parent (based on the tag), we append it
                                tree.SelectedNode = node;
                                tree.SelectedNode.Nodes.Add(TempNode);
                                tree.SelectedNode = TempNode;
                                break;
                            }
                        }
                    }

                }

                //make sure we update our dictionary to keep track of the plugins/nodes
                PipelinePlugins.Add(TempNode, TempPlugin);


                //we take the deserialized settings and repack them as a dictionary<string,string>
                Dictionary<string, string> NodeSettings = new Dictionary<string, string>();
                foreach (XElement el in pipelineNode.Settings.Elements())
                {
                    NodeSettings.Add(el.Name.LocalName, el.Value);
                }

                try
                {
                    //then we pass the dictionary back to the plugin itself to rehydrate the settings in whatever way they want.
                    PipelinePlugins[tree.SelectedNode].ImportSettings(NodeSettings);
                }
                catch
                {

                    string ErrMsg = "There was an error restoring the settings for the following plugin:" + Environment.NewLine + "\t" +
                                    pipelineNode.NodeID + ": " + pipelineNode.PluginName + Environment.NewLine + Environment.NewLine + 
                                    "The default settings for this plugin will be used for those settings where problems occurred.";


                    try
                    {
                  
                        if (pipelineNode.PluginVersion != _Plugins[pipelineNode.PluginName].PluginVersion)
                        {
                            ErrMsg += Environment.NewLine + Environment.NewLine + "It appears that the version of the plugin (" + pipelineNode.PluginVersion + ") that was used during the creation of your original pipeline " +
                                "does not match the version currently installed with BUTTER (" + _Plugins[pipelineNode.PluginVersion].PluginVersion + "). It is possible that the settings are incompatible between these two " +
                                "versions of the plugin. Please contact the plugin's author for additional troubleshooting.";
                        }
                    }
                    catch
                    {

                    }


                    MessageBox.Show(ErrMsg, "Error Restoring Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
                

            }

            //lastly, we expand the pipeline tree so that it's visible in all of its glory
            tree.ExpandAll();

        }
        





        private PipelineStorageObject PipelineStorageObjBuilder(TreeNode node, bool IsRoot, string NodeID)
        {

            string ParentNodeID = "none";
            if (!IsRoot)
            {
                ParentNodeID = (string)node.Parent.Tag;

            }

            PipelineStorageObject PipelineObj = new PipelineStorageObject(NameIn: PipelinePlugins[node].PluginName,
                                                                          VersionIn: PipelinePlugins[node].PluginVersion,
                                                                          NodeIDin: NodeID,
                                                                          NodeParentIDin: ParentNodeID,
                                                                          IsRoot: IsRoot,
                                                                          SettingsIn: PipelinePlugins[node].ExportSettings(suppressWarnings: false));

            return (PipelineObj);


        }





        [Serializable]
        public class PipelineStorageObject
        {
            public string PluginName { get; set; }
            public string PluginVersion { get; set; }
            public string NodeID { get; set; }
            public string NodeParentID { get; set; }
            public bool RootNode { get; set; }
            public XElement Settings { get; set; }


            public PipelineStorageObject(string NameIn, string VersionIn, string NodeIDin, string NodeParentIDin, bool IsRoot, Dictionary<string, string> SettingsIn)
            {
                this.PluginName = NameIn;
                this.PluginVersion = VersionIn;
                this.NodeID = NodeIDin;
                this.RootNode = IsRoot;
                this.NodeParentID = NodeParentIDin;
                //https://stackoverflow.com/a/1799792
                if (SettingsIn != null) this.Settings = new XElement("Settings", SettingsIn.Select(kv => new XElement(kv.Key, kv.Value)));
            }

            //must have a parameterless constructor in order to xmlserialize it
            internal PipelineStorageObject()
            {

            }


        }


        



    }



















}





