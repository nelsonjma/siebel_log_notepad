namespace SiebelLogNotepad.forms
{
    partial class ChangeTreeLabel
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
            this.checkBoxProcessedText = new System.Windows.Forms.CheckBox();
            this.checkBoxLine = new System.Windows.Forms.CheckBox();
            this.checkBoxLogEvent = new System.Windows.Forms.CheckBox();
            this.checkBoxLogSubEvent = new System.Windows.Forms.CheckBox();
            this.checkBoxLogDateTime = new System.Windows.Forms.CheckBox();
            this.checkBoxLogMessage = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxProcessedText
            // 
            this.checkBoxProcessedText.AutoSize = true;
            this.checkBoxProcessedText.Location = new System.Drawing.Point(12, 12);
            this.checkBoxProcessedText.Name = "checkBoxProcessedText";
            this.checkBoxProcessedText.Size = new System.Drawing.Size(100, 17);
            this.checkBoxProcessedText.TabIndex = 0;
            this.checkBoxProcessedText.Text = "Processed Text";
            this.checkBoxProcessedText.UseVisualStyleBackColor = true;
            // 
            // checkBoxLine
            // 
            this.checkBoxLine.AutoSize = true;
            this.checkBoxLine.Location = new System.Drawing.Point(6, 19);
            this.checkBoxLine.Name = "checkBoxLine";
            this.checkBoxLine.Size = new System.Drawing.Size(46, 17);
            this.checkBoxLine.TabIndex = 1;
            this.checkBoxLine.Text = "Line";
            this.checkBoxLine.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogEvent
            // 
            this.checkBoxLogEvent.AutoSize = true;
            this.checkBoxLogEvent.Location = new System.Drawing.Point(6, 42);
            this.checkBoxLogEvent.Name = "checkBoxLogEvent";
            this.checkBoxLogEvent.Size = new System.Drawing.Size(54, 17);
            this.checkBoxLogEvent.TabIndex = 2;
            this.checkBoxLogEvent.Text = "Event";
            this.checkBoxLogEvent.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogSubEvent
            // 
            this.checkBoxLogSubEvent.AutoSize = true;
            this.checkBoxLogSubEvent.Location = new System.Drawing.Point(6, 65);
            this.checkBoxLogSubEvent.Name = "checkBoxLogSubEvent";
            this.checkBoxLogSubEvent.Size = new System.Drawing.Size(76, 17);
            this.checkBoxLogSubEvent.TabIndex = 3;
            this.checkBoxLogSubEvent.Text = "Sub Event";
            this.checkBoxLogSubEvent.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogDateTime
            // 
            this.checkBoxLogDateTime.AutoSize = true;
            this.checkBoxLogDateTime.Location = new System.Drawing.Point(6, 88);
            this.checkBoxLogDateTime.Name = "checkBoxLogDateTime";
            this.checkBoxLogDateTime.Size = new System.Drawing.Size(72, 17);
            this.checkBoxLogDateTime.TabIndex = 4;
            this.checkBoxLogDateTime.Text = "DateTime";
            this.checkBoxLogDateTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogMessage
            // 
            this.checkBoxLogMessage.AutoSize = true;
            this.checkBoxLogMessage.Location = new System.Drawing.Point(6, 111);
            this.checkBoxLogMessage.Name = "checkBoxLogMessage";
            this.checkBoxLogMessage.Size = new System.Drawing.Size(69, 17);
            this.checkBoxLogMessage.TabIndex = 5;
            this.checkBoxLogMessage.Text = "Message";
            this.checkBoxLogMessage.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSave.Location = new System.Drawing.Point(12, 178);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxLine);
            this.groupBox1.Controls.Add(this.checkBoxLogEvent);
            this.groupBox1.Controls.Add(this.checkBoxLogMessage);
            this.groupBox1.Controls.Add(this.checkBoxLogSubEvent);
            this.groupBox1.Controls.Add(this.checkBoxLogDateTime);
            this.groupBox1.Location = new System.Drawing.Point(12, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 137);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // ChangeTreeLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 206);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.checkBoxProcessedText);
            this.Name = "ChangeTreeLabel";
            this.Text = "Tree Label";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxProcessedText;
        private System.Windows.Forms.CheckBox checkBoxLine;
        private System.Windows.Forms.CheckBox checkBoxLogEvent;
        private System.Windows.Forms.CheckBox checkBoxLogSubEvent;
        private System.Windows.Forms.CheckBox checkBoxLogDateTime;
        private System.Windows.Forms.CheckBox checkBoxLogMessage;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}