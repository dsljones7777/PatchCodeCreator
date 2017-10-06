using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WindowsPECLR;
namespace PatchCodeCreator
{

    internal partial class Function_Definition_View : Form
    {
        WindowsPeNet DatabaseInfo;
        public Function_Definition_View(WindowsPeNet pedb)
        {
            InitializeComponent();
            this.DataGridView_FunctionDefinitions.SuspendLayout();
            //Set the double buffered property
            PropertyInfo doubleprop = this.DataGridView_FunctionDefinitions.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleprop.SetValue(this.DataGridView_FunctionDefinitions, true);
            this.DataGridView_FunctionDefinitions.ResumeLayout();
            this.DatabaseInfo = pedb;
            if (this.DatabaseInfo.Exports == null)
                return;
            this.InitializeDB(pedb);
            for(int i = 0; i < DatabaseInfo.Exports.Count; i ++)
            {
                DatabaseInfo.Exports[i].DefinitionChangingEvent += this.UpdateFunctionDef;
            }
        }
        public bool UpdateFunctionDef(String  definition, PeExportNet what)
        {
            this.Invoke
                (
                    (MethodInvoker) (()=>
                    {
                        foreach (DataGridViewRow row in this.DataGridView_FunctionDefinitions.Rows)
                        {
                            ushort ordinal = (ushort)row.Cells[this.Column_FunctionOrdinal.Index].Value;
                            if (ordinal != what.Ordinal)
                                continue;
                            row.Cells[this.Column_FunctionDefinition.Index].Value = definition;
                            this.DataGridView_FunctionDefinitions.SuspendLayout();
                            this.DataGridView_FunctionDefinitions.AutoResizeColumn(this.Column_FunctionDefinition.Index);
                            this.DataGridView_FunctionDefinitions.ResumeLayout();
                            return;
                        }
                    })
                );
            return true;
           
        }
        private void InitializeDB(WindowsPeNet pedb)
        {
            this.DatabaseInfo = pedb;
            this.DataGridView_FunctionDefinitions.Rows.Add(pedb.Exports.Count);
            for(int i = 0; i < pedb.Exports.Count; i ++)
            {
                DataGridViewRow row = this.DataGridView_FunctionDefinitions.Rows[i];
                row.Cells[this.Column_FunctionName.Index].Value = pedb.Exports[i].Name;
                row.Cells[this.Column_FunctionOrdinal.Index].Value = pedb.Exports[i].Ordinal;
                row.Cells[this.Column_FunctionDefinition.Index].Value = pedb.Exports[i].Definition;
            }
            this.DataGridView_FunctionDefinitions.CellValueChanged += this.DataGridView_FunctionDefinitions_CellValueChanged;
        }
        private void DataGridView_FunctionDefinitions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.Column_FunctionDefinition.Index)
                return;
            DataGridViewRow row = this.DataGridView_FunctionDefinitions.Rows[e.RowIndex];
            ushort ordinal = (ushort)row.Cells[this.Column_FunctionOrdinal.Index].Value;
            string val = row.Cells[this.Column_FunctionDefinition.Index].Value as string;
            for(int i = 0; i< this.DatabaseInfo.Exports.Count; i++)
            {
                if (this.DatabaseInfo.Exports[i].Ordinal != ordinal)
                    continue;
                string oldname = this.DatabaseInfo.Exports[i].Definition;
                if (String.IsNullOrEmpty(val) == true)
                {
                    if (String.IsNullOrEmpty(oldname) == true)
                        return;
                }
                else
                {
                    if (String.IsNullOrEmpty(oldname) == false && String.Compare(oldname, val) == 0)
                        return;
                }
                this.DatabaseInfo.Exports[i].Definition = val;
                break;
            }
            this.DataGridView_FunctionDefinitions.SuspendLayout();
            this.DataGridView_FunctionDefinitions.AutoResizeColumn(this.Column_FunctionDefinition.Index);
            this.DataGridView_FunctionDefinitions.ResumeLayout();
        }

        private void Function_Definition_View_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DatabaseInfo == null)
                return;
            for(int i = 0; i < this.DatabaseInfo.Exports.Count; i ++)
            {
                this.DatabaseInfo.Exports[i].DefinitionChangingEvent -= UpdateFunctionDef;
            }
        }
    }
}
