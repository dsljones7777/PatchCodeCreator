using System;
using WindowsPECLR;
using StandardLibrary_CSharp;
namespace PatchCodeCreator
{
   
    public class FuncDefFinderResults : Exception
    {
        public readonly ExportDefinitionStatus Status;
        public readonly string FunctionDefinition;
        public FuncDefFinderResults(string functiondefinition)
        {
            this.FunctionDefinition = functiondefinition;
            this.Status = ExportDefinitionStatus.Ok;
        }
        public FuncDefFinderResults(ExportDefinitionStatus status,string message) : base(message)
        {
            this.Status = status;
            
        }
        
    }
    public interface iFuncDefFinder : IDisposable
    {
        FuncDefFinderResults Find(CancelAsyncToken token, string functionname, string dllname);
    }
   

}
