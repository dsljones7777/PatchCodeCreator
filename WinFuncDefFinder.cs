using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PatchCodeCreator;
using StandardLibrary_CSharp;
using WindowsPECLR;
using System.Collections.Generic;

namespace PatchCodeCreator
{
    /*
     * This class defines a way to find a specific value in an HTML document
     */
    internal class HtmlQueryInfo
    {
        //The id to search for
        public string ID;

        //The node name under the id
        public string NodeName;

        //The attribute name
        public string AttributeName;

        //The attribute value
        public string AttributeValue;

        
        public bool Multiple;
        /*
         * The format of the infostr is as follows:
             * Id="value"
             * NodeName="value"
             * AttribName="value"
             * AttribVal="value"
             * Multiple="Yes/No"
        */
        //Initializes the query info according to the format string
        public HtmlQueryInfo(string infostr)
        {
            //Get the ID value if it exists
            int index = infostr.IndexOf("Id=");
            this.ID = ReadValue(infostr, index);

            //Get the NodeName value if it exists
            index = infostr.IndexOf("NodeName=");
            this.NodeName = ReadValue(infostr, index);

            //Get the AttributeName if it exists
            index = infostr.IndexOf("AttribName=");
            this.AttributeName = ReadValue(infostr, index);

            //Get the AttributeValue if it exists
            index = infostr.IndexOf("AttribVal=");
            this.AttributeValue = ReadValue(infostr, index);

            //Get whether multiple is true or false
            index = infostr.IndexOf("Multiple=");
            string value = ReadValue(infostr, index);

            //If multiple was not defined then it is by default false 
            if (value == null)
                this.Multiple = false;

            //Determine whether multiple is true or false
            else if (String.Compare(value, "Yes") == 0)
                this.Multiple = true;
            else if (String.Compare(value, "No") == 0)
                this.Multiple = false;
            else
                throw new Exception("Invalid Query Value: " + value);
        }

        //Reads a value from the string at the specified index
        //The value must be contained in ""
        //If the index is -1 then a null string is returned
        //If the string is malformed (a "" does not exists) then an exception is thrown
        string ReadValue(string str, int index)
        {
            //If the index is -1 indicating that the value specifier was not found then return null
            if (index == -1)
                return null;

            //Create a substring starting at the specified index
            string substr = str.Substring(index);

            //Create a substring starting at the first "
            index = substr.IndexOf('\"');

            //If the quote does not exist then return an error
            if (index == -1)
                throw new Exception("Malformed Query Value");

            //Find the closing quote
            int endindex = substr.IndexOf('\"', index + 1);

            //If the closing quote was not found throw an error
            if(endindex == -1)
                throw new Exception("Malformed Query Value");

            //Create a string representing the read value
            substr = substr.Substring(index + 1, endindex - index - 1);

            //If the string created that represents the value is null or only whitespace return null
            if (String.IsNullOrWhiteSpace(substr) == true)
                return null;

            //Return the value read as a string
            return substr;
        }
    }
    public class WinFuncDefFinder : iFuncDefFinder
    {
        private static List<String> ValidDllNames;
        //Static initializer that creates html query info for html searches and msdn pages
        static WinFuncDefFinder()
        {
            WinFuncDefFinder.CreateSearchQueryInfo();
            WinFuncDefFinder.CreateFuncNameQueryInfo();
            ValidDllNames = new List<string>();
            ValidDllNames.AddRange(new string[] {
                "kernel32.dll",
                "ntdll.dll",
                "kernelbase.dll",
                "advapi32.dll",
                "gdi32.dll",
                "user32.dll",
                "comctl32.dll",
                "comdlg32.dll",
                "shell32.dll",
                "netapi32.dll",
                "mshtml.dll",
                "shdocvw.dll" });
            ValidDllNames.Sort();
        }

        //Constructor only to be called with StartQuery or within this class
        private WinFuncDefFinder()
        {
            this._httpCtl = new HttpClient();
            this._currentDoc = new HtmlAgilityPack.HtmlDocument();
        }
       
        //Performs a bing search and searches for an msdn page. Then the msdn page is decoded to 
        //find the function definition
        public FuncDefFinderResults Find(CancelAsyncToken token, string functionname,string dllname)
        {
            try
            {
                //Get the bing search for the function name
                GetSearchHtmlDoc(functionname,token);

                //Get the function msdn page link
                string functionpglink = ReadValueAccordToQueryInfo(WinFuncDefFinder._searchQueryInfo);
                if (functionpglink == null)
                    return new FuncDefFinderResults(ExportDefinitionStatus.Failed,"Failed to Retrieve the Search Engine Page");

                //Create a uri representing the msdn page link decoded by the html document
                Uri host = new Uri(functionpglink);

                //Verify the host of the page link points to msdn
                if (string.Compare(host.Host, "msdn.microsoft.com", true) != 0)
                    return new FuncDefFinderResults(ExportDefinitionStatus.NotFound,"Function Definition Was Not Found");

                //Retrieve the html document from the sprecified link
                GetHtmlDoc(functionpglink,token);

                //Verify the page matches the requested dynamic link library
                if (VerifyPage(dllname) == false)
                    return new FuncDefFinderResults(ExportDefinitionStatus.NotFound,"Function Definition Was Not Found");

                //Decode the function  definition from the html document (msdn page) 
                string functiondef = ReadValueAccordToQueryInfo(WinFuncDefFinder._functionDefinitionQueryInfo);

                //Update the function definition
                if (functiondef == null)
                    return new FuncDefFinderResults(ExportDefinitionStatus.NotFound,"Function Definition Was Not Found");
                return new FuncDefFinderResults(functiondef);
            }
            catch(FuncDefFinderResults e)
            {
                return e;
            }
            catch
            {
                return new FuncDefFinderResults(ExportDefinitionStatus.UnexpectedError,"An Unidentified Error Occurred");
            }
        }

        //Starts a thread that searches for the specified export function definitions
        public static CancelableTask StartQueryTask(EventHandler completionhandler, string dllname, List<PeExportNet> lib)
        {
            return new CancelableTask2<string, List<PeExportNet>>(completionhandler, StartQuery, dllname, lib);
        }

        //Thread routine to find all export function definitions
        private static void StartQuery(ref CancelAsyncToken token, string dllname, List<PeExportNet> exportlib)
        {
            //Create a windows function definition finder (will be disposed of)
            using (WinFuncDefFinder retreiver = new WinFuncDefFinder())
            {
                //Go through each import and find the function defition
                foreach (PeExportNet exportfunction in exportlib)
                {
                    //Don't search if the export function name is null
                    if (String.IsNullOrWhiteSpace(exportfunction.Name) == true)
                    {
                        exportfunction.DefinitionStatus = ExportDefinitionStatus.NotFound;
                        exportfunction.DefinitionErrorMessage = "Cannot Find Function By Ordinal";
                        try
                        {
                            exportfunction.NotifyOfChange();
                        }
                        catch (Exception e)
                        {
                            token.TerminationError = e;
                            return;
                        }
                        continue;
                    }

                    //Don't search if the export function has already been searched
                    if (exportfunction.DefinitionStatus != ExportDefinitionStatus.Pending)
                    {
                        if (exportfunction.DefinitionStatus != ExportDefinitionStatus.Timeout)
                            continue;
                    }

                    //Check to see if finding the function definitions should be canceled
                    if (token.ShouldICancel() == true)
                    {
                        token.TerminationError = new TaskCanceledException("Finding Function Definitions Was Canceled");
                        return;
                    }

                    FuncDefFinderResults results = null;
                    try
                    {
                        //Find the function definition
                        results = retreiver.Find(token, exportfunction.Name, dllname);
                    }
                    catch(TaskCanceledException e)
                    {
                        token.TerminationError = e;
                        return;
                    }

                    //Check to see if finding the function definitions should be canceled
                    if (token.ShouldICancel() == true)
                    {
                        token.TerminationError = new TaskCanceledException("Finding Function Definitions Was Canceled");
                        return;
                    }

                    //Update the function definition status
                    exportfunction.DefinitionStatus = results.Status;

                    //If finding the function definition succeeded then update the function definition 
                    if (results.Status == ExportDefinitionStatus.Ok)
                        exportfunction.Definition = results.FunctionDefinition;
                    //If an error message is present then update the function definition
                    else if (String.IsNullOrWhiteSpace(results.Message) == false)
                        exportfunction.DefinitionErrorMessage = results.Message;
                    
                    //Mark the export as having changed. If an error occurs then mark the error in the 
                    //Cancel async token and exit the function
                    try
                    {
                        exportfunction.NotifyOfChange();
                    }
                    catch (Exception e)
                    {
                        token.TerminationError = e;
                        return;
                    }
                }
            }
            return;
        }

        //Releases the resources used by the HttpClient
        void IDisposable.Dispose()
        {
            this._httpCtl.Dispose();
        }

        //reads a specified value in a html document.
        //The query info tells the code where to find the value to read
        private string ReadValueAccordToQueryInfo(HtmlQueryInfo[] info)
        {
            //The first element of a html query must contain an ID
            if (info[0].ID == null)
                throw new Exception("Id is missing in query");

            //Get the node that contains the specified ID
            HtmlNode node = this._currentDoc.GetElementbyId(info[0].ID);
            if (node == null)
                return null;

            //Read every html query item except the last one and navigate to where the requested value is
            for (int i = 1; i < info.Length - 1; i++)
            {
                //Every html query item must contain a node name
                if (info[i].NodeName == null)
                    throw new Exception("Nodename value is missing in query");

                //Find the node by it's name. If the function fails then return null indicating the operation failed
                node = FindNode(node, info[i].NodeName, info[i].AttributeName, info[i].AttributeValue);
                if (node == null)
                    return null;
            }

            //The last html query item contains an attribute name.
            HtmlQueryInfo finalnodeinfo = info[info.Length - 1];

            //If the attribute name is null the innertext of the current node is returned
            if (finalnodeinfo.AttributeName == null)
                return node.InnerText;
            else
                //If the attribute name is not null return the value of the attribute ( this should be the value 
                //that is requested)
                return this.GetAttributeValue(node, finalnodeinfo.AttributeName);
        }

        /*
         * Reads query info from an array of strings
         * The query info first item must be an id
         * The second to N-1 items are node names
         * The last query info structure is an attribute name or null for the body of the N-2 node
         *  
         */
        
        private static HtmlQueryInfo[] ReadQueryInfo(string[] strcollection)
        {

            //Read each info string from the array
            HtmlQueryInfo[] returnval = new HtmlQueryInfo[strcollection.Length];
            for (int i = 0; i < strcollection.Length; i++)
            {
                returnval[i] = new HtmlQueryInfo(strcollection[i]);
            }
            return returnval;
        }

        //TODO: Move to registry or to internal program settings
        //Creates html query strings to read the value of the first result of an
        //bing search html page 
        private static void CreateSearchQueryInfo()
        {
            string[] setupstrs = new string[]
            {
                //The first item is the ID that points to the returned search results
                "Id= \"b_results\"",
                "NodeName= \"li\"AttribName= \"class\" AttribVal= \"b_algo\" Multiple= \"Yes\"",
                "NodeName= \"h2\"",
                "NodeName= \"a\"",
                "AttribName= \"href\""
            };
            WinFuncDefFinder._searchQueryInfo = ReadQueryInfo(setupstrs);
        }
        private static void CreateFuncNameQueryInfo()
        {
            string[] setupstrs = new string[]
            {
                "Id= \"code-snippet-1\"",
                "NodeName= \"div\" AttribName= \"class\" AttribVal= \"codeSnippetContainerCodeContainer\"",
                "NodeName= \"div\" AttribName= \"class\" AttribVal= \"codeSnippetContainerCode\"",
                "NodeName= \"div\" AttribName= \"style\"",
                "NodeName= \"pre\""
            };
            WinFuncDefFinder._functionDefinitionQueryInfo = ReadQueryInfo(setupstrs);
        }

        //Verifies that current page (should be an msdn html) contains a valid definition for the specified dll 
        private bool VerifyPage(string dllname)
        {
            HtmlNode node = this._currentDoc.GetElementbyId("mainSection");
            foreach (HtmlNode innernode in node.ChildNodes)
            {
                if (innernode.Name != "table")
                    continue;
                foreach (HtmlNode inner2node in innernode.ChildNodes)
                {
                    if (inner2node.Name != "tr")
                        continue;
                    foreach (HtmlNode inner3node in inner2node.ChildNodes)
                    {
                        if (inner3node.Name != "td")
                            continue;
                        foreach (HtmlNode inner4node in inner3node.ChildNodes)
                        {
                            if (inner4node.Name != "dl")
                                continue;
                            foreach (HtmlNode inner5node in inner4node.ChildNodes)
                            {
                                if (inner5node.Name != "dt")
                                    continue;
                                string val = inner5node.InnerText.Trim();
                                if (String.Compare(val, dllname, true) == 0)
                                    return true;
                                if (IsInListOfValidWin32Dlls(val) == true)
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        
        bool IsInListOfValidWin32Dlls(String val)
        {
            if (WinFuncDefFinder.ValidDllNames.BinarySearch(val.ToLower()) >= 0)
                return true;
            return false;
        }
        //Retrieves the html document that is a Bing search for the function name
        private void GetSearchHtmlDoc(string functionname,CancelAsyncToken token)
        {
            //Create a search string
            string prepname = this.PrepNameForBingSearch(functionname);

            //Retrieve the HTML document from bing containing the search results
            GetHtmlDoc(prepname,token);
        }

        //Retrieves the specified document from the supplied URL
        private void GetHtmlDoc(string url,CancelAsyncToken token)
        {
            //Get the HTML response as a string
            var response = this._httpCtl.GetStringAsync(url);

            //Wait for the response to be returned from the requested url
            //Waits for a total of 15 seconds max
            for(int i = 0; i < 30; i ++)
            {
                //If the wait operation has been signaled (the response has completed) then read the response
                if (response.Wait(500) == true)
                    goto readresponse;

                //If the task should be canceled then throw a task cancelation exception to exit the thread
                if (token.ShouldICancel() == true)
                    throw new TaskCanceledException();
            }

            //If the 15 seconds has elasped then throw a timeout exception
            throw new TimeoutException();

            //Decode the response string into an html document that can easily be read
        readresponse:
            string contentstr = response.Result;
            try
            {
                string htmldecodedstr = WebUtility.HtmlDecode(contentstr);
                this._currentDoc.LoadHtml(htmldecodedstr);
            }
            catch
            {
                throw new FuncDefFinderResults(ExportDefinitionStatus.Failed, "Failed to Decode the HTML Document");
            }
        }
        private bool DoesNodeContainAttrib(HtmlNode node, string attribname, string attribval)
        {
            foreach (HtmlAttribute attrib in node.Attributes)
            {
                if (attrib.Name != attribname)
                    continue;
                if (attribval == null)
                    return true;
                if (attrib.Value != attribval)
                    continue;
                return true;
            }
            return false;
        }
        private HtmlNode FindNode(HtmlNode parentnode, string nodename, string attribname, string attribval)
        {
            foreach (HtmlNode node in parentnode.ChildNodes)
            {
                if (node.Name != nodename)
                    continue;
                if (attribname == null)
                    return node;
                if (DoesNodeContainAttrib(node, attribname, attribval) == true)
                    return node;
                continue;
            }
            return null;
        }
        private string GetAttributeValue(HtmlNode parentnode, string attribname)
        {
            foreach (HtmlAttribute attrib in parentnode.Attributes)
            {
                if (attrib.Name == attribname)
                    return attrib.Value;
            }
            return null;
        }

        //Creates a url that represents a bing search for the specified function
        //Specializes Unicode and ANSI defintions by removing the A or W postfix character from the search string
        private string PrepNameForBingSearch(string functionname)
        {
            string name;

            //If the function name contains A or W postfix remove it
            if (functionname[functionname.Length - 1] == 'A' || functionname[functionname.Length - 1] == 'W')
                name = functionname.Substring(0, functionname.Length - 1);
            else
                name = functionname;

            //Return the search string uri as a bing search to have the function name and msdn in the searcg
            return "https://www.bing.com/search?q=" + name + "+msdn";
        }

        //The query info required to decode a bing search to find the first search result
        private static HtmlQueryInfo[] _searchQueryInfo;

        //The query info requred to decode a msdn c++ function definition page to find the actual function definition
        private static HtmlQueryInfo[] _functionDefinitionQueryInfo;

        //The html client used to request data from the servers
        private HttpClient _httpCtl;

        //The html document that has recently been loaded
        private HtmlAgilityPack.HtmlDocument _currentDoc;
    }
}
