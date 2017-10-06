using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using EasyWindowsProgressBar;
using System.Threading;
using System.Diagnostics;
using StandardLibrary_CSharp;
using PatchDatabaseAccessor;
using WindowsPECLR;

namespace PatchCodeCreator
{
    
    static class Program
    {
        private static AutoResetEvent LoadFormEvent = new AutoResetEvent(false);
        private static AutoResetEvent FormLoadedEvent = new AutoResetEvent(false);

        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           

            //Let User to Select which mode will be used
            Form_SelectMode.ModeResult result;
            start:
            using (Form_SelectMode selectmode = new Form_SelectMode())
            {
                Application.Run(selectmode);
                result = selectmode.Result;
            }
            if (result == Form_SelectMode.ModeResult.Cancel)
                return;
            String filename;
            using (OpenFileDialog dialogopen = new OpenFileDialog())
            {
                switch (result)
                {
                    case Form_SelectMode.ModeResult.Analyze:
                        dialogopen.Filter = "Executables (*.exe, *.msi, *.drv)|*.exe;*.msi|Dynamic Link Libraries (*.dll, *.sys)|*.dll;*.sys";
                        break;
                    case Form_SelectMode.ModeResult.PatchCreate:
                        dialogopen.Filter = "Dynamic Link Libraries (*.dll, *.sys)|*.dll;*.sys|Executables (*.exe, *.msi)|*.exe;*.msi";
                        break;
                }
                DialogResult openfileresult = dialogopen.ShowDialog();
                if (openfileresult != DialogResult.OK)
                    goto start;
                filename = dialogopen.FileName;
            }
            switch(result)
            {
                case Form_SelectMode.ModeResult.PatchCreate:
                    using (Form_Patch patchform = new Form_Patch())
                    {
                        patchform.Opacity = 0;
                        patchform.ShowInTaskbar = false;
                        //run a background task to open the file
                        Task inittask = new Task(InitPatchFormWProgress, new object[] { filename, patchform });
                        inittask.Start();
                        for (;;)
                        {
                            if (LoadFormEvent.WaitOne(250) == true)
                                break;
                            if (inittask.Exception != null)
                            {
                                DisplayError(inittask.Exception);
                                goto start;
                            }
                        }
                        FormLoadedEvent.Set();
                        Application.Run(patchform);
                    }
                    break;
                case Form_SelectMode.ModeResult.Analyze:
                    using (AnalyzeForm analyzeform = new AnalyzeForm())
                    {
                        analyzeform.Show();
                        try
                        {
                            analyzeform.AnalyzeDependencies(filename);
                        }
                        catch(ProgramError e)
                        {
                            throw e;
                        }
                        catch
                        {
                            throw;
                        }
                        Application.Run(analyzeform);
                    }
                    break;
                    
            }
            goto start;
        }
        private static void InitPatchForm(object args)
        {
            object[] realargs = (object[])args;
            string filename = (string)realargs[0];
            Form_EWProgressBar progressbar  = (Form_EWProgressBar)realargs[2];
            Form_Patch patchform = (Form_Patch)realargs[1];
            try
            {
                WindowsPeNet patchdb = new WindowsPeNet(filename,true,false);
                for (;;)
                {
                    if (progressbar.InvokeRequired == true)
                        break;
                    Thread.Sleep(50);
                }
                progressbar.Invoke((MethodInvoker)(() => { progressbar.UpdateTask("Loading Form", true); }));
                patchform.UpdateWithDatabase(patchdb);
                Program.LoadFormEvent.Set();
                for (;;)
                {
                    if (patchform.InvokeRequired == true)
                        break;
                    Thread.Sleep(50);
                }
                patchform.invokeStart();
                progressbar.Invoke((MethodInvoker)(() => { progressbar.UpdateTask("Finished", true); }));
            }
            catch
            {
                for (;;)
                {
                    if (progressbar.InvokeRequired == true)
                        break;
                    Thread.Sleep(50);
                }
                progressbar.Invoke((MethodInvoker)(() => { progressbar.Close(); }));
                throw;
            }
        }
        private static void InitPatchFormWProgress(object args)
        {
            object[] realargs = (object[])args;
            string filename = (string)realargs[0];
            Form_Patch patchform = (Form_Patch)realargs[1];
            Form_EWProgressBar progressbar = new Form_EWProgressBar(2);
            progressbar.UpdateTask("Opening File", false);
            Task openfiletask = new Task(InitPatchForm, new object[] { filename, patchform, progressbar });
            progressbar.Show();
            progressbar.WindowState = FormWindowState.Normal;
            openfiletask.Start();
            Application.Run(progressbar);
            openfiletask.Wait();
            if (openfiletask.Exception != null)
                throw openfiletask.Exception;
        }
        //returns true to continue, false to exit program
        private static void DisplayError(AggregateException e)
        {
            MessageBox.Show(e.InnerException.Message);
            Application.Exit();
        }
        private static void LoadProgressForm(ref Form_EWProgressBar what, string taskname, int numberoftasks)
        {

            Thread newthread = new Thread(
                 (object val) =>
                    {
                        Form_EWProgressBar bar = (Form_EWProgressBar)val;
                        bar = new Form_EWProgressBar(numberoftasks);
                        bar.UpdateTask(taskname, false);
                        bar.Show();
                        bar.WindowState = FormWindowState.Normal;
                        Application.Run(bar);
                    }
                );
            newthread.IsBackground = false;
            newthread.SetApartmentState(ApartmentState.STA);
            newthread.Start(what);
            return;
        }
    }
}
