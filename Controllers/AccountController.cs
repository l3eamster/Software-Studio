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
    public class AccountController : Controller
    {
        private readonly DonutzStudioContext _context;
        public AccountController(DonutzStudioContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
            {
                return Redirect("/");
            }
            HttpContext.Session.Remove("Error");
            HttpContext.Session.Remove("Success");

            return View(await _context.User.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Ban(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            user.IsBan = !user.IsBan;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}