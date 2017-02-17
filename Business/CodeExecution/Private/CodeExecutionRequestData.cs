using System.Collections.Generic;
using System.Net;

namespace Business.CodeExecution.Private
{
    internal class CodeExecutionRequestData
    {
        public IDictionary<string, string> ProtectionFieldValues
        {
            get;
            set;
        }

        public CookieCollection CookieCollection
        {
            get;
            set;
        }
    }
}