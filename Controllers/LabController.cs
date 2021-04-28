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
using System.Net.Http;

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
            if (HttpContext.Session.GetInt32("IsAdmin") == 1)
            {
                return Redirect("/Admin");
            }

            // Get Ming's labs data
            var json = await GetExternalLabs();
            var exLabs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExternalLab>>(json);
            ViewBag.ExternalLabs = exLabs;

            HttpContext.Session.Remove("Error");
            HttpContext.Session.Remove("Success");
            return View(await _context.Lab.ToListAsync());
        }

        // GET: /Lab/Booking/[Id]
        public async Task<IActionResult> Booking(int? id)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") == 1)
            {
                return Redirect("/");
            }
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
        public async Task<IActionResult> Booking([FromBody] BookingForm form)
        {
            if (form.UserId == -1)
            {
                HttpContext.Session.SetString("Error", "กรุณาลงชื่อเข้าใช้");
                return Redirect("/Login");
            }

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
            HttpContext.Session.SetString("Success", "การจองสำเร็จ");
            return Redirect($"/Lab/Booking/{form.LabId}");
        }

        // GET: /Lab/GetData
        public async Task<string> GetData()
        {
            var labs = await _context.Lab.ToListAsync();
            List<dynamic> data = new List<dynamic>();
            for (var i = 0; i < 5; i++)
            {
                data.Add(new
                {
                    labName = labs.ElementAt(i).Name,
                    itemName = labs.ElementAt(i).ItemName,
                    labImage = labs.ElementAt(i).LabImage,
                    itemAmount = labs.ElementAt(i).ItemCount,
                    link = String.Format("https://{0}/Lab/Booking/{1}", Request.Host, labs.ElementAt(i).Id),
                });
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json;
        }

        // Utilities
        private bool IsBookingExist(Booking booking)
        {
            return _context.Booking.Any(m => m.LabId == booking.LabId && m.UserId == booking.UserId && m.Date.Date == booking.Date.Date && m.Time == booking.Time);
        }

        public dynamic GetObjectValue(object o, string propertyName) { return o.GetType().GetProperty(propertyName).GetValue(o, null); }

        private async Task<string> GetExternalLabs()
        {
            return "[{\"labName\":\"ห้องแอลฟา\",\"itemName\":\"ไม้บรรทัด1\",\"labImage\":\"https://www.oetker.ca/Recipe/Recipes/oetker.ca/ca-en/baking/image-thumb__24580__RecipeDetailsLightBox/farmers-salad.jpg\",\"itemAmount\":20,\"link\":\"/Lab/Booking/1\"},{\"labName\":\"ห้องเบตา\",\"itemName\":\"ไม้บรรทัด\",\"labImage\":\"https://imagesvc.meredithcorp.io/v3/mm/image?url=https%3A%2F%2Fimg1.cookinglight.timeinc.net%2Fsites%2Fdefault%2Ffiles%2Fstyles%2Fmedium_2x%2Fpublic%2Fimage%2F2017%2F01%2Fmain%2Fhalf-moon-browned-omelet.jpg%3Fitok%3DmGBP10Co\",\"itemAmount\":6,\"link\":\"/Lab/Booking/1\"},{\"labName\":\"qqq\",\"itemName\":\"www\",\"labImage\":\"https://th-test-11.slatic.net/p/81e1bb7220056fc95dfbc664819b9d91.jpg\",\"itemAmount\":5,\"link\":\"/Lab/Booking/1\"},{\"labName\":\"Test2\",\"itemName\":\"asdfsda\",\"labImage\":\"https://www.cpbrandsite.com/contents/recipe/8pfgfhpu8lne4utnnh8qfjdleup17w1kzw3ubbhl.png\",\"itemAmount\":5,\"link\":\"/Lab/Booking/1\"},{\"labName\":\"Omega\",\"itemName\":\"Macbook\",\"labImage\":\"https://pbs.twimg.com/media/CzcftTpW8AA1_Ov.jpg\",\"itemAmount\":5,\"link\":\"/Lab/Booking/1\"}]";

            string baseUrl = "https://random-data-api.com/api/blood/random_blood";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            if (data == null) return "";
                            return data;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return "";
            }
        }


        //
        // ===== FOR DEVELOPMENT =====
        //
        // GET: /Lab/DangerouslyCreateLab
        public IActionResult DangerouslyCreateLab()
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
                return Redirect("/");
            return View();
        }

        // POST: Lab/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ItemName,ItemCount,ItemImage,Color")] Lab lab)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
                return Redirect("/");

            if (ModelState.IsValid)
            {
                _context.Add(lab);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lab);
        }

        // GET: /Lab/DangerouslyCreateLab/[Id]
        public async Task<IActionResult> DangerouslyDeleteLab(int id)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
                return Redirect("/");
            var lab = await _context.Lab.FindAsync(id);
            if (lab == null) return Redirect("/");
            return View(lab);
        }

        // POST: Lab/Delete/[Id]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("IsAdmin") != 1)
            {
                return Redirect("/");
            }

            var lab = await _context.Lab.FindAsync(id);
            if (lab == null) return NotFound();

            var labBookingList = _context.Booking.Where(m => m.LabId == lab.Id);
            foreach (var booking in labBookingList)
            {
                _context.Remove(booking);
            }
            _context.Remove(lab);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
