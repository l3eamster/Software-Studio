using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DonutzStudio.Data;
using DonutzStudio.Models;

namespace DonutzStudio.Controllers
{
    public class LabController : Controller
    {
        private readonly DonutzStudioContext _context;

        public LabController(DonutzStudioContext context)
        {
            _context = context;
        }

        // GET: /Lab
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetInt32("IsAdmin")==1)
            {
                return Redirect("/Admin");
            }
            return View(await _context.Lab.ToListAsync());
        }

        // GET: /Lab/Booking/[Id]
        public async Task<IActionResult> Booking(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var lab = await _context.Lab.FindAsync(id);
            if (lab == null) return NotFound();

            string[] MonthName = {
                "ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.",
                "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.",
                "พ.ย.", "ธ.ค.",
            };

            List<dynamic> timelines = new List<dynamic>();
            for (var i = 0; i < 7; i++)
            {
                var date = DateTime.Now.AddDays(i);
                var bookings = _context.Booking.Where(m => m.LabId == id && m.Date.Date == date.Date);
                var time0 = bookings.Where(m => m.Time == 0).Count();
                var time1 = bookings.Where(m => m.Time == 1).Count();
                var time2 = bookings.Where(m => m.Time == 2).Count();
                var myBookings = bookings.Where(m => m.UserId == userId);
                var selected0 = myBookings.Where(m => m.Time == 0).Count() != 0;
                var selected1 = myBookings.Where(m => m.Time == 1).Count() != 0;
                var selected2 = myBookings.Where(m => m.Time == 2).Count() != 0;
                timelines.Add(new
                {
                    Date = date,
                    Day = date.Day,
                    Month = MonthName[date.Month - 1],
                    Year = date.Year + 543,
                    Time0 = lab.ItemCount - time0,
                    Time1 = lab.ItemCount - time1,
                    Time2 = lab.ItemCount - time2,
                    Selected0 = selected0,
                    Selected1 = selected1,
                    Selected2 = selected2,
                });
            }
            ViewBag.Timelines = timelines;
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(lab);
        }

        // POST: Lab/Booking
        [HttpPost]
        public async Task<JsonResult> Booking([FromBody] BookingForm form)
        {
            if (!ModelState.IsValid) return Json("Error");

            foreach (var book in form.BookingList)
            {
                for (var i = 0; i < book.Booking.Count(); i++)
                {
                    if (book.Booking[i] == false) continue;

                    Booking booking = new Booking();
                    booking.LabId = form.LabId;
                    booking.UserId = form.UserId;
                    booking.Date = DateTime.Parse(book.Date);
                    booking.Time = i;
                    if (!IsBookingExist(booking)) _context.Add(booking);
                }
            }
            await _context.SaveChangesAsync();
            return Json("OK");
        }

        // Utilities
        private bool IsBookingExist(Booking booking)
        {
            return _context.Booking.Any(m => m.LabId == booking.LabId && m.UserId == booking.UserId && m.Date.Date == booking.Date.Date && m.Time == booking.Time);
        }


        //
        // ===== FOR DEVELOPMENT =====
        //
        // GET: /Lab/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lab/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ItemName,ItemCount")] Lab lab)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lab);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lab);
        }
    }
}
