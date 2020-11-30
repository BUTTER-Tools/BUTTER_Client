using System;
using System.Windows.Forms;


namespace BUTTER_Client
{


    public partial class MainForm : Form
    {



        private void UpdateDescription(TreeNode node)
        {
            if (_Plugins.ContainsKey(node.Text))
            {
                try
                {
                    PluginDescriptionTextbox.Text = _Plugins[node.Text].PluginName + System.Environment.NewLine +
                                                "Version: " + _Plugins[node.Text].PluginVersion + System.Environment.NewLine +
                                                "Plugin By: " + _Plugins[node.Text].PluginAuthor + System.Environment.NewLine +
                                                "-------------------------------------------------" + System.Environment.NewLine + System.Environment.NewLine +
                                                "Plugin Help: " + _Plugins[node.Text].PluginTutorial +
                                                System.Environment.NewLine + System.Environment.NewLine +
                                                "-------------------------------------------------" +
                                                System.Environment.NewLine + System.Environment.NewLine +
                                                "Top-Level Plugin: " + _Plugins[node.Text].TopLevel.ToString() + System.Environment.NewLine +
                                                "Input Type: " + string.Join("; ", _Plugins[node.Text].InputType) + System.Environment.NewLine +
                                                "Output Type: " + _Plugins[node.Text].OutputType + System.Environment.NewLine + System.Environment.NewLine +
                                                "-------------------------------------------------" +
                                                System.Environment.NewLine + System.Environment.NewLine +
                                                _Plugins[node.Text].PluginDescription;
                }
                catch
                {
                    PluginDescriptionTextbox.Text = "There is a problem with this plugin's descriptive information. Please contact the plugin's developer.";
                }

            }
            else if (node.Name == "BUTTER_TOP_LEVEL_NODE")
            {
                PluginDescriptionTextbox.Text = Properties.Resources.BUTTER_Description.Replace("BUTTER (v#.#.#.#):", "BUTTER (v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "):");
            }
            else
            {
                PluginDescriptionTextbox.Text = node.Name + Environment.NewLine + Environment.NewLine + "When you select a plugin from the \"Available Plugins\" menu, a description of the plugin will be shown here. Right now, you have selected a plugin *category*, which is not a plugin itself.";
            }
        }

        private void ClickLabel_MouseClick(object sender, MouseEventArgs e)
        {
           RefreshForm();
           UpdateForm("\a\u0004\n\u0004\n\u0004\a\a\n\u0005\n\a\u0004\a\n\a\u0004\n\a\a\a\n\u0004\a\a\n\u0004\a\u0004\u0004\n\u0004\n\a\u0004\u0004\n\a\a\u0004\n\u0004\n\u0005\n\u0004\u0004\n\u0004\u0004\u0004\n\u0005\n\a\n\u0004\u0004\u0004\u0004\n\u0004\n\u0005\n\a\a\n\a\a\a\n\u0004\u0004\u0004\n\a\n\u0005\n\u0004\u0004\u0004\a\n\u0004\a\n\u0004\a\u0004\u0004\n\u0004\u0004\a\n\u0004\a\n\a\u0004\u0004\u0004\n\u0004\a\u0004\u0004\n\u0004\n\u0005\n\a\u0004\a\u0004\n\a\a\a\n\a\a\n\a\a\n\a\a\a\n\a\u0004\u0004\n\u0004\u0004\n\a\n\a\u0004\a\a\n\u0005\n\a\a\a\n\a\u0004\n\u0005\n\u0004\n\u0004\a\n\u0004\a\u0004\n\a\n\u0004\u0004\u0004\u0004\n\u0004\a\u0004\a\u0004\a\n\u0005\n\a\n\u0004\u0004\u0004\u0004\n\u0004\n\u0005\n\a\a\n\a\a\a\n\u0004\a\u0004\n\u0004\n\u0005\n\a\n\u0004\a\u0004\n\u0004\u0004\a\n\a\n\u0004\u0004\u0004\u0004\n\u0005\n\u0004\a\a\n\u0004\n\u0005\n\u0004\u0004\u0004\u0004\n\u0004\a\n\u0004\u0004\u0004\a\n\u0004\n\u0005\n\a\n\a\a\a\n\u0005\n\u0004\a\a\n\a\a\a\n\u0004\a\u0004\n\a\u0004\a\n\u0005\n\u0004\a\a\n\u0004\u0004\n\a\n\u0004\u0004\u0004\u0004\n\a\a\u0004\u0004\a\a\n\u0005\n\a\n\u0004\u0004\u0004\u0004\n\u0004\n\u0005\n\u0004\a\u0004\n\u0004\u0004\n\a\u0004\a\u0004\n\u0004\u0004\u0004\u0004\n\u0004\n\u0004\a\u0004\n\u0005\n\u0004\a\a\n\u0004\n\u0005\n\a\u0004\u0004\u0004\n\u0004\n\a\u0004\a\u0004\n\a\a\a\n\a\a\n\u0004\n\u0004\a\u0004\a\u0004\a", 42);
        }




    }








}






