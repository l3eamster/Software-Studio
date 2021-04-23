using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using DonutzStudio.Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DonutzStudio.Data;

namespace DonutzStudio.Controllers
{
    public class LoginController : Controller
    {
        private readonly DonutzStudioContext _context;
        public LoginController(DonutzStudioContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Username", "");
            HttpContext.Session.SetInt32("UserId", -1);
            return Redirect("/");
        }


        [HttpPost]
        public async Task<IActionResult> LoginForm([Bind("Username, Password")] LoginForm form)
        {
            var user = _context.User.Where(m => m.Name == form.Username);

            if (user.Count() == 0)
            {
                return RedirectToAction("Index");
            }
            if (user.First().Password != form.Password)
            {
                return RedirectToAction("Index");
            }
            if (user.First().IsAdmin)
            {
                HttpContext.Session.SetInt32("IsAdmin", 1);
                HttpContext.Session.SetString("Username", user.First().Name);
                HttpContext.Session.SetInt32("UserId", user.First().Id);
                return Redirect("/Admin");
            }
            else
            {
                HttpContext.Session.SetInt32("IsAdmin", 0);
                HttpContext.Session.SetString("Username", user.First().Name);
                HttpContext.Session.SetInt32("UserId", user.First().Id);
                return Redirect("/");
            }

        }
    }
}