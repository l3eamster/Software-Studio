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

        // GET: /Account
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
            {
                return Redirect("/");
            }

            var userList = await _context.User.ToListAsync();
            userList.Reverse();

            return View(userList);
        }

        // POST: /Account/Ban/[id]
        [HttpPost]
        public async Task<IActionResult> Ban(int id)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
            {
                return Redirect("/");
            }

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            user.IsBan = !user.IsBan;
            _context.Update(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("Success", !user.IsBan ? "ยกเลิกการระงับบัญชีสำเร็จ" : "ระงับบัญชีผู้ใช้สำเร็จ");

            return RedirectToAction(nameof(Index));
        }

        // POST: /Account/Delete/[id]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
            {
                return Redirect("/");
            }

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            var userBookingList = _context.Booking.Where(m => m.UserId == user.Id);
            foreach (var booking in userBookingList)
            {
                _context.Remove(booking);
            }
            _context.Remove(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("Success", "ลบบัญชีสำเร็จ");

            return RedirectToAction(nameof(Index));
        }
    }
}