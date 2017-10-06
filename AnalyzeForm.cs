using StandardLibrary_CSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsPECLR;
namespace PatchCodeCreator
{

    public partial class AnalyzeForm : Form
    {
        //List of directories to search for dynamic link libraries 
        internal static List<string> DllSearchPathsx64;
        internal static List<string> DllSearchPathsx32;

        //Static initializer that creates and initializes the dll search directories
        static AnalyzeForm()
        {
            //Initialize the list of nodes that represent each dll the file imports
            AnalyzeForm._loadedPE = new List<WindowsPeTreeNode>();

            //Create the image list for the nodes
            AnalyzeForm._imageList = new ImageList();
            AnalyzeForm._imageList.ImageSize = new Size(32, 32);
            AnalyzeForm._imageList.Images.Add(PatchCodeCreator.Properties.Resources.UnknownImage);


            //Initialize the list of directories to search
            DllSearchPathsx64 = new List<string>();
            DllSearchPathsx32 = new List<string>();

            //Add the directory where dlls are copied to the search directory
            DllSearchPathsx64.Add(@"C:\Temp\Dlls\x64");
            DllSearchPathsx32.Add(@"C:\Temp\Dlls\x32");

            //Add the system folder to the search directory
            string syspath64 = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\";
            string syspath32 = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\";
            
            DllSearchPathsx64.Add(syspath64);
            DllSearchPathsx32.Add(syspath32);

            //Add each folder and every child folder in the system folder to the search paths
            IEnumerable<string> dir = Directory.EnumerateDirectories(syspath64);
            foreach (string path in dir)
            {
                AddPaths(DllSearchPathsx64,path);
            }
            dir = Directory.EnumerateDirectories(syspath64);
            foreach(string path in dir)
            {
                AddPaths(DllSearchPathsx64, path);
            }
           
        }

        //Recursive function that adds every child directory of the supplied path 
        //and their child directories to the list
        static void AddPaths(List<string> listptr, string path)
        {
            //Get an enumerator for each directory in the path. If the function fails (due to access violations)
            //then just return
            IEnumerable<string> dir = null;
            try
            {
                dir = Directory.EnumerateDirectories(path);
            }
            catch
            {
                return;
            }

            //Add the current path to the list
            listptr.Add(path + "\\");

            //Add each sub directory in the path to the list
            foreach (string newpath in dir)
            {
                AddPaths(listptr,newpath);
            }
        }

        //Constructor that initializes the image list of the portable executable
        public AnalyzeForm()
        {
            InitializeComponent();

            //Create the image list for the nodes
            this.treeView_DependencyChain.ImageList = AnalyzeForm._imageList;
        }

        //Creates a root tree node that represents the specified file (should be a PE file)
        //Creates child node that represents each dependency of the specified file. Each child node
        //Contains the dependencies of that dll
        public void AnalyzeDependencies(string filename)
        {
            //Create the root node
            WindowsPeTreeNode treenode = new WindowsPeTreeNode();

            //Open the file specified. Since it is not known whether the file is x32, or x64 specify -1 for the value
            //isx32
            treenode.Open(filename, -1);

            //Add the file to the list of pe file nodes
            AnalyzeForm._loadedPE.Add(treenode);

            //Find all the dependecies of the opened file
            treenode.FindOnlyTopLevelDependencies();

            //Set the root node as the opened file
            this.treeView_DependencyChain.Nodes.Add(treenode);

            //Set the root node checked
            treenode.Checked = true;

            //Set each of the child nodes check boxes of the root node to checked by default
            foreach(TreeNode childnode in treenode.Nodes)
            {
                if (childnode == null)
                    continue;
                childnode.Checked = true;
            }

            //Add the event handler for when an item is checked
            this.treeView_DependencyChain.BeforeCheck += this.treeView_DependencyChain_BeforeCheck;
        }
        internal static ImageList _imageList;
        //List of opened portable executable files as tree nodes
        internal static List<WindowsPeTreeNode> _loadedPE;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createPatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Hide the current form (will be closed later)
            this.Hide();

            //Create a list of all the checked items
            List<WindowsPeNet> nodestopatch = new List<WindowsPeNet>();

            //Find all the checked child nodes of the root node
            WindowsPeTreeNode rootnode = (WindowsPeTreeNode)this.treeView_DependencyChain.Nodes[0];

            //FileNotFoundException will be thrown if the user cancels due to a file not being loaded
            try
            {
                this.AddCheckedToList(rootnode, nodestopatch);
            }
            catch(FileNotFoundException)
            {
                return;
            }

            //Create a parent mdi form
            PatchesParent mdiform = new PatchesParent(nodestopatch);

            //show the parent midi form.
            mdiform.ShowDialog();

            //Close the anaylze form
            this.Close();
        }

        private void AddCheckedToList(TreeNode topnode, List<WindowsPeNet> patchlist)
        {
            //Get each checked node that is castable to a WindowsPeTreeNode
            var query = from TreeNode childnode in topnode.Nodes.AsParallel()
                         where (childnode.Checked == true)
                         select childnode;

            WindowsPeTreeNode toplevelpenode = topnode as WindowsPeTreeNode;
            if(toplevelpenode == null)
            {
                toplevelpenode = AnalyzeForm._loadedPE.Find(
                       (WindowsPeTreeNode loadednode) =>
                       {
                           return String.Equals(topnode.Text, loadednode.NodePe.FullName,StringComparison.CurrentCultureIgnoreCase);
                       });
            }


            //Enumerate through each checked node. Remove unused exports and forwarded exports
            foreach (TreeNode node in query)
            {
                WindowsPeTreeNode penode = node as WindowsPeTreeNode;

                //If the cast failed then pe info is located in the loaded pe files.
                //Find it by comparing names
                if (penode == null)
                {
                    penode = AnalyzeForm._loadedPE.Find(
                        (WindowsPeTreeNode loadednode) =>
                        {
                            return String.Equals(node.Text, loadednode.Text,StringComparison.CurrentCultureIgnoreCase);
                        });
                }
                WindowsPeNet peinfo = penode.NodePe;

                //If a pe file was not loaded (could not be found) then notify the user
                if (penode.NodePe == null)
                {
                    DialogResult result = MessageBox.Show("Could Not Open " + node.Text, " Error Loading File. Do You Want to Continue?", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes)
                        throw new FileNotFoundException("Operation Canceled by User");
                    continue;
                }


                if (patchlist.Exists(
                    (WindowsPeNet info)=> 
                    {
                        if (info == peinfo)
                            return true;
                        return false;
                    }) == true)
                {
                    continue;
                }

                //Find all import dlls that correspond to the node
                var importquery = from PeImportDllNet importdll in toplevelpenode.NodePe.Imports.AsParallel()
                                  where (String.Equals(peinfo.Name, importdll.Name.Substring(0, importdll.Name.IndexOf('.')), StringComparison.CurrentCultureIgnoreCase) == true)
                                  select importdll;

                peinfo.Exports.RemoveAll(
                    (PeExportNet export) =>
                    {
                        bool found = false;

                        //Look through each import dll that matches the child node dll name
                        foreach (PeImportDllNet importdll in importquery)
                        {
                            //Find the import function that matches
                            PeImportNet foundimport = importdll.Functions.Find(
                                (PeImportNet import) =>
                                {
                                //If the import is import by name then see if the names are equal
                                if (String.IsNullOrWhiteSpace(import.Name) == false)
                                    {
                                        if (String.IsNullOrWhiteSpace(export.Name) == true)
                                            return false;
                                        return String.Equals(export.Name, import.Name);
                                    }
                                //Since the import is not import by name then it is import by ordinal
                                else
                                    {
                                        if (import.Ordinal == export.Ordinal)
                                            return true;
                                    }
                                    return false;
                                });
                            //If an import was found that matches the current export then mark found as true and stop searching
                            if (foundimport != null)
                                return false;
                        }
                        return !found;
                    });
                patchlist.Add(peinfo);
            }

            //Get each unchecked child node
            query = from TreeNode childnode in topnode.Nodes.AsParallel()
                    where (childnode.Checked == false)
                    select childnode;
            foreach(TreeNode node in query)
            {
                if (node.Nodes == null || node.Nodes.Count == 0)
                    continue;
                AddCheckedToList(node, patchlist);
            }
        }

        //Recursive functions that unchecks every child node
        private void UncheckChildren(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = false;
                UncheckChildren(child);
            }
        }

        //Recursive function that sets checkval on all child nodes that share the same name
        private void SetCheckOnAll(TreeNode start,String name,bool checkval)
        {
            //Set the check value on each child node with the same name
            foreach(TreeNode child in start.Nodes)
            {
                //If the child node does not have the same name, then search all of it's child nodes
                if (child.Text.Equals(name) == false)
                {
                    SetCheckOnAll(child, name,checkval);
                    continue;
                }

                child.Checked = checkval;

                //If the node is unchecked now then uncheck all the child nodes of the node regardless of their name
                if(checkval == true)
                    UncheckChildren(child);
            }
        }

        //Called before the check mark is added or removed
        private void treeView_DependencyChain_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            //The check value is going to be opposite the current value since the value has not been set
            bool checkval = !e.Node.Checked;

            //Remove the before check event handler so this function is not called while setting other check mark
            //values
            this.treeView_DependencyChain.BeforeCheck -= this.treeView_DependencyChain_BeforeCheck;

            //Set the same value on other nodes that are the same pe file (Will have the same name)
            SetCheckOnAll(this.treeView_DependencyChain.Nodes[0], e.Node.Text, checkval);

            //Add the before check event handler back to the function
            this.treeView_DependencyChain.BeforeCheck += this.treeView_DependencyChain_BeforeCheck;

            //Cancel the check operation since the check mark was already changed
            e.Cancel = true;
        }
    }

    //Tree node representation of a portable executable file
    class WindowsPeTreeNode : TreeNode
    {
        
        [DllImport("Shlwapi.dll")]
        static extern bool PathFileExistsW([MarshalAs(UnmanagedType.LPWStr)]string path);

        //The underlying PE file that holds information about the executable
        public WindowsPeNet NodePe;

        public WindowsPeTreeNode()
        {
            this.NodePe = null;
            this.Checked = false;
        }

        private void LoadDependenciesEvent(object sender, EventArgs eargs)
        {
            this.FindOnlyTopLevelDependencies();
        }
        //Opens the specified filename.
        //The isx32 variable is 0 if the file is an x64 pe file
        //The isx32 variable is > 0 if the file is an x32 pe file
        //The isx64 variable is < 0 then the filename must contain a full path otherwise an error is thrown
        //The filename does not need to contain .dll and may be a complete path
        //and filename or may be just a filename. The list of directories will be searched

        public void Open(string filename, int isx32)
        {
            //If the extensions does not exist then add it
            if (String.IsNullOrWhiteSpace(Path.GetExtension(filename)) == true)
                filename += ".dll";

            //Save the original filename
            string originalfilename = filename;
           
            //Set the text of the tree node to the name of the file
            this.Text = Path.GetFileName(filename);

            //If the file does not exist then search for the file in the dll search paths
            if(PathFileExistsW(filename) == false)
            {
                if(isx32 > 0) //Case for when the file to be opened is a x32 bit dll
                {
                    //Search the x32 dll paths for the file
                    foreach (string path in AnalyzeForm.DllSearchPathsx32)
                    {
                        filename = path + originalfilename;
                        if (PathFileExistsW(filename) == false)
                            continue;
                        goto openfile;
                    }
                }
                else if(isx32 == 0) //Case for when the file to be opened is a x64 bit dll
                {
                    //Search the x64 dll paths for the file
                    foreach (string path in AnalyzeForm.DllSearchPathsx64)
                    {
                        filename = path + originalfilename;
                        if (PathFileExistsW(filename) == false)
                            continue;
                        goto openfile;
                    }
                }
                else //Case for when the file to be opened architecture cannot be determined, an error will be thrown
                {
                    //Throw an error
                    throw new ProgramError(ProgramSection.ProgramPatch, 0,
                        "The Full Filename Must be Supplied When The File Type Architecture (x32 or x64) Cannot Be Determined");
                }

                //Since the file was not found indicate this through the color and tool tip text
                this.ForeColor = Color.DarkGoldenrod;
                this.ToolTipText = "The File was not Found";
                return;  
             }
                
            
        openfile:

            //Create the underlying windows PE object that holds the information about the portable executable
            this.NodePe = new WindowsPeNet(filename,true,true);

            //If the file that was opened does not exist in the copied dll directory then copy the file to it
            if(this.NodePe.PE32 == true)
            {
                if (File.Exists(originalfilename) == false && File.Exists(@"C:\Temp\Dlls\x32\" + originalfilename) == false)
                    File.Copy(filename, @"C:\Temp\Dlls\x32\" + originalfilename);
            }
            else
            {
                if (File.Exists(originalfilename) == false && File.Exists(@"C:\Temp\Dlls\x64\" + originalfilename) == false)
                    File.Copy(filename, @"C:\Temp\Dlls\x64\" + originalfilename);
            }
            

            //Set the fore color of the tree node to green indicating that the file was opened
            this.ForeColor = Color.Green;

            //If the file icon is not present then return
            if (this.NodePe.FileIcon == null || this.NodePe.FileIcon.Size == Size.Empty)
                return;

            //If the file icon is present then add it to the image list 
            //and set the image index of this tree node to that list
            lock (AnalyzeForm._imageList)
            {
                AnalyzeForm._imageList.Images.Add(this.NodePe.FileIcon);
                this.ImageIndex = AnalyzeForm._imageList.Images.Count - 1;
                this.SelectedImageIndex = AnalyzeForm._imageList.Images.Count - 1;
            }
        }
        
        //Finds all the dependencies of the current tree node
        //If a dependency is found that has already been added then that dependency and not reopened and added to
        //the list of loaded pe files
        public void FindAllDependencies(List<WindowsPeTreeNode> loadedpefiles,ImageList imagelist)
        {
            //Clear all the nodes
            this.Nodes.Clear();

            //Should never be null but make sure
            if (this.NodePe == null)
                return;

            //Get the import library of the portable executable
           
            WindowsPeTreeNode newnode = null;

            //If imports exist then analyze them
            if (this.NodePe.Imports != null)
            {
                //Look through each import 
                for (int i = 0; i < this.NodePe.Imports.Count; i++)
                {
                    //Get the dll description of the import
                    PeImportDllNet impdll = this.NodePe.Imports[i];

                    //Lock the loaded pe files (ensures synchronization between threads)
                    lock (loadedpefiles)
                    {
                        //Analyze each already loaded dependency as a parallel operation
                        var queryA = from node in loadedpefiles.AsParallel() 
                                     where String.Compare(impdll.Name, node.Text, true) == 0 //Compare the name of loaded dll to the node text
                                     select node;
#if DEBUG
                        //Debug condition to make sure that only one loaded pe file exists
                        if (queryA != null && queryA.Count<object>() > 1)
                            throw new ProgramError(ProgramSection.ProgramPatch, 0, "More Than One of The Same Dependencies Were Loaded");
#endif
                        //If the query was successful and an loaded pe file was found
                        if(queryA != null && queryA.Count<object>() > 0)
                        {
                            //Get the loaded pe file that was found
                            WindowsPeTreeNode loadedpe = queryA.ElementAt<WindowsPeTreeNode>(0);

                            //Add a new node to this tree node that represents the dependency.
                            //Note that this node will not have a loaded portable executable
                            this.Nodes.Add(loadedpe.Text);

                            //Set the toop tip of the new tree node to the same value of the found loaded pe file
                            this.Nodes[this.Nodes.Count - 1].ToolTipText = loadedpe.ToolTipText;

                            //Set the forecolor of the new tree node to the same value of the found loaded pe file
                            this.Nodes[this.Nodes.Count - 1].ForeColor = loadedpe.ForeColor;

                            //Set the image index of the new tree node to the same value of the found loaded pe file
                            this.Nodes[this.Nodes.Count - 1].ImageIndex = loadedpe.ImageIndex;

                            //Don't load the pe file for the dependency since it was already been loaded
                            goto skip_load;
                        }

                        //Since the dependency was not already loaded create an instance of it
                        newnode = new WindowsPeTreeNode();

                        //Add the dependency to the list of loaded pe files
                        loadedpefiles.Add(newnode);
                    }

                    //Add the dependency to this node's child nodes
                    this.Nodes.Add(newnode);
                    try
                    {
                        //Open the dependency
                        newnode.Open(impdll.Name, (this.NodePe.PE32 == true) ? 1 : 0);
                    }
                    catch (ProgramError e)
                    {
                        //Since the dependency was not found then set the color to indicate an error and set
                        //tool tip value to indicate the error message
                        newnode.ForeColor = Color.Red;
                        newnode.ToolTipText = e.Message + "\n@" + e.FunctionName + "\nLine: " + e.LineNumber.ToString() + "\nIn File " + e.FileName;
                    }
                    catch
                    {
                        newnode.ForeColor = Color.Red;
                        newnode.ToolTipText = "An Unknown Error Has Occurred";
                    }

                    //Find all dependencies of the child node
                    newnode.FindAllDependencies(loadedpefiles, imagelist);
                    skip_load:
                    continue;
                }
            }

            //Analyze the export library
            if (this.NodePe.Exports == null)
                return;
            for (int i = 0; i < this.NodePe.Exports.Count; i++)
            {
                PeExportNet export = this.NodePe.Exports[i];
                String fordllname = export.ForwardedDllName;
                if (String.IsNullOrWhiteSpace(fordllname) == true)
                    continue;
                lock (loadedpefiles)
                {
                    
                    var queryA = from node in loadedpefiles.AsParallel()
                                 where String.Compare(fordllname + ".dll", node.Text, true) == 0 
                                 select node;
                    foreach (WindowsPeTreeNode node in queryA)
                    {
                        foreach(var childnode in this.Nodes)
                        {
                            TreeNode treechildnode = childnode as TreeNode;
                            if (treechildnode == null)
                                continue;
                            if (String.Compare(fordllname + ".dll", node.Text, true) == 0)
                                goto skipload_forwarded;
                        }
                        this.Nodes.Add(node.Text);
                        this.Nodes[this.Nodes.Count - 1].ToolTipText = node.ToolTipText;
                        this.Nodes[this.Nodes.Count - 1].ForeColor = node.ForeColor;
                        this.Nodes[this.Nodes.Count - 1].ImageIndex = node.ImageIndex;
                        goto skipload_forwarded;
                    }
                    newnode = new WindowsPeTreeNode();
                    loadedpefiles.Add(newnode);
                }
                this.Nodes.Add(newnode);
                try
                {
                    newnode.Open(fordllname, (this.NodePe.PE32 == true) ? 1 : 0);
                }
                catch (ProgramError e)
                {
                    this.ToolTipText = e.Message + "\n@" + e.FunctionName + "\nLine: " + e.LineNumber.ToString() + "\nIn File " + e.FileName;
                }
                catch
                {
                    this.ToolTipText = "An Unknown Error Has Occurred";
                }
                newnode.FindAllDependencies(loadedpefiles,  imagelist);
                skipload_forwarded:
                continue;
            }
        }

        public void FindOnlyTopLevelDependencies()
        {
            this.ContextMenuStrip = null;
            //Clear all the nodes
            this.Nodes.Clear();

            //Should never be null but make sure
            if (this.NodePe == null)
                return;

            //Get the import library of the portable executable
           
            WindowsPeTreeNode newnode = null;

            //If imports exist then analyze them
            if (this.NodePe.Imports != null)
            {
                //Look through each import 
                for (int i = 0; i < this.NodePe.Imports.Count; i++)
                {
                    //Get the dll description of the import
                    PeImportDllNet impdll = this.NodePe.Imports[i];

                    //Lock the loaded pe files (ensures synchronization between threads)
                    lock (AnalyzeForm._loadedPE)
                    {
                        //Analyze each already loaded dependency as a parallel operation
                        var queryA = from node in AnalyzeForm._loadedPE.AsParallel()
                                     where String.Compare(impdll.Name, node.Text, true) == 0 //Compare the name of loaded dll to the node text
                                     select node;
#if DEBUG
                        //Debug condition to make sure that only one loaded pe file exists
                        if (queryA != null && queryA.Count<object>() > 1)
                            throw new ProgramError(ProgramSection.ProgramPatch, 0, "More Than One of The Same Dependencies Were Loaded");
#endif
                        //If the query was successful and a loaded pe file was found
                        if (queryA != null && queryA.Count<object>() > 0)
                        {
                            //Get the loaded pe file that was found
                            WindowsPeTreeNode loadedpe = queryA.ElementAt<WindowsPeTreeNode>(0);

                            //Add a new node to this tree node that represents the dependency.
                            //Note that this node will not have a loaded portable executable
                            this.Nodes.Add(loadedpe.Text);

                            //Set the toop tip of the new tree node to the same value of the found loaded pe file
                            this.Nodes[this.Nodes.Count - 1].ToolTipText = loadedpe.ToolTipText;

                            //Set the forecolor of the new tree node to the same value of the found loaded pe file
                            this.Nodes[this.Nodes.Count - 1].ForeColor = loadedpe.ForeColor;

                            //Set the image index of the new tree node to the same value of the found loaded pe file
                            this.Nodes[this.Nodes.Count - 1].ImageIndex = loadedpe.ImageIndex;

                            //Set the image index when selected
                            this.Nodes[this.Nodes.Count - 1].SelectedImageIndex = loadedpe.ImageIndex; 

                            //Don't load the pe file for the dependency since it was already been loaded
                            goto skip_load;
                        }

                        //Since the dependency was not already loaded create an instance of it
                        newnode = new WindowsPeTreeNode();

                        //Add the dependency to the list of loaded pe files
                        AnalyzeForm._loadedPE.Add(newnode);
                    }

                    //Add the dependency to this node's child nodes
                    this.Nodes.Add(newnode);
                    try
                    {
                        //Open the dependency
                        newnode.Open(impdll.Name, (this.NodePe.PE32 == true) ? 1 : 0);
                    }
                    catch (ProgramError e)
                    {
                        //Since the dependency was not found then set the color to indicate an error and set
                        //tool tip value to indicate the error message
                        newnode.ForeColor = Color.Red;
                        newnode.ToolTipText = e.Message + "\n@" + e.FunctionName + "\nLine: " + e.LineNumber.ToString() + "\nIn File " + e.FileName;
                    }
                    catch
                    {
                        newnode.ForeColor = Color.Red;
                        newnode.ToolTipText = "An Unknown Error Has Occurred";
                    }

                    //Create a context menu strip to load the dependencies
                    System.Windows.Forms.ContextMenuStrip menustrip = new ContextMenuStrip();
                    menustrip.ShowImageMargin = false;
                    menustrip.ShowCheckMargin = false;
                    menustrip.RenderMode = ToolStripRenderMode.System;

                    //Create a context menu button to load dependencies when clicked
                    System.Windows.Forms.ToolStripButton loaddependbutton = new ToolStripButton("Load Dependencies");
                    loaddependbutton.TextAlign = ContentAlignment.MiddleCenter;
                    loaddependbutton.Click += newnode.LoadDependenciesEvent;

                    //Add the load dependency button to the create context menu
                    menustrip.Items.Add(loaddependbutton);
                   
                    //Set the context menu of the current node to the create context menu
                    newnode.ContextMenuStrip = menustrip;

                    //Set the menu strip to auto size
                    menustrip.AutoSize = false;
                    menustrip.AutoSize = true;

                skip_load:
                    continue;
                }
            }

            //Analyze the export library
           
            if (this.NodePe.Exports == null)
                return;
            for (int i = 0; i < this.NodePe.Exports.Count; i++)
            {
                string fordllname = this.NodePe.Exports[i].ForwardedDllName;
                if (String.IsNullOrWhiteSpace(fordllname) == true)
                    continue;

                string dllname = fordllname + ".dll";

                //Analyze each already loaded dependency as a parallel operation
                var queryA = from node in AnalyzeForm._loadedPE.AsParallel()
                             where String.Compare(dllname, node.Text, true) == 0 //Compare the name of loaded dll to the node text
                             select node;
#if DEBUG
                //Debug condition to make sure that only one loaded pe file exists
                if (queryA != null && queryA.Count<object>() > 1)
                    throw new ProgramError(ProgramSection.ProgramPatch, 0, "More Than One of The Same Dependencies Were Loaded");
#endif
                //If the query was successful and a loaded pe file was found
                if (queryA != null && queryA.Count<object>() > 0)
                {
                    //Get the loaded pe file that was found
                    WindowsPeTreeNode loadedpe = queryA.ElementAt<WindowsPeTreeNode>(0);

                    //Add a new node to this tree node that represents the dependency.
                    //Note that this node will not have a loaded portable executable
                    this.Nodes.Add(loadedpe.Text);

                    //Set the toop tip of the new tree node to the same value of the found loaded pe file
                    this.Nodes[this.Nodes.Count - 1].ToolTipText = loadedpe.ToolTipText;

                    //Set the forecolor of the new tree node to the same value of the found loaded pe file
                    this.Nodes[this.Nodes.Count - 1].ForeColor = loadedpe.ForeColor;

                    //Set the image index of the new tree node to the same value of the found loaded pe file
                    this.Nodes[this.Nodes.Count - 1].ImageIndex = loadedpe.ImageIndex;

                    //Set the image index when selected
                    this.Nodes[this.Nodes.Count - 1].SelectedImageIndex = loadedpe.ImageIndex;

                    //Don't load the pe file for the dependency since it was already been loaded
                    goto skipload_forwarded;
                }

                //Since the dependency was not already loaded create an instance of it
                newnode = new WindowsPeTreeNode();

                //Add the dependency to the list of loaded pe files
                AnalyzeForm._loadedPE.Add(newnode);
                this.Nodes.Add(newnode);
                try
                {
                    newnode.Open(dllname, (this.NodePe.PE32 == true) ? 1 : 0);
                }
                catch (ProgramError e)
                {
                    this.ToolTipText = e.Message + "\n@" + e.FunctionName + "\nLine: " + e.LineNumber.ToString() + "\nIn File " + e.FileName;
                }
                catch
                {
                    this.ToolTipText = "An Unknown Error Has Occurred";
                }

                //Create a context menu strip to load the dependencies
                System.Windows.Forms.ContextMenuStrip menustrip = new ContextMenuStrip();
                menustrip.AutoSize = true;
                menustrip.ShowImageMargin = false;
                menustrip.ShowCheckMargin = false;
                menustrip.RenderMode = ToolStripRenderMode.System;

                //Create a context menu button to load dependencies when clicked
                System.Windows.Forms.ToolStripButton loaddependbutton = new ToolStripButton("Load Dependencies");
                loaddependbutton.TextAlign = ContentAlignment.MiddleCenter;
                loaddependbutton.Click += newnode.LoadDependenciesEvent;

                //Add the load dependency button to the create context menu
                menustrip.Items.Add(loaddependbutton);

                //Set the context menu of the current node to the create context menu
                newnode.ContextMenuStrip = menustrip;
            skipload_forwarded:
                continue;
            }
           
            

        }
        
    }
}
