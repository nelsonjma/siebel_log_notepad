namespace SiebelLogNotepad.Forms
{
    partial class IgnoreEvents
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IgnoreEvents));
            this.splitContainerMaster = new System.Windows.Forms.SplitContainer();
            this.dataGridViewIgnoreList = new System.Windows.Forms.DataGridView();
            this.buttonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaster)).BeginInit();
            this.splitContainerMaster.Panel1.SuspendLayout();
            this.splitContainerMaster.Panel2.SuspendLayout();
            this.splitContainerMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIgnoreList)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMaster
            // 
            this.splitContainerMaster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMaster.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerMaster.IsSplitterFixed = true;
            this.splitContainerMaster.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMaster.Name = "splitContainerMaster";
            this.splitContainerMaster.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMaster.Panel1
            // 
            this.splitContainerMaster.Panel1.Controls.Add(this.dataGridViewIgnoreList);
            // 
            // splitContainerMaster.Panel2
            // 
            this.splitContainerMaster.Panel2.Controls.Add(this.buttonSave);
            this.splitContainerMaster.Size = new System.Drawing.Size(601, 298);
            this.splitContainerMaster.SplitterDistance = 261;
            this.splitContainerMaster.TabIndex = 0;
            // 
            // dataGridViewIgnoreList
            // 
            this.dataGridViewIgnoreList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIgnoreList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewIgnoreList.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewIgnoreList.Name = "dataGridViewIgnoreList";
            this.dataGridViewIgnoreList.Size = new System.Drawing.Size(599, 259);
            this.dataGridViewIgnoreList.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Location = new System.Drawing.Point(4, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // IgnoreEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 298);
            this.Controls.Add(this.splitContainerMaster);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IgnoreEvents";
            this.Text = "Ignore Events";
            this.splitContainerMaster.Panel1.ResumeLayout(false);
            this.splitContainerMaster.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaster)).EndInit();
            this.splitContainerMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIgnoreList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMaster;
        private System.Windows.Forms.DataGridView dataGridViewIgnoreList;
        private System.Windows.Forms.Button buttonSave;
    }
}