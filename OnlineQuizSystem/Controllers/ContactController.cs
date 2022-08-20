using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models;
using System.Data;

namespace OnlineQuizSystem.Controllers
{
    public class ContactController : Controller
    {
        private readonly OnlineQuizSystemContext context;
        public ContactController(OnlineQuizSystemContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Index() 
        {
            var messages = context.Messages.Where(x => !x.Done).OrderByDescending(x => x.SentDate).ToList();
            return View(messages);
        }

        [Route("[controller]/[action]/{error:range(0,2)?}")] //Id 1 ise bir hatayla karşılaştı. Bunu bildirmesi için kullanıcı bilgilendirilecek.
        [HttpGet]
        public IActionResult SendMessage(int error)
        {
            if(error == 1)
                ViewBag.Error = true;
            return View();
        }

        [HttpPost]//6
        public IActionResult SendMessage(Message model)
        {
            if (!ModelState.IsValid)
                return Json(false);
            context.Messages.Add(model);
            context.SaveChanges();
            return Json(true);
        }

        [Route("[controller]/[action]/{id:min(0)?}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Done(int id)
        {
            var message = context.Messages.FirstOrDefault(x => x.MessageId == id);
            if (message is not null)
            {
                message.Done = true;
                context.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
    }
}
