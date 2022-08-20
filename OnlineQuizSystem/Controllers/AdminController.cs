using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Models.ViewModels;
using OnlineQuizSystem.Services;
using System.Security.Claims;

namespace OnlineQuizSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly OnlineQuizSystemContext context;
        private readonly SessionManager sessionManager;

        public AdminController(OnlineQuizSystemContext context, SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
            this.context = context;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            ViewBag.MessageCount = context.Messages.Where(x => !x.Done).Count();
            var group = context.Groups.Find(1);
            return View(group);
        }
        public IActionResult Login()
        {
            if(HttpContext.User.FindFirst(ClaimTypes.Name) is not null)
                return RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var group = context.Groups.Find(1);
                if (group is not null && model.User == "admin" && group.Pass == model.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.User),
                        new Claim(ClaimTypes.Role, "Administrator")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = true };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return Json(true); 
                }
            }
            return Json(false);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Register()
        {
            sessionManager.State = true;
            sessionManager.Success = true;
            return RedirectToAction("Register", "Users");
        }     
        
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Update(Group model)
        {
            var group = context.Groups.Find(1);
            if (group is not null)
            {
                group.Link = model.Link;
                group.State = model.State;
                group.RequiredScore = model.RequiredScore;
                context.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
    }
}

