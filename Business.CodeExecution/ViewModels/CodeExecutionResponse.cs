using System.Collections.Generic;

namespace Business.CodeExecution.ViewModels
{
    public class CodeExecutionResponse
    {
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(ErrorMessage);
            }
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public IEnumerable<string> Output
        {
            get;
            set;
        }
    }
}