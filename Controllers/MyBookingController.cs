using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DonutzStudio.Data;
using DonutzStudio.Models;

namespace DonutzStudio.Controllers
{
    public class MyBookingController : Controller
    {
        private readonly DonutzStudioContext _context;
        public MyBookingController(DonutzStudioContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Redirect("/");
            }
            if (HttpContext.Session.GetInt32("IsAdmin") == 1)
            {
                return Redirect("/");
            }
            var myId = HttpContext.Session.GetInt32("UserId");
            var bookings = await _context.Booking.Where(m => m.UserId == myId).ToListAsync();
            bookings = bookings.FindAll(m => CheckAvailable(m.Date, m.Time));
            var lab = await _context.Lab.ToListAsync();

            string[] TimeSlot = {
                "เช้า", "บ่าย", "ค่ำ"
            };

            List<dynamic> memDate = new List<dynamic>();

            List<dynamic> myBooking = new List<dynamic>();
            foreach (var booking in bookings)
            {
                var repeatCount = memDate.FindAll(date => date == booking.Date.Date).Count();
                if (repeatCount == 0)
                {
                    memDate.Add(booking.Date.Date);
                }

                var _Lab = lab.Where(m => m.Id == booking.LabId).First();
                myBooking.Add(new
                {
                    LabName = _Lab.Name,
                    Time = TimeSlot[booking.Time],
                    ItemName = _Lab.ItemName,
                    BookingID = booking.Id,
                    Date = booking.Date.Date
                });

            }
            List<dynamic> group = new List<dynamic>();
            foreach (var date in memDate)
            {
                group.Add(new
                {
                    Date = date,
                    Booking = myBooking.FindAll(book => GetObjectValue(book, "Date") == date)
                });
            }
            ViewBag.Mybooking = group;
            // Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(group));
            return View();
        }
        public dynamic GetObjectValue(object o, string propertyName) { return o.GetType().GetProperty(propertyName).GetValue(o, null); }

        public bool CheckAvailable(DateTime date, int time)
        {
            return DateTime.Compare(DateTime.Now.AddHours(7).Date, date.Date) <= 0 &&
            DateTime.Compare(DateTime.Now.AddHours(7), date.Date.AddHours(9 + time * 4)) < 0;
        }
    }
}