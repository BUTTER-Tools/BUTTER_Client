using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {

        private void InitializePluginTree()
        {
            List<Dictionary<string, string>> PluginDetailsList = new List<Dictionary<string, string>>();

            //make sure that the top-level plugin node exists and has its image set
            AvailablePluginTreeList.Nodes.Add("BUTTER_TOP_LEVEL_NODE", "BUTTER");
            AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Expand();
            AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].NodeFont = new System.Drawing.Font(AvailablePluginTreeList.Font, System.Drawing.FontStyle.Underline);


            PluginImageList.ImageSize = new Size(18, 18);

            //also, make sure that the top-level icon is set
            PluginImageList.Images.Add("BUTTER", Properties.Resources.butter_icon);
            AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].ImageKey = "BUTTER";
            AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].SelectedImageKey = "BUTTER";

            //and, lastly, make sure that the plugin category icon is loaded
            PluginImageList.Images.Add("PluginCategory", Properties.Resources.plug);
            PluginImageList.Images.Add("PluginClassification", Properties.Resources.plugin_category);


        }


    }








}






