using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineQuizSystem.Services;

namespace OnlineQuizSystem.Filters
{
    public class StateAttribute: ActionFilterAttribute
    {
        private SessionManager sessionManager = null!;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            sessionManager = new SessionManager(context.HttpContext.Session);
            if (sessionManager.State is null || sessionManager.State == true)
                context.Result = new RedirectToActionResult("Index", "Home", null);

        }
    }
}
