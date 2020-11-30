//using System;
//using System.Windows.Forms;
//using System.Collections.Generic;
//using PluginContracts;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using System.Xml.Linq;
//using System.Linq;
//using System.Xml.Serialization;

//namespace BUTTER_Client
//{


//    public partial class MainForm : Form
//    {

//        //Right now, none of this actually works, so we're going to just leave it alone for time being.
//        private void SavePipelineButton_Click(object sender, EventArgs e)
//        {


//            if (AnalysisPipelineTreeList.Nodes.Count > 0)
//            {



//                using (var dialog = new SaveFileDialog())
//                {
//                    dialog.Title = "Please choose the output location for your Analysis Pipeline";
//                    dialog.FileName = "BUTTER Pipeline.btr";
//                    dialog.Filter = "BUTTER Pipeline File (*.btr)|*.btr";
//                    if (dialog.ShowDialog() == DialogResult.OK)
//                    {

//                        PipelineSave pipelineSave = new PipelineSave(AnalysisPipelineTreeList, PipelinePlugins);

//                        SaveTree(pipelineSave, dialog.FileName);
//                    }
//                }


//            }
//            else
//            {
//                MessageBox.Show("You do not have any plugins in your Analysis Pipeline to save.", "No Pipeline to Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }


//        }




//        private void LoadPipelineButton_Click(object sender, EventArgs e)
//        {

//            using (var dialog = new OpenFileDialog())
//            {
//                //wipeout the dictionary of plugins
//                PipelinePlugins = new Dictionary<TreeNode, Plugin>();

//                dialog.Title = "Please choose the your Analysis Pipeline file";
//                dialog.FileName = "BUTTER Pipeline.btr";
//                dialog.Filter = "BUTTER Pipeline File (*.btr)|*.btr";
//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    LoadTree(AnalysisPipelineTreeList, dialog.FileName);
//                }

                
                
//                //Rebuild our dictionary of plugins
//                foreach (TreeNode RootNode in AnalysisPipelineTreeList.Nodes)
//                {
//                    Plugin TempPlugin = (Plugin)Activator.CreateInstance(_Plugins[RootNode.Text].GetType());
//                    PipelinePlugins.Add(RootNode, TempPlugin);
//                    foreach (TreeNode node in TreeNodeRecursiveCollection(RootNode.Nodes))
//                    {
//                        TempPlugin = (Plugin)Activator.CreateInstance(_Plugins[node.Text].GetType());
//                        PipelinePlugins.Add(node, TempPlugin);

//                    }
//                }

//            }

//        }








//        //https://stackoverflow.com/a/5868931
//        public static void SaveTree(PipelineSave pipelineSave, string filename)
//        {
//            using (Stream file = File.Open(filename, FileMode.Create))
//            {
                
//                BinaryFormatter bf = new BinaryFormatter();
//                bf.Serialize(file, pipelineSave);
//            }
//        }





//        public void LoadTree(BufferedTreeView tree, string filename)
//        {
            
//            using (Stream file = File.Open(filename, FileMode.Open))
//            {
//                BinaryFormatter bf = new BinaryFormatter();
//                PipelineSave obj = (PipelineSave)bf.Deserialize(file);

//                TreeNode[] nodeList = (obj.tree as IEnumerable<TreeNode>).ToArray();
//                tree.Nodes.AddRange(nodeList);
//            }

//            tree.ExpandAll();

//        }




//        [Serializable()]
//        public class PipelineSave
//        {
//            public List<TreeNode> tree { get; set; }
//            //serializing each of the plugins will probably prove very, very difficult
//            //Dictionary<TreeNode, Plugin> plugins { get; set; }

//            public PipelineSave(BufferedTreeView tree_in, Dictionary<TreeNode, Plugin> dict_in)
//            {
//                tree = tree_in.Nodes.Cast<TreeNode>().ToList();
//                //plugins = dict_in;
//            }
            

//        }



      






//    }

//}





