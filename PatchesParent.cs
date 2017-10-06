using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsPECLR;

namespace PatchCodeCreator
{
    public partial class PatchesParent : Form
    {
        List<Form_Patch> _patchForms;
        int _currentIndex;
        public PatchesParent(List<WindowsPeNet> pelist)
        {
            InitializeComponent();
            this._patchForms = new List<Form_Patch>();
            foreach (WindowsPeNet info in pelist)
            {
                Form_Patch patchform = new Form_Patch();
                //Set the patch form display values
                //The window will be maximized without a title or control bar
                //It will not show up in the taskbar and it will fill the control
                patchform.TopLevel = false;
                patchform.WindowState = FormWindowState.Maximized;
                patchform.FormBorderStyle = FormBorderStyle.None;
                patchform.MaximizeBox = false;
                patchform.ShowInTaskbar = false;
                patchform.Dock = DockStyle.Fill;
                patchform.TopMost = false;
                patchform.Size = this.PatchFormsPanel.Size;

                //Load the current pe info
                patchform.UpdateWithDatabase(info);

                //Add an event to clean up resource for when the form is closed
                patchform.FormClosed += Patchform_FormClosed;

                //Add the patch form to the controls and to the created list
                this.PatchFormsPanel.Controls.Add(patchform);
                this._patchForms.Add(patchform);
            }
        }

        //Event Handler for when a patch form is closed
        private void Patchform_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Remoce the patch from from the saved list
            Form_Patch closedform = sender as Form_Patch;
            this._patchForms.Remove(closedform);
            closedform.Dispose();

            //If no other patch forms exists then exit the window
            if (this._patchForms.Count == 0)
            {
                this.Close();
                return;
            }

            //If the index that was removed was the last index then show the last item
            if (this._currentIndex >= this._patchForms.Count)
                this.LastButton_Click(null, null);
            //Instead of trying to determine where the patch form index was at then
            //show the patch form at the current index
            else
            {
                this._patchForms[this._currentIndex].Show();
                this._patchForms[this._currentIndex].BringToFront();
            }   
        }

        //Enables/Disables the last or first button
        void EnableDisableButtons()
        {
            //If the current index is 0 then the First Button should not be shown
            if (this._currentIndex == 0)
            {
                this.FirstButton.Enabled = false;
                this.LastButton.Enabled = true;
            }

            //If the current index is the last index then the Last Button should not be shown
            else if (this._currentIndex == this._patchForms.Count - 1)
            {
                this.LastButton.Enabled = false;
                this.FirstButton.Enabled = true;
            }

            //Since neither previous condition is true then show both buttons
            else
            {
                this.FirstButton.Enabled = true;
                this.LastButton.Enabled = true;
            }
        }
        private void FirstButton_Click(object sender, EventArgs e)
        {
            //Hide the current index and show the first index
            this._patchForms[this._currentIndex].Hide();
            Form_Patch patchform = this._patchForms[0];
            patchform.BringToFront();
            patchform.Show();
            patchform.Size = this.PatchFormsPanel.Size;
            this._currentIndex = 0;
            this.EnableDisableButtons();
        }

        private void LastButton_Click(object sender, EventArgs e)
        {
            //Hide the current index and show the last index
            this._patchForms[this._currentIndex].Hide();
            Form_Patch patchform = this._patchForms[this._patchForms.Count - 1];
            patchform.BringToFront();
            patchform.Show();
            LastButton.Focus();
            patchform.Size = this.PatchFormsPanel.Size;
            this._currentIndex = this._patchForms.Count - 1;
            this.EnableDisableButtons();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            //Hide the current index and show the next index. If the next index is greater than the current index
            //then loop around to the beginning
            this._patchForms[this._currentIndex].Hide();
            this._currentIndex++;
            if (this._currentIndex >= this._patchForms.Count)
                this._currentIndex = 0;
            Form_Patch patchform = this._patchForms[this._currentIndex];
            patchform.Show();
            patchform.BringToFront();
            patchform.Size = this.PatchFormsPanel.Size;
            this.EnableDisableButtons();
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            //Hide the current index and show the previous index. If the previous index is less than 0 then loop
            //around to the last index
            this._patchForms[this._currentIndex].Hide();
            this._currentIndex--;
            if (this._currentIndex < 0)
                this._currentIndex = this._patchForms.Count - 1;
            Form_Patch patchform = this._patchForms[this._currentIndex];
            patchform.Show();
            patchform.BringToFront();
            patchform.Size = this.PatchFormsPanel.Size;
            this.EnableDisableButtons();
        }

        private void PatchesParent_Shown(object sender, EventArgs e)
        {
            //Set the current patch form to the first index
            this.FirstButton_Click(null, null);
        }

        //Called when the Patch Parents Form is closing
        private void PatchesParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Close each patch form without a prompt
            foreach(Form_Patch patchform in this._patchForms)
            {
                patchform.FormClosed -= this.Patchform_FormClosed;
                patchform.CloseFormWithoutPrompt();
                patchform.Dispose();
                continue;
            }
            return;
        }

        //Event Handler for when the form is being resized
        private void PatchesParent_Resize(object sender, EventArgs e)
        {
            //Resize the current patch form
            this._patchForms[this._currentIndex].Size = this.PatchFormsPanel.Size;
        }
    }
}
