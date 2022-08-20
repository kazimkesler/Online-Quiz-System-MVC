using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Models.ViewModels;
using OnlineQuizSystem.Services;

namespace OnlineQuizSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionManager sessionManager;
        private readonly OnlineQuizSystemContext context;
        private readonly Random rnd = new();
        public HomeController(OnlineQuizSystemContext context, SessionManager sessionManager)
        {
            this.context = context;
            this.sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            var toSkip = rnd.Next(context.Questions.Count() - 4);
            var Questions = context.Questions.Skip(toSkip).Take(4).ToList();
            return View(Questions);
        }

        public IActionResult QuizInfo()
        {
            return View();
        }

        public IActionResult Confirm()
        {
            sessionManager.Clear();
            sessionManager.State = false;
            sessionManager.Category = 1;
            var UserAnswerVM = new List<UserAnswerVM>();
            for (int i = 1; i <= 10; i++)
            {
                var questionsInCategory = context.Questions.Where(x => x.Category == i);
                int toSkip = rnd.Next(questionsInCategory.Count());
                var question = questionsInCategory.Skip(toSkip).Take(1).Single();
                UserAnswerVM.Add(new UserAnswerVM
                {
                    QuestionId = question.QuestionId,
                    Path = question.Path,
                    Category = question.Category,
                    CorrectAnswer = question.Answer
                });
            }
            //UserAnswerVM.Shuffle();
            sessionManager.UserAnswers = UserAnswerVM;
            return RedirectToAction("Index", "Quiz");
        }
    }
}