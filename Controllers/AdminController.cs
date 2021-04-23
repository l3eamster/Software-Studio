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
    public class AdminController : Controller
    {
        private readonly DonutzStudioContext _context;
        public AdminController(DonutzStudioContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetInt32("IsAdmin")==0)
            {
                return Redirect("/");
            }
            return View(await _context.Lab.ToListAsync());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var lab = await _context.Lab.FindAsync(id);
            if (lab == null) return NotFound();

            return View(lab);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ItemName,ItemCount")] Lab lab)
        {
            if (id != lab.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lab);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!labExists(lab.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lab);
        }
        private bool labExists(int id)
        {
            return _context.Lab.Any(e => e.Id == id);
        }
    }
}