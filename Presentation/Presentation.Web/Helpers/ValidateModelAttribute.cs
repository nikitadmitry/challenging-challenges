using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Web.Helpers
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ErrorObjectResult(context.ModelState);
            }
        }
    }
}