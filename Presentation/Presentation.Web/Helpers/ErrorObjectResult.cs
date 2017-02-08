using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Framework.Utilities;

namespace Presentation.Web.Helpers
{
    public class ErrorObjectResult : BadRequestObjectResult
    {
        public ErrorObjectResult(IEnumerable<string> errors) : base(errors)
        {
        }

        public ErrorObjectResult(string error) : base(error.YieldEnumerable())
        {
        }

        public ErrorObjectResult(ModelStateDictionary modelState) : base(modelState.Values.SelectMany(m => m.Errors)
            .Select(e => e.ErrorMessage))
        {
            
        }
    }
}