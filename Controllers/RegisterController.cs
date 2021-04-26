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
    public class RegisterController : Controller
    {
        private readonly DonutzStudioContext _context;
        public RegisterController(DonutzStudioContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,Password2")] RegisterForm form)
        {
            if (form.Password != form.Password2)
            {
                HttpContext.Session.SetString("Error", "Password and Confirm password mismatch");
                return RedirectToAction("Index");
            }
            var formOld = _context.User.Where(m => m.Name == form.Username);
            if (formOld.Count() != 0)
            {
                HttpContext.Session.SetString("Error", "Username already exist");
                return RedirectToAction("Index");
            }

            var user = new User();
            user.Name = form.Username;
            user.Password = form.Password;
            user.IsAdmin = false;
            user.Created = DateTime.Now;
            _context.Add(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Error");
            HttpContext.Session.SetString("Username", user.Name);
            HttpContext.Session.SetInt32("UserId", user.Id);

            return Redirect("/");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}