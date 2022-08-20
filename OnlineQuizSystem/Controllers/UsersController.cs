using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Filters;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Services;

namespace OnlineQuizSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly OnlineQuizSystemContext context;
        private readonly SessionManager sessionManager;
        public UsersController(OnlineQuizSystemContext context, SessionManager sessionManager)
        {
            this.context = context;
            this.sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View(context.UserScores.ToList()); //Bu entity bir tabloya bağlı değildir, bir stored procedure'a bağlıdır.
        }

        [Completed]
        [Success]
        public IActionResult Register()
        {
            return View();
        }

        [Completed]
        [Success]
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, responseText = "failed" });
            else if (context.Users.Find(user.Phone) is not null)
                return Json(new { status = false, responseText = "already_registered" });
            context.Users.Add(user);
            if (sessionManager.UserAnswers is not null)
                context.UserQuestions.AddRange(sessionManager.UserAnswers.Where(x => x.Answer != null).Select(x => new UserQuestion()
                {
                    Phone = user.Phone,
                    Question = x.QuestionId,
                    Answer = (int)x.Answer!,
                    Duration = x.Duration
                }));
            context.SaveChanges();
            sessionManager.Clear();
            return Json(new { status = true, responseText = "success" });
        }
    }
}
