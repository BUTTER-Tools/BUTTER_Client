namespace BUTTER_Client
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.AvailablePluginsLabel = new System.Windows.Forms.Label();
            this.AnalysisPipelineLabel = new System.Windows.Forms.Label();
            this.PluginDescriptionTextbox = new System.Windows.Forms.TextBox();
            this.PluginDescriptionLabel = new System.Windows.Forms.Label();
            this.AddPluginToPipelineButton = new System.Windows.Forms.Button();
            this.RemovePluginFromPipelineButton = new System.Windows.Forms.Button();
            this.PluginSettingsButton = new System.Windows.Forms.Button();
            this.BeginAnalysisButton = new System.Windows.Forms.Button();
            this.CancelAnalysisButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ExpandAllPluginsButton = new System.Windows.Forms.Button();
            this.AnalysisPipelineTreeList = new BUTTER_Client.BufferedTreeView();
            this.AvailablePluginTreeList = new BUTTER_Client.BufferedTreeView();
            this.LoadPipelineButton = new System.Windows.Forms.Button();
            this.SavePipelineButton = new System.Windows.Forms.Button();
            this.BUTTERBusyImagebox = new System.Windows.Forms.PictureBox();
            this.ProcessingPowerTrackbar = new System.Windows.Forms.TrackBar();
            this.ProcessingPowerLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.ClickLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BUTTERBusyImagebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessingPowerTrackbar)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // AvailablePluginsLabel
            // 
            this.AvailablePluginsLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AvailablePluginsLabel.Location = new System.Drawing.Point(16, 30);
            this.AvailablePluginsLabel.Name = "AvailablePluginsLabel";
            this.AvailablePluginsLabel.Size = new System.Drawing.Size(128, 19);
            this.AvailablePluginsLabel.TabIndex = 2;
            this.AvailablePluginsLabel.Text = "Available Plugins";
            // 
            // AnalysisPipelineLabel
            // 
            this.AnalysisPipelineLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnalysisPipelineLabel.Location = new System.Drawing.Point(430, 30);
            this.AnalysisPipelineLabel.Name = "AnalysisPipelineLabel";
            this.AnalysisPipelineLabel.Size = new System.Drawing.Size(127, 19);
            this.AnalysisPipelineLabel.TabIndex = 3;
            this.AnalysisPipelineLabel.Text = "Analysis Pipeline";
            // 
            // PluginDescriptionTextbox
            // 
            this.PluginDescriptionTextbox.BackColor = System.Drawing.SystemColors.Info;
            this.PluginDescriptionTextbox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PluginDescriptionTextbox.Location = new System.Drawing.Point(26, 39);
            this.PluginDescriptionTextbox.MaxLength = 2147483647;
            this.PluginDescriptionTextbox.Multiline = true;
            this.PluginDescriptionTextbox.Name = "PluginDescriptionTextbox";
            this.PluginDescriptionTextbox.ReadOnly = true;
            this.PluginDescriptionTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PluginDescriptionTextbox.Size = new System.Drawing.Size(377, 679);
            this.PluginDescriptionTextbox.TabIndex = 999;
            this.PluginDescriptionTextbox.TabStop = false;
            this.PluginDescriptionTextbox.Text = resources.GetString("PluginDescriptionTextbox.Text");
            // 
            // PluginDescriptionLabel
            // 
            this.PluginDescriptionLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PluginDescriptionLabel.Location = new System.Drawing.Point(142, 18);
            this.PluginDescriptionLabel.Name = "PluginDescriptionLabel";
            this.PluginDescriptionLabel.Size = new System.Drawing.Size(138, 19);
            this.PluginDescriptionLabel.TabIndex = 5;
            this.PluginDescriptionLabel.Text = "Plugin Description";
            // 
            // AddPluginToPipelineButton
            // 
            this.AddPluginToPipelineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddPluginToPipelineButton.Location = new System.Drawing.Point(230, 518);
            this.AddPluginToPipelineButton.Name = "AddPluginToPipelineButton";
            this.AddPluginToPipelineButton.Size = new System.Drawing.Size(129, 45);
            this.AddPluginToPipelineButton.TabIndex = 4;
            this.AddPluginToPipelineButton.Text = "Add Plugin \r\nto Pipeline";
            this.AddPluginToPipelineButton.UseVisualStyleBackColor = true;
            this.AddPluginToPipelineButton.Click += new System.EventHandler(this.AddPluginToPipelineButton_Click);
            this.AddPluginToPipelineButton.MouseHover += new System.EventHandler(this.AddPluginToPipelineButton_MouseHover);
            // 
            // RemovePluginFromPipelineButton
            // 
            this.RemovePluginFromPipelineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemovePluginFromPipelineButton.Location = new System.Drawing.Point(659, 518);
            this.RemovePluginFromPipelineButton.Name = "RemovePluginFromPipelineButton";
            this.RemovePluginFromPipelineButton.Size = new System.Drawing.Size(115, 45);
            this.RemovePluginFromPipelineButton.TabIndex = 6;
            this.RemovePluginFromPipelineButton.Text = "Remove Plugin from Pipeline";
            this.RemovePluginFromPipelineButton.UseVisualStyleBackColor = true;
            this.RemovePluginFromPipelineButton.Click += new System.EventHandler(this.RemovePluginFromPipelineButton_Click);
            this.RemovePluginFromPipelineButton.MouseHover += new System.EventHandler(this.RemovePluginFromPipelineButton_MouseHover);
            // 
            // PluginSettingsButton
            // 
            this.PluginSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PluginSettingsButton.Location = new System.Drawing.Point(489, 518);
            this.PluginSettingsButton.Name = "PluginSettingsButton";
            this.PluginSettingsButton.Size = new System.Drawing.Size(115, 45);
            this.PluginSettingsButton.TabIndex = 5;
            this.PluginSettingsButton.Text = "Plugin Settings";
            this.PluginSettingsButton.UseVisualStyleBackColor = true;
            this.PluginSettingsButton.Click += new System.EventHandler(this.PluginSettingsButton_Click);
            this.PluginSettingsButton.MouseHover += new System.EventHandler(this.PluginSettingsButton_MouseHover);
            // 
            // BeginAnalysisButton
            // 
            this.BeginAnalysisButton.BackColor = System.Drawing.Color.LightGreen;
            this.BeginAnalysisButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BeginAnalysisButton.Location = new System.Drawing.Point(887, 655);
            this.BeginAnalysisButton.Name = "BeginAnalysisButton";
            this.BeginAnalysisButton.Size = new System.Drawing.Size(115, 45);
            this.BeginAnalysisButton.TabIndex = 30;
            this.BeginAnalysisButton.Text = "Begin Analysis";
            this.BeginAnalysisButton.UseVisualStyleBackColor = false;
            this.BeginAnalysisButton.Click += new System.EventHandler(this.BeginAnalysisButton_Click);
            this.BeginAnalysisButton.MouseHover += new System.EventHandler(this.BeginAnalysisButton_MouseHover);
            // 
            // CancelAnalysisButton
            // 
            this.CancelAnalysisButton.BackColor = System.Drawing.Color.LightCoral;
            this.CancelAnalysisButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelAnalysisButton.Location = new System.Drawing.Point(1042, 655);
            this.CancelAnalysisButton.Name = "CancelAnalysisButton";
            this.CancelAnalysisButton.Size = new System.Drawing.Size(115, 45);
            this.CancelAnalysisButton.TabIndex = 31;
            this.CancelAnalysisButton.Text = "Cancel";
            this.CancelAnalysisButton.UseVisualStyleBackColor = false;
            this.CancelAnalysisButton.Click += new System.EventHandler(this.CancelAnalysisButton_Click);
            this.CancelAnalysisButton.MouseHover += new System.EventHandler(this.CancelAnalysisButton_MouseHover);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ExpandAllPluginsButton);
            this.panel1.Controls.Add(this.AnalysisPipelineTreeList);
            this.panel1.Controls.Add(this.AvailablePluginTreeList);
            this.panel1.Controls.Add(this.LoadPipelineButton);
            this.panel1.Controls.Add(this.SavePipelineButton);
            this.panel1.Controls.Add(this.RemovePluginFromPipelineButton);
            this.panel1.Controls.Add(this.AnalysisPipelineLabel);
            this.panel1.Controls.Add(this.PluginSettingsButton);
            this.panel1.Controls.Add(this.AvailablePluginsLabel);
            this.panel1.Controls.Add(this.AddPluginToPipelineButton);
            this.panel1.Location = new System.Drawing.Point(435, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(838, 591);
            this.panel1.TabIndex = 0;
            // 
            // ExpandAllPluginsButton
            // 
            this.ExpandAllPluginsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExpandAllPluginsButton.Location = new System.Drawing.Point(60, 518);
            this.ExpandAllPluginsButton.Name = "ExpandAllPluginsButton";
            this.ExpandAllPluginsButton.Size = new System.Drawing.Size(115, 45);
            this.ExpandAllPluginsButton.TabIndex = 3;
            this.ExpandAllPluginsButton.Text = "Expand All";
            this.ExpandAllPluginsButton.UseVisualStyleBackColor = true;
            this.ExpandAllPluginsButton.Click += new System.EventHandler(this.ExpandAllPluginsButton_Click);
            this.ExpandAllPluginsButton.MouseHover += new System.EventHandler(this.ExpandAllPluginsButton_MouseHover);
            // 
            // AnalysisPipelineTreeList
            // 
            this.AnalysisPipelineTreeList.AllowDrop = true;
            this.AnalysisPipelineTreeList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AnalysisPipelineTreeList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.AnalysisPipelineTreeList.Font = new System.Drawing.Font("Corbel", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnalysisPipelineTreeList.HideSelection = false;
            this.AnalysisPipelineTreeList.Location = new System.Drawing.Point(424, 52);
            this.AnalysisPipelineTreeList.Name = "AnalysisPipelineTreeList";
            this.AnalysisPipelineTreeList.ShowNodeToolTips = true;
            this.AnalysisPipelineTreeList.Size = new System.Drawing.Size(400, 453);
            this.AnalysisPipelineTreeList.TabIndex = 2;
            this.AnalysisPipelineTreeList.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.AnalysisPipelineTreeList_BeforeCollapse);
            this.AnalysisPipelineTreeList.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.PluginsToRunTreeList_DrawNode);
            this.AnalysisPipelineTreeList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.AnalysisPipelineTreeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AnalysisPipelineTreeList_AfterSelect);
            this.AnalysisPipelineTreeList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.AnalysisPipelineTreeList_NodeMouseClick);
            this.AnalysisPipelineTreeList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.AnalysisPipelineTreeList_NodeMouseDoubleClick);
            this.AnalysisPipelineTreeList.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.AnalysisPipelineTreeList.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.AnalysisPipelineTreeList.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            this.AnalysisPipelineTreeList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            this.AnalysisPipelineTreeList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AnalysisPipelineTreeList_MouseUp);
            // 
            // AvailablePluginTreeList
            // 
            this.AvailablePluginTreeList.Font = new System.Drawing.Font("Corbel", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AvailablePluginTreeList.HideSelection = false;
            this.AvailablePluginTreeList.Location = new System.Drawing.Point(10, 52);
            this.AvailablePluginTreeList.Name = "AvailablePluginTreeList";
            this.AvailablePluginTreeList.Size = new System.Drawing.Size(400, 453);
            this.AvailablePluginTreeList.TabIndex = 1;
            this.AvailablePluginTreeList.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.AvailablePluginTreeList_DrawNode);
            this.AvailablePluginTreeList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AvailablePluginTreeList_AfterSelect);
            this.AvailablePluginTreeList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.AvailablePluginTreeList_NodeMouseClick);
            this.AvailablePluginTreeList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.AvailablePluginTreeList_NodeMouseDoubleClick);
            this.AvailablePluginTreeList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AvailablePluginTreeList_MouseUp);
            // 
            // LoadPipelineButton
            // 
            this.LoadPipelineButton.Location = new System.Drawing.Point(663, 23);
            this.LoadPipelineButton.Name = "LoadPipelineButton";
            this.LoadPipelineButton.Size = new System.Drawing.Size(75, 23);
            this.LoadPipelineButton.TabIndex = 10;
            this.LoadPipelineButton.TabStop = false;
            this.LoadPipelineButton.Text = "Load";
            this.LoadPipelineButton.UseVisualStyleBackColor = true;
            this.LoadPipelineButton.Click += new System.EventHandler(this.LoadPipelineButton_Click);
            this.LoadPipelineButton.MouseHover += new System.EventHandler(this.LoadPipelineButton_MouseHover);
            // 
            // SavePipelineButton
            // 
            this.SavePipelineButton.Location = new System.Drawing.Point(744, 23);
            this.SavePipelineButton.Name = "SavePipelineButton";
            this.SavePipelineButton.Size = new System.Drawing.Size(75, 23);
            this.SavePipelineButton.TabIndex = 11;
            this.SavePipelineButton.TabStop = false;
            this.SavePipelineButton.Text = "Save";
            this.SavePipelineButton.UseVisualStyleBackColor = true;
            this.SavePipelineButton.Click += new System.EventHandler(this.SavePipelineButton_Click);
            this.SavePipelineButton.MouseHover += new System.EventHandler(this.SavePipelineButton_MouseHover);
            // 
            // BUTTERBusyImagebox
            // 
            this.BUTTERBusyImagebox.BackColor = System.Drawing.Color.Transparent;
            this.BUTTERBusyImagebox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BUTTERBusyImagebox.Image = global::BUTTER_Client.Properties.Resources.butter;
            this.BUTTERBusyImagebox.Location = new System.Drawing.Point(1209, 645);
            this.BUTTERBusyImagebox.Name = "BUTTERBusyImagebox";
            this.BUTTERBusyImagebox.Size = new System.Drawing.Size(64, 64);
            this.BUTTERBusyImagebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BUTTERBusyImagebox.TabIndex = 12;
            this.BUTTERBusyImagebox.TabStop = false;
            this.BUTTERBusyImagebox.DoubleClick += new System.EventHandler(this.BUTTERBusyImagebox_DoubleClick);
            // 
            // ProcessingPowerTrackbar
            // 
            this.ProcessingPowerTrackbar.BackColor = System.Drawing.Color.DimGray;
            this.ProcessingPowerTrackbar.LargeChange = 2;
            this.ProcessingPowerTrackbar.Location = new System.Drawing.Point(3, 33);
            this.ProcessingPowerTrackbar.Maximum = 20;
            this.ProcessingPowerTrackbar.Minimum = 1;
            this.ProcessingPowerTrackbar.Name = "ProcessingPowerTrackbar";
            this.ProcessingPowerTrackbar.Size = new System.Drawing.Size(369, 45);
            this.ProcessingPowerTrackbar.TabIndex = 20;
            this.ProcessingPowerTrackbar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ProcessingPowerTrackbar.Value = 15;
            this.ProcessingPowerTrackbar.ValueChanged += new System.EventHandler(this.ProcessingPowerTrackbar_ValueChanged);
            // 
            // ProcessingPowerLabel
            // 
            this.ProcessingPowerLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProcessingPowerLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessingPowerLabel.Location = new System.Drawing.Point(70, 8);
            this.ProcessingPowerLabel.Name = "ProcessingPowerLabel";
            this.ProcessingPowerLabel.Size = new System.Drawing.Size(236, 22);
            this.ProcessingPowerLabel.TabIndex = 11;
            this.ProcessingPowerLabel.Text = "Processing Power:";
            this.ProcessingPowerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ProcessingPowerLabel);
            this.panel2.Controls.Add(this.ProcessingPowerTrackbar);
            this.panel2.Location = new System.Drawing.Point(457, 636);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(377, 82);
            this.panel2.TabIndex = 1004;
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ProgressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgressLabel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgressLabel.Location = new System.Drawing.Point(26, 741);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(1247, 23);
            this.ProgressLabel.TabIndex = 1005;
            this.ProgressLabel.Text = "Ready";
            this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClickLabel
            // 
            this.ClickLabel.AutoSize = true;
            this.ClickLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.ClickLabel.Location = new System.Drawing.Point(-3, 763);
            this.ClickLabel.Name = "ClickLabel";
            this.ClickLabel.Size = new System.Drawing.Size(14, 13);
            this.ClickLabel.TabIndex = 1006;
            this.ClickLabel.Text = "X";
            this.ClickLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ClickLabel_MouseClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 773);
            this.Controls.Add(this.ClickLabel);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.BUTTERBusyImagebox);
            this.Controls.Add(this.CancelAnalysisButton);
            this.Controls.Add(this.BeginAnalysisButton);
            this.Controls.Add(this.PluginDescriptionLabel);
            this.Controls.Add(this.PluginDescriptionTextbox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1317, 812);
            this.MinimumSize = new System.Drawing.Size(1278, 718);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BUTTER Solutions";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BUTTERBusyImagebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessingPowerTrackbar)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label AvailablePluginsLabel;
        private System.Windows.Forms.Label AnalysisPipelineLabel;
        private System.Windows.Forms.TextBox PluginDescriptionTextbox;
        private System.Windows.Forms.Label PluginDescriptionLabel;
        private System.Windows.Forms.Button AddPluginToPipelineButton;
        private System.Windows.Forms.Button RemovePluginFromPipelineButton;
        private System.Windows.Forms.Button PluginSettingsButton;
        private System.Windows.Forms.Button BeginAnalysisButton;
        private System.Windows.Forms.Button CancelAnalysisButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button LoadPipelineButton;
        private System.Windows.Forms.Button SavePipelineButton;
        private System.Windows.Forms.PictureBox BUTTERBusyImagebox;
        private System.Windows.Forms.TrackBar ProcessingPowerTrackbar;
        private System.Windows.Forms.Label ProcessingPowerLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label ProgressLabel;
        private BufferedTreeView AvailablePluginTreeList;
        private BufferedTreeView AnalysisPipelineTreeList;
        private System.Windows.Forms.Button ExpandAllPluginsButton;
        private System.Windows.Forms.Label ClickLabel;
    }
}

