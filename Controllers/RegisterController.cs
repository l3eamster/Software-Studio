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
            if (form.Username.Count() < 4)
            {
                return RedirectToAction("Index");
            }
            if (form.Password.Count() < 6)
            {
                return RedirectToAction("Index");
            }
            if (form.Password != form.Password2)
            {
                return RedirectToAction("Index");
            }
            var formOld = _context.User.Where(m => m.Name == form.Username);
            if (formOld.Count() != 0)
            {
                return RedirectToAction("Index");
            }

            var user = new User();
            user.Name = form.Username;
            user.Password = form.Password;
            user.IsAdmin = false;
            _context.Add(user);
            await _context.SaveChangesAsync();
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