using ScriptGenie.Controls;

namespace ScriptGenie
{
    partial class scriptGenieMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(scriptGenieMain));
            scriptGenieMain_splitDisplay = new SplitDisplay();
            scriptGenieMain_splitDisplayPanel1_menuStrip = new MenuStrip();
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator = new ToolStripMenuItem();
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator = new ToolStripComboBox();
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server = new ToolStripMenuItem();
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server = new ToolStripComboBox();
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)scriptGenieMain_splitDisplay).BeginInit();
            scriptGenieMain_splitDisplay.Panel1.SuspendLayout();
            scriptGenieMain_splitDisplay.SuspendLayout();
            scriptGenieMain_splitDisplayPanel1_menuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // scriptGenieMain_splitDisplay
            // 
            scriptGenieMain_splitDisplay.BorderStyle = BorderStyle.FixedSingle;
            scriptGenieMain_splitDisplay.Dock = DockStyle.Fill;
            scriptGenieMain_splitDisplay.Location = new Point(0, 0);
            scriptGenieMain_splitDisplay.Name = "scriptGenieMain_splitDisplay";
            scriptGenieMain_splitDisplay.Orientation = Orientation.Horizontal;
            // 
            // scriptGenieMain_splitDisplay.Panel1
            // 
            scriptGenieMain_splitDisplay.Panel1.Controls.Add(scriptGenieMain_splitDisplayPanel1_menuStrip);
            // 
            // scriptGenieMain_splitDisplay.Panel2
            // 
            scriptGenieMain_splitDisplay.Panel2.BackgroundImage = Properties.Resources.bkd_001;
            scriptGenieMain_splitDisplay.Panel2.BackgroundImageLayout = ImageLayout.Stretch;
            scriptGenieMain_splitDisplay.Size = new Size(953, 603);
            scriptGenieMain_splitDisplay.SplitterDistance = 33;
            scriptGenieMain_splitDisplay.SplitterWidth = 6;
            scriptGenieMain_splitDisplay.TabIndex = 0;
            // 
            // scriptGenieMain_splitDisplayPanel1_menuStrip
            // 
            scriptGenieMain_splitDisplayPanel1_menuStrip.Font = new Font("Segoe UI", 11F);
            scriptGenieMain_splitDisplayPanel1_menuStrip.ImageScalingSize = new Size(25, 25);
            scriptGenieMain_splitDisplayPanel1_menuStrip.Items.AddRange(new ToolStripItem[] { scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator, scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator, scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server, scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server, scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export });
            scriptGenieMain_splitDisplayPanel1_menuStrip.Location = new Point(0, 0);
            scriptGenieMain_splitDisplayPanel1_menuStrip.Name = "scriptGenieMain_splitDisplayPanel1_menuStrip";
            scriptGenieMain_splitDisplayPanel1_menuStrip.Size = new Size(951, 28);
            scriptGenieMain_splitDisplayPanel1_menuStrip.TabIndex = 0;
            scriptGenieMain_splitDisplayPanel1_menuStrip.Text = "menuStrip1";
            // 
            // scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator
            // 
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator.Name = "scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator";
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator.Size = new Size(90, 24);
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator.Text = "Generator:";
            // 
            // scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator
            // 
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.Margin = new Padding(1, 0, 10, 0);
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.Name = "scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator";
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.Size = new Size(121, 24);
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator.SelectedIndexChanged += scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator_SelectedIndexChanged;
            // 
            // scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server
            // 
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server.Name = "scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server";
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server.Size = new Size(65, 24);
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server.Text = "Server:";
            // 
            // scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server
            // 
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.Margin = new Padding(1, 0, 10, 0);
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.Name = "scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server";
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.Size = new Size(121, 24);
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server.SelectedIndexChanged += scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server_SelectedIndexChanged;
            // 
            // scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export
            // 
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export.Name = "scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export";
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export.Size = new Size(64, 24);
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export.Text = "Export";
            scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export.Click += scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Segoe UI", 11F);
            statusStrip1.ImageScalingSize = new Size(25, 25);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 578);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(953, 25);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(216, 20);
            toolStripStatusLabel1.Text = "Version 1.0.0.0  |  Build 111725a";
            // 
            // scriptGenieMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(953, 603);
            Controls.Add(statusStrip1);
            Controls.Add(scriptGenieMain_splitDisplay);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = scriptGenieMain_splitDisplayPanel1_menuStrip;
            MaximizeBox = false;
            Name = "scriptGenieMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Script Genie";
            Load += scriptGenieMain_Load;
            scriptGenieMain_splitDisplay.Panel1.ResumeLayout(false);
            scriptGenieMain_splitDisplay.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)scriptGenieMain_splitDisplay).EndInit();
            scriptGenieMain_splitDisplay.ResumeLayout(false);
            scriptGenieMain_splitDisplayPanel1_menuStrip.ResumeLayout(false);
            scriptGenieMain_splitDisplayPanel1_menuStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitDisplay scriptGenieMain_splitDisplay;
        private MenuStrip scriptGenieMain_splitDisplayPanel1_menuStrip;
        private ToolStripMenuItem scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_generator;
        private ToolStripComboBox scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_generator;
        private ToolStripMenuItem scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripLabel_server;
        private ToolStripComboBox scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripComboBox_server;
        private ToolStripMenuItem scriptGenieMain_splitDisplayPanel1_menuStrip_menuStripButton_export;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}