namespace PatchCodeCreator
{
    partial class PatchesParent
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
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.LastButton = new System.Windows.Forms.Button();
            this.Next = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.FirstButton = new System.Windows.Forms.Button();
            this.PatchFormsPanel = new System.Windows.Forms.Panel();
            this.ControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.LastButton);
            this.ControlPanel.Controls.Add(this.Next);
            this.ControlPanel.Controls.Add(this.PreviousButton);
            this.ControlPanel.Controls.Add(this.FirstButton);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlPanel.Location = new System.Drawing.Point(0, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(1231, 30);
            this.ControlPanel.TabIndex = 1;
            // 
            // LastButton
            // 
            this.LastButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.LastButton.Location = new System.Drawing.Point(225, 0);
            this.LastButton.Name = "LastButton";
            this.LastButton.Size = new System.Drawing.Size(75, 30);
            this.LastButton.TabIndex = 3;
            this.LastButton.Text = "Last";
            this.LastButton.UseVisualStyleBackColor = true;
            this.LastButton.Click += new System.EventHandler(this.LastButton_Click);
            // 
            // Next
            // 
            this.Next.Dock = System.Windows.Forms.DockStyle.Left;
            this.Next.Location = new System.Drawing.Point(150, 0);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(75, 30);
            this.Next.TabIndex = 2;
            this.Next.Text = "Next";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PreviousButton.Location = new System.Drawing.Point(75, 0);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(75, 30);
            this.PreviousButton.TabIndex = 1;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // FirstButton
            // 
            this.FirstButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.FirstButton.Location = new System.Drawing.Point(0, 0);
            this.FirstButton.Name = "FirstButton";
            this.FirstButton.Size = new System.Drawing.Size(75, 30);
            this.FirstButton.TabIndex = 0;
            this.FirstButton.Text = "First";
            this.FirstButton.UseVisualStyleBackColor = true;
            this.FirstButton.Click += new System.EventHandler(this.FirstButton_Click);
            // 
            // PatchFormsPanel
            // 
            this.PatchFormsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PatchFormsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PatchFormsPanel.ImeMode = System.Windows.Forms.ImeMode.On;
            this.PatchFormsPanel.Location = new System.Drawing.Point(0, 30);
            this.PatchFormsPanel.Name = "PatchFormsPanel";
            this.PatchFormsPanel.Size = new System.Drawing.Size(1231, 559);
            this.PatchFormsPanel.TabIndex = 2;
            // 
            // PatchesParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 589);
            this.Controls.Add(this.PatchFormsPanel);
            this.Controls.Add(this.ControlPanel);
            this.Name = "PatchesParent";
            this.ShowIcon = false;
            this.Text = "Patch Creator - Multiple Items";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PatchesParent_FormClosing);
            this.Shown += new System.EventHandler(this.PatchesParent_Shown);
            this.Resize += new System.EventHandler(this.PatchesParent_Resize);
            this.ControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Button LastButton;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.Button FirstButton;
        private System.Windows.Forms.Panel PatchFormsPanel;
    }
}



