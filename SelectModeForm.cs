using System;
using System.IO;
using System.Windows.Forms;
using StandardLibrary_CSharp;
using System.Threading;
using System.Threading.Tasks;
namespace PatchCodeCreator
{
    public partial class Form_SelectMode : Form
    {
        internal enum ModeResult
        {
            PatchCreate,
            Analyze,
            Cancel
        }
        internal ModeResult Result;
        public Form_SelectMode()
        {
            this.Result = ModeResult.Cancel;
            InitializeComponent();

        }
        private void SelectMode_Load(object sender, EventArgs e)
        {

        }
       
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void RadioButton_PatchCode_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButton_PatchCode.Checked == true)
                this.RadioButton_Analyze.Checked = false;
            else
                this.RadioButton_Analyze.Checked = true;
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Process_Click(object sender, EventArgs e)
        {
            if (this.RadioButton_Analyze.Checked == true)
                this.Result = ModeResult.Analyze;
            else
                this.Result = ModeResult.PatchCreate;
            this.Close();
        }
    }
}
