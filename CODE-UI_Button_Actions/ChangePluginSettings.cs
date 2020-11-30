using System;
using System.Windows.Forms;

namespace BUTTER_Client
{


    public partial class MainForm : Form
    {

                              

        private void PluginSettingsButton_Click(object sender, EventArgs e)
        {

            if (AnalysisPipelineTreeList.SelectedNode != null)
            {
               
                PipelinePlugins[AnalysisPipelineTreeList.SelectedNode].ChangeSettings();

            }
        }

        

    }



}






