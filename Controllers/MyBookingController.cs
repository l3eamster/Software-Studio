using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace DonutzStudio.Controllers
{
    public class MyBookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}