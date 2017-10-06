using System;
using System.Text;
using EnvDTE;
using EnvDTE80;
using EnvDTE100;
using System.IO;
using System.Runtime.InteropServices;
using WindowsPECLR;
using StandardLibrary_CSharp;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace PatchCodeCreator
{
    // This class creates a visual studio project and adds it to a solution
    // Currently C++ projects are only supported
    internal class ProjectCreator : IDisposable
    {
        
        //Sets the percent completed 0 is 0%; 1 is 100%
        public delegate void pSetPercentCompleted(decimal percent);

        public static CancelableResultTask<string[]> CreateCppCode(string outpath, WindowsPeNet info)
        {



            return null;
        }

        // Creates a task that will open a solution and create a project that is capable of patching a windows portable executable file
        public static CancelableResultTask<string> CreateCppWinDllProject(pSetPercentCompleted percentcompleted,
            string solutionpath, string solutionname, WindowsPeNet info, string[] outputpaths)
        {
            CancelableResultTask<string> createprojecttask = new CancelableResultTask<string>(
                (ref CancelAsyncToken token) =>
                {
                    //Set the percent completed to 0
                    percentcompleted(0);

                    //Make sure the exports are ready
                    if (ProjectCreator.IsReady(info) == false)
                        throw new ProgramError(ProgramSection.WindowPe_CSHRP, 0, "The exports are not ready to be processed");

                    //Set the percent completed to 1/15
                    percentcompleted((decimal)1 / 15);
                    using (ProjectCreator creator = new ProjectCreator())
                    {
                        //Set the percent completed to 1/3 (Since opening the DTE usually takes the longest
                        percentcompleted((decimal)5 / 15);

                        //Check to see if the token calls for this function to be canceled
                        if (token.ShouldICancel() == true)
                            throw new TaskCanceledException();

                        //Load the solution
                        creator.LoadSolution(solutionpath, solutionname);

                        //Set the percent completed to 7/15
                        percentcompleted((decimal)7 / 15);

                        //Check to see if the token calls for this function to be canceled
                        if (token.ShouldICancel() == true)
                            throw new TaskCanceledException();

                        //Create a unique file name for the project
                    createnewprojectname:
                        creator._projectName = creator.CreateUniqueFileName(outputpaths, info.Name, ".dll");
                        creator._projectDirectory = creator._solutionDirectory + creator._projectName + '\\';
                        if (Directory.Exists(creator._projectDirectory) == true)
                            goto createnewprojectname;

                        //Check to see if the token calls for this function to be canceled
                        if (token.ShouldICancel() == true)
                            throw new TaskCanceledException();

                        //Create the project directory
                        DirectoryInfo dirinfo = Directory.CreateDirectory(creator._projectDirectory);

                        //Set the percent completed to 8/15
                        percentcompleted((decimal)8 / 15);
                       
                        //Create the code files and record the filenames that are created
                        string[] filenames = null;
                        try
                        {
                            //Create the code files
                            filenames = creator.CreateCppCodeFiles(info, creator._projectName);

                            //Set the percent completed to 11/15
                            percentcompleted((decimal)11 / 15);

                            //Check to see if the token calls for this function to be canceled
                            if (token.ShouldICancel() == true)
                                throw new TaskCanceledException();
                            //Create a new project from the template
                            if (info.PE32 == true)
                                creator._currentProject = creator._currentSolution.AddFromTemplate(ProjectCreator.CPP_WIN32_TEMPLATE_LOCATION, creator._projectDirectory, creator._projectName + ".vcxproj");
                            else
                                creator._currentProject = creator._currentProject = creator._currentSolution.AddFromTemplate(ProjectCreator.CPP_WIN64_TEMPLATE_LOCATION, creator._projectDirectory, creator._projectName + ".vcxproj");
                            //Set the percent completed to 13/15
                            percentcompleted((decimal)13 /15);
                        }
                        catch
                        {
                            //On an error delete the directory which will delete the code files created as well
                            dirinfo.Delete(true);
                            throw;
                        }
                        try
                        {
                            //Check to see if the token calls for this function to be canceled
                            if (token.ShouldICancel() == true)
                                throw new TaskCanceledException();

                            //Add each code file into the new project
                            foreach (string x in filenames)
                            {
                                creator._currentProject.ProjectItems.AddFromFile(x);
                            }

                            //Set the percent completed to 14/15
                            percentcompleted((decimal)14 / 15);

                            //Save the project and solution
                            creator._currentProject.Save();
                            creator._currentSolution.Close(true);
                            creator._currentSolution = null;

                            //Set the percent completed to 1
                            percentcompleted((decimal)1);
                        }
                        catch
                        {
                            //On an error remove the project from the solution and delete the directory
                            try
                            {
                                creator._currentSolution.Remove(creator._currentProject);
                            }
                            catch
                            {
                            }
                            dirinfo.Delete(true);
                            throw;
                        }
                        return creator._projectName;
                    }
                },
                System.Threading.ApartmentState.STA,
                null
                );
            return createprojecttask;
        }

        // The constructor initalized the development tool environment and registers the message filter for it
        protected ProjectCreator()
        {
            this._randomGenerator = new Random((int)DateTime.Now.Ticks);
            
            System.Type type = System.Type.GetTypeFromProgID("VisualStudio.DTE.14.0");
            Object obj = System.Activator.CreateInstance(type, true);
            this._devToolEnviron = (DTE2)obj;
            try
            {
                MessageFilter.Register();
            }
            catch
            {
                this._devToolEnviron.Quit();
                throw;
            }
        }

        // The default C++ template to use when creating a project
        private const string CPP_WIN32_TEMPLATE_LOCATION = @"C:\Program Files\Cybernated\Templates\Win32PatchDll\Win32PatchDll\Win32PatchDll.vcxproj";

        private const string CPP_WIN64_TEMPLATE_LOCATION = @"C:\Program Files\Cybernated\Templates\Win64PatchDll\Win64PatchDll\Win64PatchDll.vcxproj";

        // Characters to use when creating a function name
        private const string RANDOM_FUNCTION_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Characters to use when creating a filename
        private const string RANDOM_NAME_CHARS = "abcdefghijklmnopqrstuvwxyz0123456789";

        // Loads the solution located at path\filename
        private void LoadSolution(string path, string filename)
        {
            this._devToolEnviron.Solution.Open(path + filename);
            this._currentSolution = (Solution)this._devToolEnviron.Solution;
            this._solutionDirectory = path;
        }
       
        
        // Random number generator used for creating new function names and file names
        private Random _randomGenerator;

        
        // The development tool environment. It is used to open solutions, and create projects
        private DTE2 _devToolEnviron;


        // The current solution that is opened with the development tool environment
        private Solution _currentSolution;

        
        // The directory that the current solution is open in
        private string _solutionDirectory;


        // The project that is currently opened with the development tool environment
        private Project _currentProject;


        // The directory of the current project opened
        private string _projectDirectory;

        // The current project's name. Does not include the extension of the project
        private string _projectName;
        
        // Creates a unique function name that is guaranteed to start with a letter
        private void CreateUniqueFunctionName(WindowsPeNet peinfo, PeExportNet exporttocreatename)
        {
            string oldname = exporttocreatename.Name;

            //If the original function name is null then the patched name will be the ordinal_ + the ordinal number
            if(String.IsNullOrWhiteSpace(oldname) == true)
            {
                exporttocreatename.PatchedName = "ordinal_" + exporttocreatename.Ordinal.ToString();
                return;
            }

            //Create a string builder that has the same capacity of the original function name
            StringBuilder builder = new StringBuilder(oldname.Length,oldname.Length);
            builder.Length = oldname.Length;
            int index;
            string newname;
        startcreatename:
            
            //Make sure the first character of the new patch name is a letter and not a number (C++ does not allow for numbers as the function name)
            index = this._randomGenerator.Next(ProjectCreator.RANDOM_FUNCTION_CHARS.Length - 10);
            builder[0] = ProjectCreator.RANDOM_FUNCTION_CHARS[index];

            //Select a random letter or number for remaining characters
            for(int i = 1; i < oldname.Length; i ++)
            {
                index = this._randomGenerator.Next(ProjectCreator.RANDOM_FUNCTION_CHARS.Length);
                builder[i] = ProjectCreator.RANDOM_FUNCTION_CHARS[index];
            }

            newname = builder.ToString();

            //Make sure the patched name is not the same as original function name
            if (String.CompareOrdinal(oldname, newname) == 0)
                goto startcreatename;

            //Make sure the patch name does not already exist in all the other functions in the patch library
            for ( int i = 0; i < peinfo.Exports.Count; i ++)
            {
                PeExportNet export = peinfo.Exports[i];

                //If the references are the same then don't compare the names
                if (Object.ReferenceEquals(export, exporttocreatename) == true)
                    continue;

                //Make sure the new patch name is not an existing patch name
                if (String.IsNullOrWhiteSpace(export.PatchedName) == false && String.CompareOrdinal(newname, export.PatchedName) == 0)
                    goto startcreatename;
                if (String.IsNullOrWhiteSpace(export.Name) == false && string.CompareOrdinal(newname, export.Name) == 0)
                    goto startcreatename;
            }
            exporttocreatename.PatchedName = newname;
        }

        // Creates a unique file name. The file name created is guaranteed to not be in the paths specified
        private string CreateUniqueFileName(string [] paths, string originalfilename, string extension)
        {
            StringBuilder builder = new StringBuilder(originalfilename.Length, originalfilename.Length);
            builder.Length = originalfilename.Length;
            string newfilename;
            string filenamewext;
        createfilename_label:
            for(int i = 0; i < originalfilename.Length; i ++)
            {
                int index = this._randomGenerator.Next(ProjectCreator.RANDOM_NAME_CHARS.Length);
                builder[i] = ProjectCreator.RANDOM_NAME_CHARS[index];
            }
            newfilename = builder.ToString();
            filenamewext = newfilename + extension;
            foreach(string directory in paths)
            {
                if (File.Exists(directory + filenamewext) == true)
                    goto createfilename_label;
            }
            return newfilename;
        }

        // Takes the old function definition and modifies the function name to reflect the patched name.
        private string GetNewFunctionDefinition(PeExportNet export)
        {
            if (String.IsNullOrWhiteSpace(export.Name) == true)
                return null;
            if (String.IsNullOrWhiteSpace(export.ForwardedTo) == false)
                return null;
            int index = export.Definition.IndexOf(export.Name);
            StringBuilder stringbuild = new StringBuilder(export.Definition);
            for(int i = 0; i < export.PatchedName.Length; i ++)
            {
                stringbuild[index + i] = export.PatchedName[i];
            }
            return stringbuild.ToString().Trim();
        }

        // This function creates a module definition file, header file, and source file for C++ code. The information for these files
        // is derived from the exports of a portable executable file. A patch name is created for every export unless the export
        // is forwarded. The function definitions are modified to reflect the patch name.
        private string[] CreateCppCodeFiles(WindowsPeNet info, string name)
        {
            //Create the header, source, and export files
            string headerpath = this._projectDirectory + name + ".h";
            string sourcepath = this._projectDirectory + name + ".cpp";
            string exportspath = this._projectDirectory + "Exports.cpp";
            using (StreamWriter headerstream = new StreamWriter(headerpath, false, Encoding.Unicode))
            using (StreamWriter sourcestream = new StreamWriter(sourcepath, false, Encoding.Unicode))
            using (StreamWriter exportstream = new StreamWriter(exportspath, false, Encoding.Unicode)) 
            {
                //Write the beginning of the header (pragma once, etc.)
                headerstream.WriteLine("#pragma once");
                headerstream.WriteLine(@"/*");

                //Write this product's details (program name, program version)
                headerstream.Write("\tAutocreated header by: ");
                headerstream.WriteLine(Application.ProductName);
                headerstream.Write("\tVersion: ");
                headerstream.WriteLine(Application.ProductVersion);
                //Write the original dll the patch was made for (Filename, os version, image version)
                headerstream.Write("\n\tCreated for ");
                headerstream.WriteLine(info.FullName);
                headerstream.Write("\tOS Version: ");
                headerstream.WriteLine(info.OsVersion.ToString());
                headerstream.Write("\tImage Version: ");
                headerstream.WriteLine(info.ImageVersion.ToString());
                headerstream.WriteLine(@"*/");

                //Write all the includes for the header file
                for(int i = 0; i < info.TotalHeaderIncludes; i ++)
                {
                    headerstream.Write("#include");
                    headerstream.WriteLine(info.get_HeaderIncludeName(i));
                }
                headerstream.WriteLine("extern \"C\" {");

                //Include required headers for source and export code files (stdafx etc...)
                sourcestream.WriteLine("#include \"stdafx.h\"");
                exportstream.WriteLine("#include \"stdafx.h\"");
                sourcestream.Write("#include \"");
                sourcestream.Write(name);
                sourcestream.WriteLine(".h\"");

                //Write all the includes for the source file
                for(int i = 0; i < info.TotalSourceIncludes; i ++)
                {
                    sourcestream.Write("#include ");
                    sourcestream.WriteLine(info.get_SourceIncludeName(i));
                }
                sourcestream.WriteLine();

                for ( int i = 0; i < info.Exports.Count; i ++ )
                {
                    PeExportNet export = info.Exports[i];

                    //Create the patched function name
                    this.CreateUniqueFunctionName(info, export);

                    string funcdef = GetNewFunctionDefinition(export);
                    string ordinal = export.Ordinal.ToString();

                    //Write the export to the exports source ( #pragma comment(linker, "/EXPORT:") )
                    exportstream.Write("#pragma comment(linker, \"/EXPORT:");
                    exportstream.Write(export.PatchedName);
                    exportstream.Write("=");

                    //If the export function is forwarded then write that and loop
                    if (String.IsNullOrWhiteSpace(export.ForwardedTo) == false)
                    {
                        exportstream.Write(export.ForwardedTo);
                        exportstream.Write(",@");
                        exportstream.Write(ordinal);
                        exportstream.WriteLine("\")");
                        continue;
                    }

                    //Write the patched name since the export is not forwarded
                    exportstream.Write(export.PatchedName);
                    exportstream.Write(",@");
                    exportstream.Write(ordinal);
                    exportstream.WriteLine("\")");

                    //Write header definition
                    headerstream.Write(@"// From original function: ");
                    headerstream.Write(export.Name);
                    headerstream.Write(" @");
                    headerstream.WriteLine(ordinal);
                    headerstream.WriteLine(funcdef);
                    
                    //Write source definition
                    sourcestream.Write(@"// From original function: ");
                    sourcestream.Write(export.Name);
                    sourcestream.Write(" @");
                    sourcestream.WriteLine(ordinal);
                    sourcestream.WriteLine(funcdef.ToCharArray(), 0, funcdef.Length - 1);
                    sourcestream.WriteLine("{\n};");
                }
                headerstream.WriteLine("};");

                exportstream.Write("\r\n");
                headerstream.Write("\r\n");
                sourcestream.Write("\r\n");
            }
            return new string[] { headerpath, sourcepath, exportspath };
        }

        // Determines if each export function is ready to be patched
        private static bool IsReady(WindowsPeNet peinfo)
        {
            for(int i = 0; i < peinfo.Exports.Count;i ++)
            {
                PeExportNet patchinfo = peinfo.Exports[i];
                if (patchinfo.IsValidForPatch() == false)
                    return false;
            }
            return true;
        }

        // This class is used to intercept messages sent from the development tool environment
        private class MessageFilter : IOleMessageFilter
        {
            public static void Register()
            {
                IOleMessageFilter newFilter = new MessageFilter();
                IOleMessageFilter oldFilter = null;
                uint err = unchecked((uint)CoRegisterMessageFilter(newFilter, out oldFilter));
                if (err != 0)
                    throw new ProgramError(ProgramSection.PatchCodeCreator_CSHRP, 0, "Failed to register Message Filter with the Development Tool Environment.\nHRESULT:\t 0x" + err.ToString("X"));
                return;
            }
            public static void Revoke()
            {
                IOleMessageFilter oldFilter = null;
                CoRegisterMessageFilter(null, out oldFilter);
            }
            int IOleMessageFilter.HandleInComingCall(int dwCallType,
              System.IntPtr hTaskCaller, int dwTickCount, System.IntPtr
              lpInterfaceInfo)
            {
                
                return 0;
            }

            int IOleMessageFilter.RetryRejectedCall(System.IntPtr
              hTaskCallee, int dwTickCount, int dwRejectType)
            {
                if (dwRejectType == 2)
                  return 1000;
                return -1;
            }

            int IOleMessageFilter.MessagePending(System.IntPtr hTaskCallee,
              int dwTickCount, int dwPendingType)
            {
                
                return 2;
            }

            // Implement the IOleMessageFilter interface.
            [DllImport("Ole32.dll")]
            private static extern int
              CoRegisterMessageFilter(IOleMessageFilter newFilter, out
              IOleMessageFilter oldFilter);
        }


        // This interface is used to filter messages from the development tool environment
        [ComImport(), Guid("00000016-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        interface IOleMessageFilter
        {
            [PreserveSig]
            int HandleInComingCall(
                int dwCallType,
                IntPtr hTaskCaller,
                int dwTickCount,
                IntPtr lpInterfaceInfo);

            [PreserveSig]
            int RetryRejectedCall(
                IntPtr hTaskCallee,
                int dwTickCount,
                int dwRejectType);

            [PreserveSig]
            int MessagePending(
                IntPtr hTaskCallee,
                int dwTickCount,
                int dwPendingType);
        }

        public void Dispose()
        {
            if(this._currentSolution != null)
                this._currentSolution.Close();
            if (this._devToolEnviron != null)
                this._devToolEnviron.Quit();
            MessageFilter.Revoke();
        }
    }
    
}
