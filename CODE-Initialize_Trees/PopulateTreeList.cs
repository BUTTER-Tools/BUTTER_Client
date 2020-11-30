using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PluginContracts;


namespace BUTTER_Client
{

    

    public partial class MainForm : Form
    {


        private void PopulatePluginTreeList(Plugin plugin, Icon PluginIcon)
        {

            //making separate categories for stand-alone and progressive types of plugins.
            //mostly useful to help users see the difference between the two so that they
            //can more easily understand which ones get chained together versus not
            string pluginClassification = "Sequential Plugins";
            if (plugin is LinearPlugin) pluginClassification = "Stand-Alone Plugins";

            if (!AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes.ContainsKey(pluginClassification))
            {
                AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes.Add(pluginClassification, pluginClassification,
                                                                    "PluginClassification", "PluginClassification");
                AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes[pluginClassification].NodeFont =
                    new System.Drawing.Font(AvailablePluginTreeList.Font, System.Drawing.FontStyle.Underline | FontStyle.Bold);
            }







            if (AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes[pluginClassification].Nodes.ContainsKey(plugin.PluginType))
            {

                //and make sure that the image is loaded in for that plugin
                PluginImageList.Images.Add(plugin.PluginName, PluginIcon);

                //if the plugin type already has a node, we just assign the new plugin to that plugin type
                AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes[pluginClassification]
                    .Nodes[plugin.PluginType]
                    .Nodes.Add(plugin.PluginName, plugin.PluginName,
                    plugin.PluginName, plugin.PluginName);




                
            }
            else
            {
                //if the plugin type doesn't already exist, then we first have to make a node for that type,
                //and we have to make sure that it uses our "umbrella" icon
                AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes[pluginClassification].Nodes.Add(plugin.PluginType, "Category: " + plugin.PluginType,
                                                                    "PluginCategory", "PluginCategory");

                AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes[pluginClassification].Nodes[plugin.PluginType].NodeFont = 
                    new System.Drawing.Font(AvailablePluginTreeList.Font, System.Drawing.FontStyle.Underline | FontStyle.Bold);


                PluginImageList.Images.Add(plugin.PluginName, PluginIcon);

                AvailablePluginTreeList.Nodes["BUTTER_TOP_LEVEL_NODE"].Nodes[pluginClassification]
                    .Nodes[plugin.PluginType]
                    .Nodes.Add(plugin.PluginName, plugin.PluginName,
                                plugin.PluginName, plugin.PluginName);


            }


            


        }


    }
    

}
