using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineQuizSystem.Models;

namespace OnlineQuizSystem.Filters
{
    public class GlobalExceptionHandler : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                try
                {
                    Console.WriteLine("\n----------------------------------\n" + context.Exception.Message + " has been thrown. Details:\n----------------------------------\n" + context.Exception.ToString() + "\n----------------------------------\n");
                    OnlineQuizSystemContext dbContext = context.HttpContext.RequestServices.GetService<OnlineQuizSystemContext>()!;
                    dbContext.GlobalExceptionLogs.Add(new GlobalExceptionLog
                    {
                        Error = context.Exception.Message,
                        Detail = context.Exception.ToString()
                    });
                    dbContext.SaveChanges();
                    context.Result = new RedirectToActionResult("SendMessage", "Contact", new {error=1}); //1 değeri ile kullanıcı bir hatayla karşılaştığı konusunda bilgilendirilecek
                    context.ExceptionHandled = true;
                }

                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("UNHANDLED ERROR");
                    Console.ResetColor();
                }

            }
        }
    }
}
