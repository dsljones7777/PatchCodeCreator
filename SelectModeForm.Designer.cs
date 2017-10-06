namespace PatchCodeCreator
{
    partial class Form_SelectMode
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
            this.Dialog_Open = new System.Windows.Forms.OpenFileDialog();
            this.RadioButton_PatchCode = new System.Windows.Forms.RadioButton();
            this.RadioButton_Analyze = new System.Windows.Forms.RadioButton();
            this.GroupBox_ModeSelection = new System.Windows.Forms.GroupBox();
            this.Button_Process = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.GroupBox_ModeSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // RadioButton_PatchCode
            // 
            this.RadioButton_PatchCode.AutoSize = true;
            this.RadioButton_PatchCode.Checked = true;
            this.RadioButton_PatchCode.Location = new System.Drawing.Point(24, 32);
            this.RadioButton_PatchCode.Name = "RadioButton_PatchCode";
            this.RadioButton_PatchCode.Size = new System.Drawing.Size(81, 17);
            this.RadioButton_PatchCode.TabIndex = 0;
            this.RadioButton_PatchCode.TabStop = true;
            this.RadioButton_PatchCode.Text = "Patch Code";
            this.RadioButton_PatchCode.UseVisualStyleBackColor = true;
            this.RadioButton_PatchCode.CheckedChanged += new System.EventHandler(this.RadioButton_PatchCode_CheckedChanged);
            // 
            // RadioButton_Analyze
            // 
            this.RadioButton_Analyze.AutoSize = true;
            this.RadioButton_Analyze.Location = new System.Drawing.Point(24, 87);
            this.RadioButton_Analyze.Name = "RadioButton_Analyze";
            this.RadioButton_Analyze.Size = new System.Drawing.Size(62, 17);
            this.RadioButton_Analyze.TabIndex = 1;
            this.RadioButton_Analyze.Text = "Analyze";
            this.RadioButton_Analyze.UseVisualStyleBackColor = true;
            // 
            // GroupBox_ModeSelection
            // 
            this.GroupBox_ModeSelection.Controls.Add(this.RadioButton_PatchCode);
            this.GroupBox_ModeSelection.Controls.Add(this.RadioButton_Analyze);
            this.GroupBox_ModeSelection.Dock = System.Windows.Forms.DockStyle.Left;
            this.GroupBox_ModeSelection.Location = new System.Drawing.Point(0, 0);
            this.GroupBox_ModeSelection.Name = "GroupBox_ModeSelection";
            this.GroupBox_ModeSelection.Size = new System.Drawing.Size(117, 135);
            this.GroupBox_ModeSelection.TabIndex = 2;
            this.GroupBox_ModeSelection.TabStop = false;
            this.GroupBox_ModeSelection.Text = "Mode";
            this.GroupBox_ModeSelection.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // Button_Process
            // 
            this.Button_Process.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Process.Location = new System.Drawing.Point(143, 29);
            this.Button_Process.Name = "Button_Process";
            this.Button_Process.Size = new System.Drawing.Size(75, 23);
            this.Button_Process.TabIndex = 3;
            this.Button_Process.Text = "Process";
            this.Button_Process.UseVisualStyleBackColor = true;
            this.Button_Process.Click += new System.EventHandler(this.Button_Process_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Cancel.Location = new System.Drawing.Point(143, 81);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Button_Cancel.TabIndex = 4;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            // 
            // Form_SelectMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 135);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Button_Process);
            this.Controls.Add(this.GroupBox_ModeSelection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form_SelectMode";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Mode";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SelectMode_Load);
            this.GroupBox_ModeSelection.ResumeLayout(false);
            this.GroupBox_ModeSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog Dialog_Open;
        private System.Windows.Forms.RadioButton RadioButton_PatchCode;
        private System.Windows.Forms.RadioButton RadioButton_Analyze;
        private System.Windows.Forms.GroupBox GroupBox_ModeSelection;
        private System.Windows.Forms.Button Button_Process;
        private System.Windows.Forms.Button Button_Cancel;
    }
}