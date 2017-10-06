using StandardLibrary_CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WindowsPECLR;
namespace PatchCodeCreator
{

    // This form shows opened portable executable files and allow for their exports to be patched
    internal partial class Form_Patch : Form
    {
        private bool _isClosing;
        // This is the current portable executable file
        private WindowsPeNet _currentInfo;

        // Determines which way is currently scrolling
        private bool _scrollDown;

        //The path where created patches solutions are stored
        private const string SolutionPath = @"C:\Users\David Jones\OneDrive\Programming\Windows Dll Patches\";
        //The path for the created patch solution
        private const string SolutionFilename = "Windows Dll Patches.sln";

        private CancelableTask _funcDefFinderTask;
        private CancelableResultTask<string> _createProjectTask;

        public Form_Patch()
        {
            this._isClosing = false;
            InitializeComponent();
            //Set the double buffered property for the datagridview
            PropertyInfo doubleprop = this.DataGrid_Options.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleprop.SetValue(this.DataGrid_Options, true);
            //Initialize default values for forward to 
            this.Column_ForwardTo.TrueValue = true;
            this.Column_ForwardTo.FalseValue = false;
            //Add click event handlers for menu items
            this.ToolStripMenuItem_FunctionDefinitions.Click += this.ToolStripMenuItem_FunctionDefinitions_ItemClicked;
            this.ToolStripMenuItem_AsProject.Click += this.ToolStripMenuItem_CreateProject_ItemClicked;
            this.ToolStripMenuItem_Close.Click += ToolStripMenuItem_Close_Click;
        }

        ~Form_Patch()
        {
            this.Dispose(false);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (this._funcDefFinderTask != null)
                this._funcDefFinderTask.Dispose();
            this._funcDefFinderTask = null;
            if (this._createProjectTask != null)
                this._createProjectTask.Dispose();
            this._createProjectTask = null;
            base.Dispose(disposing);
        }

        public void invokeStart()
        {
            this.Invoke((MethodInvoker)this.Initialize);
        }
        private void Initialize()
        {
            this.ShowInTaskbar = true;
            this.Opacity = 1.00;
            this.WindowState = FormWindowState.Normal;
        }

        // This function updates the form to show the exports and basic information of the portable executable file
        public void UpdateWithDatabase(WindowsPeNet info)
        {
            //Store the current info
            this._currentInfo = info;
            
            //Update the rows in the DataGridView
            this.LoadDataGridRows();

            //Update the sidebar info containing version subsystem and etc information
            this.UpdateInfo();

            if (this._currentInfo.Exports == null)
                return;

            //Update the progress bar and status label
            this.ToolStripProgressBar.Maximum = this._currentInfo.Exports.Count;
            this.ToolStripStatusLabel_GettingFunctionDefinitions.Text = "0 / " + this._currentInfo.Exports.Count.ToString() + " exports processed ( 0.00% )( 0 Missing Function Definitions )";
            this.ToolStripStatusLabel_GettingFunctionDefinitions.Tag = (int)0;

            //Make sure no rows are selected
            this.DataGrid_Options.ClearSelection();

            //Attach updated event handlers
            for(int i = 0; i < this._currentInfo.Exports.Count; i ++)
            {
                this._currentInfo.Exports[i].ExportChangedEvent += this.ExportChangedEvent;
                this._currentInfo.Exports[i].DefinitionChangingEvent += this.ExportDefinitionChangingEvent;
            }

            //Start task to find the export function definitions
            this._funcDefFinderTask = WinFuncDefFinder.StartQueryTask(this.DefinitionFinderCompleted, this._currentInfo.FullName, this._currentInfo.Exports);
            this._funcDefFinderTask.CurrentTask.Start();
        }
        private void ExportChangedEvent(PeExportNet what)
        {
            this.Invoke((PeExportNet.ExportUpdated)this.ExportFuncUpdated, what);
        }
        private bool ExportDefinitionChangingEvent(String val, PeExportNet what)
        {
            if (String.IsNullOrWhiteSpace(what.Definition) == true)
                return true;
            DialogResult result = DialogResult.Cancel;
            if (String.IsNullOrWhiteSpace(what.Name) == false)
            {
                result = MessageBox.Show("Should the Existing Function Definition For "
                + what.Name + " in " + this._currentInfo.FullName +
                " Be Replaced?\nCurrent Definition:\n\t" +
                what.Definition + "\nNew Definition:\n\t" + ((val != null) ? val : "Nothing"),
                "Replace Function Definition?", MessageBoxButtons.YesNo);
            }
            else
            {
                result = MessageBox.Show("Should the Existing Function Definition For @"
               + what.Ordinal.ToString() + " in " + this._currentInfo.FullName +
               " Be Replaced?\nCurrent Definition:\n\t" +
               what.Definition + "\nNew Definition:\n\t" + ((val != null) ? val : "Nothing"),
               "Replace Function Definition?", MessageBoxButtons.YesNo);
            }
            if (result == DialogResult.Yes)
                return true;
            return false;
        }
        private void ToolStripMenuItem_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        // This is called when an export function is updated
        private void ExportFuncUpdated(PeExportNet export)
        { 
            //Determine which row contains the export that was updated
            int total = this.DataGrid_Options.Rows.Count;
            for (int i = 0; i < total; i ++)
            {
                DataGridViewRow row = this.DataGrid_Options.Rows[i];
                if ((ushort)row.Cells[this.Column_Ordinal.Index].Value != export.Ordinal)
                    continue;
                this.SuspendLayout();
                this.PopulateRow(row, export);
                this.ResumeLayout();
                return;
            }
            
        }

        private void DefinitionFinderCompleted(object sender, EventArgs e)
        {
            if (this._isClosing == true)
                return;
            this._funcDefFinderTask.Dispose();
            this._funcDefFinderTask = null;
            foreach(PeExportNet export in this._currentInfo.Exports)
            {
                if (export.DefinitionStatus == ExportDefinitionStatus.Timeout 
                    || export.DefinitionStatus == ExportDefinitionStatus.Pending)
                {
                    //Start task to find the export function definitions
                    this._funcDefFinderTask = WinFuncDefFinder.StartQueryTask(this.DefinitionFinderCompleted, this._currentInfo.FullName, this._currentInfo.Exports);
                    this._funcDefFinderTask.CurrentTask.Start();
                    return;
                }
            }
        }
        
        // This updates the side bar containing the portable executables information
        private void UpdateInfo()
        {
            // Update the name field
            this.Label_Name.Text = this._currentInfo.Name;

            //Update what type of portable executable file is being used
            if ((this._currentInfo.CoffHeaderCharacteristics & WindowsPeNet.PE_IMAGE_FILE_SYSTEM) != 0)
                this.Label_Type.Text = "System Driver";
            else if ((this._currentInfo.CoffHeaderCharacteristics & WindowsPeNet.PE_IMAGE_FILE_DLL) != 0)
                this.Label_Type.Text = "Dynamic Link Library";
            else if ((this._currentInfo.CoffHeaderCharacteristics & WindowsPeNet.PE_IMAGE_FILE_EXECUTABLE_IMAGE) != 0)
                this.Label_Type.Text = "Executable Image";
            else
                this.Label_Type.Text = "Unknown";

            //Update whether the portable executable is 32-bit or 64-bit
            if (this._currentInfo.PE32 == true)
                this.Label_Architecture.Text = "x32";
            else
                this.Label_Architecture.Text = "x64";

            //Update the image and os version
            this.Label_ImageVersion.Text = "ImageVersion:\n\t" + this._currentInfo.ImageVersion.ToString();
            this.Label_OSVersion.Text = "OS Version:\n\t" + this._currentInfo.OsVersion.ToString();
            
            //Update what subsystem is being used
            switch (this._currentInfo.Subsystem)
            {
                case WindowsPeNet.PE_IMAGE_SUBSYSTEM_NATIVE:
                    this.Label_Subsystem.Text = "Subsytem:\n\tNative";
                    break;
                case WindowsPeNet.PE_IMAGE_SUBSYSTEM_WINDOWS_CUI:
                    this.Label_Subsystem.Text = "Subsytem:\n\tWindows DOS";
                    break;
                case WindowsPeNet.PE_IMAGE_SUBSYSTEM_WINDOWS_GUI:
                    this.Label_Subsystem.Text = "Subsystem:\n\tWindows GUI";
                    break;
                default:
                    this.Label_Type.Text = "Unknown Subsystem";
                    break;
            }
        }

        // This loads all the rows
        private void LoadDataGridRows()
        {
            this.ResumeLayout();
            //Clear all the rows
            this.DataGrid_Options.Rows.Clear();

            //If no exports exist then exit the function
            if (this._currentInfo.Exports == null || this._currentInfo.Exports.Count == 0)
                return;
           
            //Add each export to a row
            for (int i = 0; i < this._currentInfo.Exports.Count; i ++)
            {
                this.DataGrid_Options.Rows.Add();

                //Set the tag of the row to not initialized(False)
                DataGridViewRow row = this.DataGrid_Options.Rows[i];
                row.Tag = false;

                //Set all required cells in the last added row
                this.PopulateRow(row, this._currentInfo.Exports[i]);
            }
            this.ResumeLayout();   
        }

        // This function makes it possible to not allow a row selection
        private void DataGrid_Options_SelectionChanged(object sender, EventArgs e)
        {
            //Clear any selected rows
            this.DataGrid_Options.ClearSelection();
        }

        // Called when the function definition view form is closed. 
        // This re-shows the patch form and removes any handlers it has associated with the function
        // definition view
        private void FormClosingEvent(object sender, EventArgs e)
        {
            Function_Definition_View funcdefview = (Function_Definition_View)sender;
            funcdefview.FormClosing -= FormClosingEvent;
            this.Show();
        }

        /// <summary>
        /// Called when the menu item to show function definitions is pressed.
        /// This orders that the patch form is hidden and the function definitions form is shown.
        /// A handler is attached to the function definitions form for when it closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_FunctionDefinitions_ItemClicked(object sender, EventArgs e)
        {
            //Create the function definition view 
            Function_Definition_View funcdefview = new Function_Definition_View(this._currentInfo);

            //Add the handler for when the function definition form is closed
            funcdefview.FormClosing += FormClosingEvent;

            //Show the function definition form
            funcdefview.Show();

            //Hide the patch form
            this.Hide();
        }

        // Called when the menu item create project is pressed.
        // This function creates a task that will create a new project that will be ready for patching
        private void ToolStripMenuItem_CreateProject_ItemClicked(object sender, EventArgs e)
        {
            //Specify where the source will be outputted when the created files are compiled
            string[] outpaths = new string[] { @"C:\Program Files\Cybernated\System Dlls\x32\", @"C:\Program Files\Cybernated\System Dlls\x64\",
                @"C:\Program Files\Cybernated\System Dlls\x32\Debug", @"C:\Program Files\Cybernated\System Dlls\x64\Debug"};

            this._currentInfo.AddSourceInclude("Program Communication\\Program-Server Communication.h",true);
            this._currentInfo.AddSourceInclude("Program Communication\\MessageIdentity.h",true);

            //Create the task
            _createProjectTask = ProjectCreator.CreateCppWinDllProject(this.CreateProject_StatusUpdate, SolutionPath, SolutionFilename, this._currentInfo, outpaths);
            TaskAwaiter<string> what = _createProjectTask.CurrentTask.GetAwaiter();

            //Assign the function to be called when the create project task finishes
            what.OnCompleted(this.CreateProjectCompletedEvent);

            // Start the create project task
            _createProjectTask.Start();

        }
        private void CreateProjectCompletedEvent()
        {
            this.Invoke((MethodInvoker)(() =>
            {
                //If an exception does not exist display the project name that created to the user
                if (_createProjectTask.CurrentTask.Exception == null)
                    MessageBox.Show(this, "Project Name:\n\t" + this._createProjectTask.CurrentTask.Result, "Created Project");

                //If an exception does exist then display the error message to the user
                else
                    MessageBox.Show(this, _createProjectTask.CurrentTask.Exception.Message, "Error Creating Project", MessageBoxButtons.OK);
                this.ToolStripStatusLabel_CreateProjectStatus.Visible = false;

            }));
        }
        private void DataGrid_Options_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != Column_ForwardTo.Index)
                return;
            DataGridViewCheckBoxCell cbcell = this.DataGrid_Options[e.ColumnIndex, e.RowIndex] as DataGridViewCheckBoxCell;
            DataGridViewCell ordcell = this.DataGrid_Options[Column_Ordinal.Index, e.RowIndex];
            DataGridViewCell versioncell = this.DataGrid_Options[Column_PatchedOSVersion.Index, e.RowIndex];
            PeExportNet export = null;
            ushort ordinal = (ushort)ordcell.Value;
            for(int i = 0; i < this._currentInfo.Exports.Count; i ++)
            {
                if (this._currentInfo.Exports[i].Ordinal != ordinal)
                    continue;
                export = this._currentInfo.Exports[i];
                break;
            }
            DataGridViewRow row = this.DataGrid_Options.Rows[e.RowIndex];
            cbcell.Value = !(bool)cbcell.Value;
            if ((bool)cbcell.Value == (bool)cbcell.TrueValue)
                export.SetForwardedTo((string)versioncell.Value);
            else
                export.SetForwardedTo(null);
        }

        public void CloseFormWithoutPrompt()
        {
            this._isClosing = true;
            if (this._funcDefFinderTask != null && this._funcDefFinderTask.CurrentTask.IsCompleted == false)
            {
                this._funcDefFinderTask.RemoveCompletionEvent(this.DefinitionFinderCompleted);
                this._funcDefFinderTask.Cancel();
            }
            if (this._createProjectTask != null && this._createProjectTask.CurrentTask.IsCompleted == false)
            {
                this._createProjectTask.Cancel();
            }
            this.FormClosing -= this.Form_Patch_FormClosing;
            if (this._currentInfo != null && this._currentInfo.Exports != null)
            {
                foreach (PeExportNet export in this._currentInfo.Exports)
                {
                    export.ExportChangedEvent -= this.ExportChangedEvent;
                    export.DefinitionChangingEvent -= this.ExportDefinitionChangingEvent;
                }
            }
            this.Close();
        }

        //Updates the row with the data
        private void PopulateRow(DataGridViewRow row, PeExportNet exportinfo)
        {
            foreach (DataGridViewCell x in row.Cells)
            {
                //Set the function name
                if (x.ColumnIndex == this.Column_FunctionName.Index)
                    x.Value = exportinfo.Name;

                //Set the ordinal number
                if (x.ColumnIndex == this.Column_Ordinal.Index)
                    x.Value = exportinfo.Ordinal;

                //Set the forwarded function name
                if (x.ColumnIndex == this.Column_Forwarded.Index)
                    x.Value = exportinfo.ForwardedFunctionName;

                //Set the existing patched versions
                if (x.ColumnIndex == this.Column_PatchedOSVersion.Index)
                {
                    DataGridViewComboBoxCell ccell = x as DataGridViewComboBoxCell;
                    ccell.Value = null;
                    List<string> versions = exportinfo.PatchedVersions;
                    if(versions != null)
                    {
                        ccell.DataSource = versions.ToArray();
                        ccell.Value = versions[0];
                    }
                   
                }

                //If the export is forwarded then select the forward to button
                if (x.ColumnIndex == this.Column_ForwardTo.Index)
                {
                    if (String.IsNullOrWhiteSpace(exportinfo.ForwardedTo) == true)
                        x.Value = false;
                    else
                        x.Value = true;
                }

                //If the function definition contains an error then display it
                if (String.IsNullOrWhiteSpace(exportinfo.DefinitionErrorMessage) == false)
                {
                    //If another error message exists then display both error messages
                    if (String.IsNullOrWhiteSpace(exportinfo.OtherErrorMessage) == false)
                        row.ErrorText = exportinfo.OtherErrorMessage + "\n" + exportinfo.DefinitionErrorMessage;
                    else
                        //Since only a function definition error message exists then only display that
                        row.ErrorText = exportinfo.DefinitionErrorMessage;
                }

                //If an error message exists then display it
                else if (String.IsNullOrWhiteSpace(exportinfo.OtherErrorMessage) == false)
                    row.ErrorText = exportinfo.OtherErrorMessage;
                else
                    row.ErrorText = null;

                //Determine the definition status
                switch(exportinfo.DefinitionStatus)
                {
                case ExportDefinitionStatus.Pending:
                case ExportDefinitionStatus.Timeout:
                    break;
                default:
                    if ((bool)row.Tag == false)
                    {
#if DEBUG
                        if (this.ToolStripProgressBar.Value == this.ToolStripProgressBar.Maximum)
                            return;
#endif
                        this.ToolStripProgressBar.Value++;
                        //Set the status message at the bottom of the screen to show the percentage completed
                        this.ToolStripStatusLabel_GettingFunctionDefinitions.Text = this.ToolStripProgressBar.Value.ToString() 
                            + " / " + this._currentInfo.Exports.Count.ToString() +" Export Function Definitions Processed ( " 
                            + Math.Round((decimal)this.ToolStripProgressBar.Value / 
                            this._currentInfo.Exports.Count * 100, 2).ToString() + "% )";
                        row.Tag = true;
                    }
                    break;
                }
            }
            return;
        }
        private void Form_Patch_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void Form_Patch_KeyUp(object sender, KeyEventArgs e)
        {
            this.Timer_Schroll.Stop();
            e.Handled = true;
        }
        private void Timer_Schroll_Tick(object sender, EventArgs e)
        {
            if(this._scrollDown == false)
            {
                if (this.DataGrid_Options.FirstDisplayedScrollingRowIndex != 0)
                    this.DataGrid_Options.FirstDisplayedScrollingRowIndex--;
            }
            else
            {
                int total = this.DataGrid_Options.DisplayedRowCount(false);
                if (this.DataGrid_Options.FirstDisplayedScrollingRowIndex + total > this.DataGrid_Options.Rows.Count)
                    return;
                this.DataGrid_Options.FirstDisplayedScrollingRowIndex++;
            }
        }
        private void DataGrid_Options_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                int total = this.DataGrid_Options.DisplayedRowCount(false);
                if (this.DataGrid_Options.FirstDisplayedScrollingRowIndex + total > this.DataGrid_Options.Rows.Count)
                    return;
                this.DataGrid_Options.FirstDisplayedScrollingRowIndex++;
                this._scrollDown = true;
                this.Timer_Schroll.Start();
            }
            else if (e.KeyCode == Keys.Up && this.DataGrid_Options.FirstDisplayedScrollingRowIndex != 0)
            {
                this.DataGrid_Options.FirstDisplayedScrollingRowIndex--;
                this._scrollDown = false;
                this.Timer_Schroll.Start();
            }
            
        }

        private void Form_Patch_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(this, " All unsaved progress will be lost",
                "Are you sure you want to exit?", MessageBoxButtons.YesNoCancel);

            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            this._isClosing = true;
            if (this._funcDefFinderTask != null && this._funcDefFinderTask.CurrentTask.IsCompleted == false)
            {
                this._funcDefFinderTask.RemoveCompletionEvent(this.DefinitionFinderCompleted);
                this._funcDefFinderTask.Cancel();
            }
                
            if(this._createProjectTask != null && this._createProjectTask.CurrentTask.IsCompleted == false)
            {
                this._createProjectTask.Cancel();
            }
                
            if(this._currentInfo != null && this._currentInfo.Exports != null)
            {
                foreach(PeExportNet export in this._currentInfo.Exports)
                {
                    export.ExportChangedEvent -= this.ExportChangedEvent;
                    export.DefinitionChangingEvent -= this.ExportDefinitionChangingEvent;
                }
            }
            return;
        }
        private void CreateProject_StatusUpdate(decimal progress)
        {
            this.Invoke( (MethodInvoker)(
                ()=> {
                    this.ToolStripStatusLabel_CreateProjectStatus.Visible = true;
                    if(progress != 1)
                    {
                        this.ToolStripStatusLabel_CreateProjectStatus.Text = "Creating project... " + Math.Round(progress * 100, 2).ToString() + "% Completed";
                    }
                    else
                    {
                        this.ToolStripStatusLabel_CreateProjectStatus.Text = null;
                    }
                }), null);
        }

        private void ToolStripMenuItem_Close_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_Patch_Load(object sender, EventArgs e)
        {
            
        }
    }
}
