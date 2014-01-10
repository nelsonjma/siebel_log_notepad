namespace SiebelLogNotepad
{
    partial class FormNotepad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNotepad));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonChangeTree = new System.Windows.Forms.Button();
            this.buttonCollapse = new System.Windows.Forms.Button();
            this.buttonExpandTree = new System.Windows.Forms.Button();
            this.buttonMarkAndFind = new System.Windows.Forms.Button();
            this.buttonRefreshTree = new System.Windows.Forms.Button();
            this.buttonStatistics = new System.Windows.Forms.Button();
            this.buttonIgnore = new System.Windows.Forms.Button();
            this.buttonOpenConfig = new System.Windows.Forms.Button();
            this.buttonOpenLog = new System.Windows.Forms.Button();
            this.splitContainerTreeTextBox = new System.Windows.Forms.SplitContainer();
            this.panelTreeView = new System.Windows.Forms.Panel();
            this.treeViewSiebelTree = new System.Windows.Forms.TreeView();
            this.panelTextBox = new System.Windows.Forms.Panel();
            this.buttonGoToLine = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeTextBox)).BeginInit();
            this.splitContainerTreeTextBox.Panel1.SuspendLayout();
            this.splitContainerTreeTextBox.Panel2.SuspendLayout();
            this.splitContainerTreeTextBox.SuspendLayout();
            this.panelTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.IsSplitterFixed = true;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.panelButtons);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerTreeTextBox);
            this.splitContainerMain.Size = new System.Drawing.Size(982, 558);
            this.splitContainerMain.SplitterDistance = 29;
            this.splitContainerMain.SplitterWidth = 1;
            this.splitContainerMain.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonGoToLine);
            this.panelButtons.Controls.Add(this.buttonChangeTree);
            this.panelButtons.Controls.Add(this.buttonCollapse);
            this.panelButtons.Controls.Add(this.buttonExpandTree);
            this.panelButtons.Controls.Add(this.buttonMarkAndFind);
            this.panelButtons.Controls.Add(this.buttonRefreshTree);
            this.panelButtons.Controls.Add(this.buttonStatistics);
            this.panelButtons.Controls.Add(this.buttonIgnore);
            this.panelButtons.Controls.Add(this.buttonOpenConfig);
            this.panelButtons.Controls.Add(this.buttonOpenLog);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(982, 29);
            this.panelButtons.TabIndex = 0;
            // 
            // buttonChangeTree
            // 
            this.buttonChangeTree.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonChangeTree.Image = ((System.Drawing.Image)(resources.GetObject("buttonChangeTree.Image")));
            this.buttonChangeTree.Location = new System.Drawing.Point(244, 2);
            this.buttonChangeTree.Name = "buttonChangeTree";
            this.buttonChangeTree.Size = new System.Drawing.Size(24, 24);
            this.buttonChangeTree.TabIndex = 8;
            this.buttonChangeTree.UseVisualStyleBackColor = true;
            // 
            // buttonCollapse
            // 
            this.buttonCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCollapse.Image = global::SiebelLogNotepad.Resources.collapse;
            this.buttonCollapse.Location = new System.Drawing.Point(214, 2);
            this.buttonCollapse.Name = "buttonCollapse";
            this.buttonCollapse.Size = new System.Drawing.Size(24, 24);
            this.buttonCollapse.TabIndex = 7;
            this.buttonCollapse.UseVisualStyleBackColor = true;
            this.buttonCollapse.Click += new System.EventHandler(this.buttonCollapse_Click);
            // 
            // buttonExpandTree
            // 
            this.buttonExpandTree.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonExpandTree.Image = global::SiebelLogNotepad.Resources.expand;
            this.buttonExpandTree.Location = new System.Drawing.Point(184, 2);
            this.buttonExpandTree.Name = "buttonExpandTree";
            this.buttonExpandTree.Size = new System.Drawing.Size(24, 24);
            this.buttonExpandTree.TabIndex = 6;
            this.buttonExpandTree.UseVisualStyleBackColor = true;
            this.buttonExpandTree.Click += new System.EventHandler(this.buttonExpandTree_Click);
            // 
            // buttonMarkAndFind
            // 
            this.buttonMarkAndFind.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonMarkAndFind.Image = global::SiebelLogNotepad.Resources.find_in_tree;
            this.buttonMarkAndFind.Location = new System.Drawing.Point(154, 2);
            this.buttonMarkAndFind.Name = "buttonMarkAndFind";
            this.buttonMarkAndFind.Size = new System.Drawing.Size(24, 24);
            this.buttonMarkAndFind.TabIndex = 5;
            this.buttonMarkAndFind.UseVisualStyleBackColor = true;
            this.buttonMarkAndFind.Click += new System.EventHandler(this.buttonMarkAndFind_Click);
            // 
            // buttonRefreshTree
            // 
            this.buttonRefreshTree.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRefreshTree.Image = global::SiebelLogNotepad.Resources.refresh;
            this.buttonRefreshTree.Location = new System.Drawing.Point(124, 2);
            this.buttonRefreshTree.Name = "buttonRefreshTree";
            this.buttonRefreshTree.Size = new System.Drawing.Size(24, 24);
            this.buttonRefreshTree.TabIndex = 4;
            this.buttonRefreshTree.UseVisualStyleBackColor = true;
            this.buttonRefreshTree.Click += new System.EventHandler(this.buttonRefreshTree_Click);
            // 
            // buttonStatistics
            // 
            this.buttonStatistics.Enabled = false;
            this.buttonStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonStatistics.Image = global::SiebelLogNotepad.Resources.statistics;
            this.buttonStatistics.Location = new System.Drawing.Point(94, 2);
            this.buttonStatistics.Name = "buttonStatistics";
            this.buttonStatistics.Size = new System.Drawing.Size(24, 24);
            this.buttonStatistics.TabIndex = 3;
            this.buttonStatistics.UseVisualStyleBackColor = true;
            // 
            // buttonIgnore
            // 
            this.buttonIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonIgnore.Image = global::SiebelLogNotepad.Resources.ignore;
            this.buttonIgnore.Location = new System.Drawing.Point(64, 2);
            this.buttonIgnore.Name = "buttonIgnore";
            this.buttonIgnore.Size = new System.Drawing.Size(24, 24);
            this.buttonIgnore.TabIndex = 2;
            this.buttonIgnore.UseVisualStyleBackColor = true;
            this.buttonIgnore.Click += new System.EventHandler(this.buttonIgnore_Click);
            // 
            // buttonOpenConfig
            // 
            this.buttonOpenConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOpenConfig.Image = global::SiebelLogNotepad.Resources.open_config;
            this.buttonOpenConfig.Location = new System.Drawing.Point(34, 2);
            this.buttonOpenConfig.Name = "buttonOpenConfig";
            this.buttonOpenConfig.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenConfig.TabIndex = 1;
            this.buttonOpenConfig.UseVisualStyleBackColor = true;
            this.buttonOpenConfig.Click += new System.EventHandler(this.buttonOpenConfig_Click);
            // 
            // buttonOpenLog
            // 
            this.buttonOpenLog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOpenLog.Image = global::SiebelLogNotepad.Resources.open_log;
            this.buttonOpenLog.Location = new System.Drawing.Point(4, 2);
            this.buttonOpenLog.Name = "buttonOpenLog";
            this.buttonOpenLog.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenLog.TabIndex = 0;
            this.buttonOpenLog.UseVisualStyleBackColor = true;
            this.buttonOpenLog.Click += new System.EventHandler(this.buttonOpenLog_Click);
            // 
            // splitContainerTreeTextBox
            // 
            this.splitContainerTreeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerTreeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTreeTextBox.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTreeTextBox.Name = "splitContainerTreeTextBox";
            // 
            // splitContainerTreeTextBox.Panel1
            // 
            this.splitContainerTreeTextBox.Panel1.Controls.Add(this.panelTreeView);
            // 
            // splitContainerTreeTextBox.Panel2
            // 
            this.splitContainerTreeTextBox.Panel2.Controls.Add(this.panelTextBox);
            this.splitContainerTreeTextBox.Size = new System.Drawing.Size(982, 528);
            this.splitContainerTreeTextBox.SplitterDistance = 371;
            this.splitContainerTreeTextBox.TabIndex = 0;
            // 
            // panelTreeView
            // 
            this.panelTreeView.Controls.Add(this.treeViewSiebelTree);
            this.panelTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTreeView.Location = new System.Drawing.Point(0, 0);
            this.panelTreeView.Name = "panelTreeView";
            this.panelTreeView.Size = new System.Drawing.Size(369, 526);
            this.panelTreeView.TabIndex = 0;
            // 
            // treeViewSiebelTree
            // 
            this.treeViewSiebelTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewSiebelTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSiebelTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewSiebelTree.Location = new System.Drawing.Point(0, 0);
            this.treeViewSiebelTree.Name = "treeViewSiebelTree";
            this.treeViewSiebelTree.Size = new System.Drawing.Size(369, 526);
            this.treeViewSiebelTree.TabIndex = 0;
            this.treeViewSiebelTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewSiebelTree_NodeMouseClick);
            // 
            // panelTextBox
            // 
            this.panelTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTextBox.Location = new System.Drawing.Point(0, 0);
            this.panelTextBox.Name = "panelTextBox";
            this.panelTextBox.Size = new System.Drawing.Size(605, 526);
            this.panelTextBox.TabIndex = 0;
            // 
            // buttonGoToLine
            // 
            this.buttonGoToLine.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGoToLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGoToLine.Location = new System.Drawing.Point(275, 2);
            this.buttonGoToLine.Name = "buttonGoToLine";
            this.buttonGoToLine.Size = new System.Drawing.Size(57, 24);
            this.buttonGoToLine.TabIndex = 10;
            this.buttonGoToLine.Text = "Go To Line";
            this.buttonGoToLine.UseVisualStyleBackColor = true;
            this.buttonGoToLine.Click += new System.EventHandler(this.buttonGoToLine_Click);
            // 
            // FormNotepad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 558);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormNotepad";
            this.Text = "Siebel Log Notepad";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNotepad_FormClosing);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.splitContainerTreeTextBox.Panel1.ResumeLayout(false);
            this.splitContainerTreeTextBox.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeTextBox)).EndInit();
            this.splitContainerTreeTextBox.ResumeLayout(false);
            this.panelTreeView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.SplitContainer splitContainerTreeTextBox;
        private System.Windows.Forms.Panel panelTreeView;
        private System.Windows.Forms.Panel panelTextBox;
        private System.Windows.Forms.Button buttonOpenLog;
        private System.Windows.Forms.Button buttonStatistics;
        private System.Windows.Forms.Button buttonIgnore;
        private System.Windows.Forms.Button buttonOpenConfig;
        private System.Windows.Forms.Button buttonRefreshTree;
        private System.Windows.Forms.TreeView treeViewSiebelTree;
        private System.Windows.Forms.Button buttonMarkAndFind;
        private System.Windows.Forms.Button buttonCollapse;
        private System.Windows.Forms.Button buttonExpandTree;
        private System.Windows.Forms.Button buttonChangeTree;
        private System.Windows.Forms.Button buttonGoToLine;
    }
}

