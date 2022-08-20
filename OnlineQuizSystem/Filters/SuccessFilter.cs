using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineQuizSystem.Services;

namespace OnlineQuizSystem.Filters
{
    public class SuccessAttribute : ActionFilterAttribute
    {
        private SessionManager sessionManager;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            sessionManager = new SessionManager(context.HttpContext.Session);
            if (sessionManager.Success is null || sessionManager.Success == false) 
                context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
