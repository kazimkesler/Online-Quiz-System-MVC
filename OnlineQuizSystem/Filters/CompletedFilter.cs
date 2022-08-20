using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineQuizSystem.Services;

namespace OnlineQuizSystem.Filters
{
    public class CompletedAttribute : ActionFilterAttribute
    {
        private SessionManager sessionManager = null!;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            sessionManager = new SessionManager(context.HttpContext.Session);
            if (sessionManager.State is not null && sessionManager.State == false) 
                context.Result = new RedirectToActionResult("Index", "Quiz", null);
        }
    }
}
