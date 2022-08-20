using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Filters;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Services;

namespace OnlineQuizSystem.Controllers
{
    [State]
    public class QuizController : Controller
    {
        private readonly Random rnd = new();
        private readonly OnlineQuizSystemContext context;
        private readonly SessionManager sessionManager;
        public QuizController(OnlineQuizSystemContext context, SessionManager sessionManager)
        {
            this.context = context;
            this.sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetQuestion()
        {
            sessionManager.Start = DateTime.Now;
            var x = sessionManager.CurrentUserAnswer;
            return Json(new
            {
                answer = x.Answer,
                category = x.Category,
                duration = x.Duration,
                path = x.Path,
                totalElapsedTime = x.TotalElapsedTime
            });
        }
        public IActionResult Next()
        {
            var userAnswer = sessionManager.CurrentUserAnswer;
            userAnswer.TotalElapsedTime += (int)(DateTime.Now - sessionManager.Start).TotalMilliseconds;
            sessionManager.UpdateAnswer(userAnswer);
            if (sessionManager.Category == 10)
            {
                if (sessionManager.UserAnswers.Where(x => x.Answer is not null).Count() < 5)
                    return Json("notEnough");
                else
                {
                    sessionManager.State = true;
                    return Json("finished");
                }
            }
            sessionManager.Category++;
            return RedirectToAction(nameof(GetQuestion));
        }
        public IActionResult Previous()
        {
            if (sessionManager.Category > 1)
            {
                var userAnswer = sessionManager.CurrentUserAnswer;
                userAnswer.TotalElapsedTime += (int)(DateTime.Now - sessionManager.Start).TotalMilliseconds;
                sessionManager.UpdateAnswer(userAnswer);
                sessionManager.Category--;
            }
            return RedirectToAction(nameof(GetQuestion));
        }

        [Route("[controller]/[action]/{id:range(0, 4)?}")]
        public IActionResult Answer(int? id)
        {
            var userAnswer = sessionManager.CurrentUserAnswer;
            userAnswer.TotalElapsedTime += (int)(DateTime.Now - sessionManager.Start).TotalMilliseconds;
            sessionManager.UpdateAnswer(userAnswer);
            if (userAnswer.TotalElapsedTime >= 10000)
            {
                userAnswer.Answer = id;
                if (id is not null)
                    userAnswer.Duration = userAnswer.TotalElapsedTime;
                sessionManager.UpdateAnswer(userAnswer);
            }
            return RedirectToAction(nameof(GetQuestion));
        }
    }
}
