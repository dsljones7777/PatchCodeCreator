namespace PatchCodeCreator
{
    partial class Function_Definition_View
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DataGridView_FunctionDefinitions = new System.Windows.Forms.DataGridView();
            this.Column_FunctionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_FunctionOrdinal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_FunctionDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_FunctionDefinitions)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridView_FunctionDefinitions
            // 
            this.DataGridView_FunctionDefinitions.AllowUserToAddRows = false;
            this.DataGridView_FunctionDefinitions.AllowUserToDeleteRows = false;
            this.DataGridView_FunctionDefinitions.AllowUserToResizeColumns = false;
            this.DataGridView_FunctionDefinitions.AllowUserToResizeRows = false;
            this.DataGridView_FunctionDefinitions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridView_FunctionDefinitions.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView_FunctionDefinitions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DataGridView_FunctionDefinitions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.DataGridView_FunctionDefinitions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_FunctionDefinitions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_FunctionName,
            this.Column_FunctionOrdinal,
            this.Column_FunctionDefinition});
            this.DataGridView_FunctionDefinitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView_FunctionDefinitions.Location = new System.Drawing.Point(0, 0);
            this.DataGridView_FunctionDefinitions.MultiSelect = false;
            this.DataGridView_FunctionDefinitions.Name = "DataGridView_FunctionDefinitions";
            this.DataGridView_FunctionDefinitions.RowHeadersVisible = false;
            this.DataGridView_FunctionDefinitions.Size = new System.Drawing.Size(712, 384);
            this.DataGridView_FunctionDefinitions.TabIndex = 0;
            // 
            // Column_FunctionName
            // 
            this.Column_FunctionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Column_FunctionName.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column_FunctionName.HeaderText = "Function Name";
            this.Column_FunctionName.Name = "Column_FunctionName";
            this.Column_FunctionName.ReadOnly = true;
            this.Column_FunctionName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_FunctionName.Width = 96;
            // 
            // Column_FunctionOrdinal
            // 
            this.Column_FunctionOrdinal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Column_FunctionOrdinal.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_FunctionOrdinal.HeaderText = "Ordinal";
            this.Column_FunctionOrdinal.Name = "Column_FunctionOrdinal";
            this.Column_FunctionOrdinal.ReadOnly = true;
            this.Column_FunctionOrdinal.Width = 65;
            // 
            // Column_FunctionDefinition
            // 
            this.Column_FunctionDefinition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_FunctionDefinition.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_FunctionDefinition.HeaderText = "Function Definition";
            this.Column_FunctionDefinition.Name = "Column_FunctionDefinition";
            this.Column_FunctionDefinition.Width = 110;
            // 
            // Function_Definition_View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 384);
            this.Controls.Add(this.DataGridView_FunctionDefinitions);
            this.Name = "Function_Definition_View";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Function_Definition_View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Function_Definition_View_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_FunctionDefinitions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridView_FunctionDefinitions;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FunctionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FunctionOrdinal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FunctionDefinition;
    }
}