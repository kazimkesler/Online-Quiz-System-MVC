using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Filters;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Services;

namespace OnlineQuizSystem.Controllers
{
    [Completed]
    public class ResultController : Controller
    {
        private readonly OnlineQuizSystemContext context;
        private readonly SessionManager sessionManager;
        public ResultController(OnlineQuizSystemContext context, SessionManager sessionManager)
        {
            this.context = context;
            this.sessionManager = sessionManager;
        }
        public IActionResult Index() 
        {
            var userAnswers = sessionManager.UserAnswers;
            int right = userAnswers.Sum(x => x.CorrectAnswer == x.Answer ? 1 : 0);
            int wrong = userAnswers.Sum(x => x.Answer is not null && x.CorrectAnswer != x.Answer ? 1 : 0);
            int blank = userAnswers.Sum(x => x.Answer is null ? 1 : 0);
            double duration = userAnswers.Sum(x => x.Duration);
            int dif = right - wrong;
            if (dif < 0) dif = 0;
            double maxScore = 10.0 * 10.0 / 100000.0;
            double point = Math.Round(dif * dif / duration / maxScore * 100, 2);
            if (point < 0) point = 0;
            bool success = point > context.Groups.First().RequiredScore;
            sessionManager.Success = success;
            ViewBag.Right = right;
            ViewBag.Wrong = wrong;
            ViewBag.Blank = blank;
            ViewBag.Duration = duration;
            ViewBag.Point = point;
            ViewBag.Success = success;
            return View(userAnswers);
        }
    }
}
