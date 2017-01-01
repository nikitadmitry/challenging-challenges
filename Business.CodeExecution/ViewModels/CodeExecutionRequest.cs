using System.Collections.Generic;

namespace Business.CodeExecution.ViewModels
{
    public class CodeExecutionRequest
    {
        public string SourceCode
        {
            get;
            set;
        }

        public CodeLanguage CodeLanguage
        {
            get;
            set;
        }

        public IEnumerable<string> Input
        {
            get;
            set;
        }
    }
}