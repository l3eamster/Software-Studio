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
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return Redirect("/");
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("IsAdmin");
            HttpContext.Session.Remove("Error");
            HttpContext.Session.Remove("Success");
            return Redirect("/");
        }


        [HttpPost]
        public IActionResult LoginForm([Bind("Username, Password")] LoginForm form)
        {
            var user = _context.User.Where(m => m.Name == form.Username);

            // Verify
            if (user.Count() == 0)
            {
                HttpContext.Session.SetString("Error", "ไม่พบบัญชีผู้ใช้");
                return RedirectToAction("Index");
            }
            if (user.First().IsBan)
            {
                HttpContext.Session.SetString("Error", "บัญชีนี้ถูกระงับการใช้งาน");
                return RedirectToAction("Index");
            }
            if (user.First().Password != form.Password)
            {
                HttpContext.Session.SetString("Error", "รหัสผ่านไม่ถูกต้อง");
                return RedirectToAction("Index");
            }

            // Login
            if (user.First().IsAdmin)
            {
                HttpContext.Session.Remove("Error");
                HttpContext.Session.Remove("Success");
                HttpContext.Session.SetInt32("IsAdmin", 1);
                HttpContext.Session.SetString("Username", user.First().Name);
                HttpContext.Session.SetInt32("UserId", user.First().Id);
                return Redirect("/Admin");
            }
            else
            {
                HttpContext.Session.Remove("Error");
                HttpContext.Session.Remove("Success");
                HttpContext.Session.SetInt32("IsAdmin", 0);
                HttpContext.Session.SetString("Username", user.First().Name);
                HttpContext.Session.SetInt32("UserId", user.First().Id);
                return Redirect("/");
            }

        }
    }
}