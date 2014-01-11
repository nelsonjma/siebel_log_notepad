namespace SiebelLogNotepad.forms
{
    partial class MarkAndFind
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
            this.buttonChangeColor = new System.Windows.Forms.Button();
            this.radioButtonText = new System.Windows.Forms.RadioButton();
            this.radioButtonTree = new System.Windows.Forms.RadioButton();
            this.buttonFindNext = new System.Windows.Forms.Button();
            this.buttonMarkAll = new System.Windows.Forms.Button();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonChangeColor
            // 
            this.buttonChangeColor.FlatAppearance.BorderSize = 0;
            this.buttonChangeColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangeColor.Location = new System.Drawing.Point(248, 30);
            this.buttonChangeColor.Name = "buttonChangeColor";
            this.buttonChangeColor.Size = new System.Drawing.Size(120, 23);
            this.buttonChangeColor.TabIndex = 11;
            this.buttonChangeColor.Text = "Change Color";
            this.buttonChangeColor.UseVisualStyleBackColor = true;
            this.buttonChangeColor.Click += new System.EventHandler(this.buttonChangeColor_Click);
            // 
            // radioButtonText
            // 
            this.radioButtonText.AutoSize = true;
            this.radioButtonText.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButtonText.Location = new System.Drawing.Point(12, 88);
            this.radioButtonText.Name = "radioButtonText";
            this.radioButtonText.Size = new System.Drawing.Size(57, 17);
            this.radioButtonText.TabIndex = 10;
            this.radioButtonText.TabStop = true;
            this.radioButtonText.Text = "In Text";
            this.radioButtonText.UseVisualStyleBackColor = true;
            // 
            // radioButtonTree
            // 
            this.radioButtonTree.AutoSize = true;
            this.radioButtonTree.Checked = true;
            this.radioButtonTree.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButtonTree.Location = new System.Drawing.Point(12, 65);
            this.radioButtonTree.Name = "radioButtonTree";
            this.radioButtonTree.Size = new System.Drawing.Size(58, 17);
            this.radioButtonTree.TabIndex = 9;
            this.radioButtonTree.TabStop = true;
            this.radioButtonTree.Text = "In Tree";
            this.radioButtonTree.UseVisualStyleBackColor = true;
            // 
            // buttonFindNext
            // 
            this.buttonFindNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonFindNext.Location = new System.Drawing.Point(130, 30);
            this.buttonFindNext.Name = "buttonFindNext";
            this.buttonFindNext.Size = new System.Drawing.Size(112, 23);
            this.buttonFindNext.TabIndex = 8;
            this.buttonFindNext.Text = "Find Next";
            this.buttonFindNext.UseVisualStyleBackColor = true;
            this.buttonFindNext.Click += new System.EventHandler(this.buttonFindNext_Click);
            // 
            // buttonMarkAll
            // 
            this.buttonMarkAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonMarkAll.Location = new System.Drawing.Point(12, 30);
            this.buttonMarkAll.Name = "buttonMarkAll";
            this.buttonMarkAll.Size = new System.Drawing.Size(112, 23);
            this.buttonMarkAll.TabIndex = 7;
            this.buttonMarkAll.Text = "Mark All";
            this.buttonMarkAll.UseVisualStyleBackColor = true;
            this.buttonMarkAll.Click += new System.EventHandler(this.buttonMarkAll_Click);
            // 
            // textBoxFind
            // 
            this.textBoxFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFind.Location = new System.Drawing.Point(12, 3);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(356, 20);
            this.textBoxFind.TabIndex = 6;
            // 
            // MarkAndFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 117);
            this.Controls.Add(this.buttonChangeColor);
            this.Controls.Add(this.radioButtonText);
            this.Controls.Add(this.radioButtonTree);
            this.Controls.Add(this.buttonFindNext);
            this.Controls.Add(this.buttonMarkAll);
            this.Controls.Add(this.textBoxFind);
            this.Name = "MarkAndFind";
            this.Text = "Mark & Find";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChangeColor;
        private System.Windows.Forms.RadioButton radioButtonText;
        private System.Windows.Forms.RadioButton radioButtonTree;
        private System.Windows.Forms.Button buttonFindNext;
        private System.Windows.Forms.Button buttonMarkAll;
        private System.Windows.Forms.TextBox textBoxFind;
    }
}