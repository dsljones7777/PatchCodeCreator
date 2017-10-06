namespace PatchCodeCreator
{
    partial class Form_Patch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.GroupBox_Info = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_Name = new System.Windows.Forms.Label();
            this.Label_Type = new System.Windows.Forms.Label();
            this.Label_Architecture = new System.Windows.Forms.Label();
            this.Label_ImageVersion = new System.Windows.Forms.Label();
            this.Label_OSVersion = new System.Windows.Forms.Label();
            this.Label_Subsystem = new System.Windows.Forms.Label();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ToolStripStatusLabel_GettingFunctionDefinitions = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel_CreateProjectStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Create = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_AsProject = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_HeaderSource = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_View = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_FunctionDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.Timer_Schroll = new System.Windows.Forms.Timer(this.components);
            this.DataGrid_Options = new System.Windows.Forms.DataGridView();
            this.Column_FunctionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Ordinal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Forwarded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PatchedOSVersion = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_ForwardTo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.GroupBox_Info.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Options)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox_Info
            // 
            this.GroupBox_Info.AutoSize = true;
            this.GroupBox_Info.Controls.Add(this.flowLayoutPanel1);
            this.GroupBox_Info.Dock = System.Windows.Forms.DockStyle.Left;
            this.GroupBox_Info.Location = new System.Drawing.Point(0, 24);
            this.GroupBox_Info.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.GroupBox_Info.Name = "GroupBox_Info";
            this.GroupBox_Info.Size = new System.Drawing.Size(96, 632);
            this.GroupBox_Info.TabIndex = 1;
            this.GroupBox_Info.TabStop = false;
            this.GroupBox_Info.Text = "Info";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.Controls.Add(this.Label_Name);
            this.flowLayoutPanel1.Controls.Add(this.Label_Type);
            this.flowLayoutPanel1.Controls.Add(this.Label_Architecture);
            this.flowLayoutPanel1.Controls.Add(this.Label_ImageVersion);
            this.flowLayoutPanel1.Controls.Add(this.Label_OSVersion);
            this.flowLayoutPanel1.Controls.Add(this.Label_Subsystem);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(90, 613);
            this.flowLayoutPanel1.TabIndex = 6;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // Label_Name
            // 
            this.Label_Name.AutoSize = true;
            this.Label_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Name.Location = new System.Drawing.Point(3, 0);
            this.Label_Name.Name = "Label_Name";
            this.Label_Name.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.Label_Name.Size = new System.Drawing.Size(80, 19);
            this.Label_Name.TabIndex = 0;
            this.Label_Name.Text = "Name";
            this.Label_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label_Type
            // 
            this.Label_Type.AutoSize = true;
            this.Label_Type.Location = new System.Drawing.Point(3, 19);
            this.Label_Type.Name = "Label_Type";
            this.Label_Type.Padding = new System.Windows.Forms.Padding(3);
            this.Label_Type.Size = new System.Drawing.Size(37, 19);
            this.Label_Type.TabIndex = 1;
            this.Label_Type.Text = "Type";
            // 
            // Label_Architecture
            // 
            this.Label_Architecture.AutoSize = true;
            this.Label_Architecture.Location = new System.Drawing.Point(3, 38);
            this.Label_Architecture.Name = "Label_Architecture";
            this.Label_Architecture.Padding = new System.Windows.Forms.Padding(3);
            this.Label_Architecture.Size = new System.Drawing.Size(70, 19);
            this.Label_Architecture.TabIndex = 2;
            this.Label_Architecture.Text = "Architecture";
            // 
            // Label_ImageVersion
            // 
            this.Label_ImageVersion.AutoSize = true;
            this.Label_ImageVersion.Location = new System.Drawing.Point(3, 57);
            this.Label_ImageVersion.Name = "Label_ImageVersion";
            this.Label_ImageVersion.Padding = new System.Windows.Forms.Padding(3);
            this.Label_ImageVersion.Size = new System.Drawing.Size(80, 19);
            this.Label_ImageVersion.TabIndex = 3;
            this.Label_ImageVersion.Text = "Image Version";
            // 
            // Label_OSVersion
            // 
            this.Label_OSVersion.AutoSize = true;
            this.Label_OSVersion.Location = new System.Drawing.Point(3, 76);
            this.Label_OSVersion.Name = "Label_OSVersion";
            this.Label_OSVersion.Padding = new System.Windows.Forms.Padding(3);
            this.Label_OSVersion.Size = new System.Drawing.Size(66, 19);
            this.Label_OSVersion.TabIndex = 4;
            this.Label_OSVersion.Text = "OS Version";
            // 
            // Label_Subsystem
            // 
            this.Label_Subsystem.AutoSize = true;
            this.Label_Subsystem.Location = new System.Drawing.Point(3, 95);
            this.Label_Subsystem.Name = "Label_Subsystem";
            this.Label_Subsystem.Padding = new System.Windows.Forms.Padding(3);
            this.Label_Subsystem.Size = new System.Drawing.Size(64, 19);
            this.Label_Subsystem.TabIndex = 5;
            this.Label_Subsystem.Text = "Subsystem";
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripProgressBar,
            this.ToolStripStatusLabel_GettingFunctionDefinitions,
            this.ToolStripStatusLabel_CreateProjectStatus});
            this.StatusStrip.Location = new System.Drawing.Point(0, 656);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StatusStrip.Size = new System.Drawing.Size(1243, 22);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.TabIndex = 3;
            // 
            // ToolStripProgressBar
            // 
            this.ToolStripProgressBar.Name = "ToolStripProgressBar";
            this.ToolStripProgressBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ToolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // ToolStripStatusLabel_GettingFunctionDefinitions
            // 
            this.ToolStripStatusLabel_GettingFunctionDefinitions.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.ToolStripStatusLabel_GettingFunctionDefinitions.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.ToolStripStatusLabel_GettingFunctionDefinitions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripStatusLabel_GettingFunctionDefinitions.Name = "ToolStripStatusLabel_GettingFunctionDefinitions";
            this.ToolStripStatusLabel_GettingFunctionDefinitions.Padding = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.ToolStripStatusLabel_GettingFunctionDefinitions.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ToolStripStatusLabel_GettingFunctionDefinitions.Size = new System.Drawing.Size(22, 17);
            // 
            // ToolStripStatusLabel_CreateProjectStatus
            // 
            this.ToolStripStatusLabel_CreateProjectStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.ToolStripStatusLabel_CreateProjectStatus.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.ToolStripStatusLabel_CreateProjectStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripStatusLabel_CreateProjectStatus.Name = "ToolStripStatusLabel_CreateProjectStatus";
            this.ToolStripStatusLabel_CreateProjectStatus.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.ToolStripStatusLabel_CreateProjectStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ToolStripStatusLabel_CreateProjectStatus.Size = new System.Drawing.Size(34, 17);
            this.ToolStripStatusLabel_CreateProjectStatus.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_File,
            this.ToolStripMenuItem_View});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1243, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem_File
            // 
            this.ToolStripMenuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Create,
            this.ToolStripMenuItem_Close});
            this.ToolStripMenuItem_File.Name = "ToolStripMenuItem_File";
            this.ToolStripMenuItem_File.Size = new System.Drawing.Size(37, 20);
            this.ToolStripMenuItem_File.Text = "File";
            // 
            // ToolStripMenuItem_Create
            // 
            this.ToolStripMenuItem_Create.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_AsProject,
            this.ToolStripMenuItem_HeaderSource});
            this.ToolStripMenuItem_Create.Name = "ToolStripMenuItem_Create";
            this.ToolStripMenuItem_Create.Size = new System.Drawing.Size(108, 22);
            this.ToolStripMenuItem_Create.Text = "Create";
            // 
            // ToolStripMenuItem_AsProject
            // 
            this.ToolStripMenuItem_AsProject.Name = "ToolStripMenuItem_AsProject";
            this.ToolStripMenuItem_AsProject.Size = new System.Drawing.Size(169, 22);
            this.ToolStripMenuItem_AsProject.Text = "As Project";
            // 
            // ToolStripMenuItem_HeaderSource
            // 
            this.ToolStripMenuItem_HeaderSource.Name = "ToolStripMenuItem_HeaderSource";
            this.ToolStripMenuItem_HeaderSource.Size = new System.Drawing.Size(169, 22);
            this.ToolStripMenuItem_HeaderSource.Text = "As Header/Source";
            // 
            // ToolStripMenuItem_Close
            // 
            this.ToolStripMenuItem_Close.Name = "ToolStripMenuItem_Close";
            this.ToolStripMenuItem_Close.Size = new System.Drawing.Size(108, 22);
            this.ToolStripMenuItem_Close.Text = "Close";
            this.ToolStripMenuItem_Close.Click += new System.EventHandler(this.ToolStripMenuItem_Close_Click_1);
            // 
            // ToolStripMenuItem_View
            // 
            this.ToolStripMenuItem_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_FunctionDefinitions});
            this.ToolStripMenuItem_View.Name = "ToolStripMenuItem_View";
            this.ToolStripMenuItem_View.Size = new System.Drawing.Size(44, 20);
            this.ToolStripMenuItem_View.Text = "View";
            // 
            // ToolStripMenuItem_FunctionDefinitions
            // 
            this.ToolStripMenuItem_FunctionDefinitions.Name = "ToolStripMenuItem_FunctionDefinitions";
            this.ToolStripMenuItem_FunctionDefinitions.Size = new System.Drawing.Size(181, 22);
            this.ToolStripMenuItem_FunctionDefinitions.Text = "Function Definitions";
            // 
            // Timer_Schroll
            // 
            this.Timer_Schroll.Interval = 200;
            this.Timer_Schroll.Tick += new System.EventHandler(this.Timer_Schroll_Tick);
            // 
            // DataGrid_Options
            // 
            this.DataGrid_Options.AllowUserToAddRows = false;
            this.DataGrid_Options.AllowUserToDeleteRows = false;
            this.DataGrid_Options.AllowUserToResizeColumns = false;
            this.DataGrid_Options.AllowUserToResizeRows = false;
            this.DataGrid_Options.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGrid_Options.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGrid_Options.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DataGrid_Options.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGrid_Options.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGrid_Options.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_Options.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_FunctionName,
            this.Column_Ordinal,
            this.Column_Forwarded,
            this.Column_PatchedOSVersion,
            this.Column_ForwardTo});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGrid_Options.DefaultCellStyle = dataGridViewCellStyle7;
            this.DataGrid_Options.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGrid_Options.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DataGrid_Options.Location = new System.Drawing.Point(96, 24);
            this.DataGrid_Options.Name = "DataGrid_Options";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGrid_Options.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.DataGrid_Options.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.DataGrid_Options.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DataGrid_Options.ShowCellErrors = false;
            this.DataGrid_Options.ShowCellToolTips = false;
            this.DataGrid_Options.ShowEditingIcon = false;
            this.DataGrid_Options.Size = new System.Drawing.Size(1147, 632);
            this.DataGrid_Options.TabIndex = 5;
            // 
            // Column_FunctionName
            // 
            this.Column_FunctionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_FunctionName.DataPropertyName = "Column_FunctionName";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_FunctionName.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_FunctionName.HeaderText = "Exported\nName";
            this.Column_FunctionName.Name = "Column_FunctionName";
            this.Column_FunctionName.ReadOnly = true;
            this.Column_FunctionName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column_FunctionName.Width = 74;
            // 
            // Column_Ordinal
            // 
            this.Column_Ordinal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_Ordinal.DataPropertyName = "Column_Ordinal";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_Ordinal.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_Ordinal.HeaderText = "Export\nOrdinal";
            this.Column_Ordinal.Name = "Column_Ordinal";
            this.Column_Ordinal.ReadOnly = true;
            this.Column_Ordinal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column_Ordinal.Width = 65;
            // 
            // Column_Forwarded
            // 
            this.Column_Forwarded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_Forwarded.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column_Forwarded.HeaderText = "Forwarded\nName";
            this.Column_Forwarded.Name = "Column_Forwarded";
            this.Column_Forwarded.ReadOnly = true;
            this.Column_Forwarded.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column_Forwarded.Width = 82;
            // 
            // Column_PatchedOSVersion
            // 
            this.Column_PatchedOSVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_PatchedOSVersion.DataPropertyName = "Column_PatchedOSVersion";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_PatchedOSVersion.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column_PatchedOSVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Column_PatchedOSVersion.HeaderText = "Forward\nTo\nVersions";
            this.Column_PatchedOSVersion.MaxDropDownItems = 15;
            this.Column_PatchedOSVersion.Name = "Column_PatchedOSVersion";
            this.Column_PatchedOSVersion.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column_PatchedOSVersion.Width = 53;
            // 
            // Column_ForwardTo
            // 
            this.Column_ForwardTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_ForwardTo.DataPropertyName = "Column_ForwardTo";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = false;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_ForwardTo.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column_ForwardTo.FalseValue = "";
            this.Column_ForwardTo.HeaderText = "Forward\nTo\nPatch";
            this.Column_ForwardTo.Name = "Column_ForwardTo";
            this.Column_ForwardTo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column_ForwardTo.TrueValue = "";
            this.Column_ForwardTo.Width = 51;
            // 
            // Form_Patch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1243, 678);
            this.Controls.Add(this.DataGrid_Options);
            this.Controls.Add(this.GroupBox_Info);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Name = "Form_Patch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patch Creator";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Patch_FormClosing);
            this.Load += new System.EventHandler(this.Form_Patch_Load);
            this.GroupBox_Info.ResumeLayout(false);
            this.GroupBox_Info.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Options)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBox_Info;
        private System.Windows.Forms.Label Label_Subsystem;
        private System.Windows.Forms.Label Label_OSVersion;
        private System.Windows.Forms.Label Label_ImageVersion;
        private System.Windows.Forms.Label Label_Architecture;
        private System.Windows.Forms.Label Label_Type;
        private System.Windows.Forms.Label Label_Name;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripProgressBar ToolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_GettingFunctionDefinitions;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_File;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Create;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_AsProject;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_HeaderSource;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Close;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_View;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_FunctionDefinitions;
        private System.Windows.Forms.Timer Timer_Schroll;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_CreateProjectStatus;
        private System.Windows.Forms.DataGridView DataGrid_Options;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FunctionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Ordinal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Forwarded;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_PatchedOSVersion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_ForwardTo;
    }
}

