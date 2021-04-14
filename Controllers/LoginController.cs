using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace DonutzStudio.Controllers
{
    public class LoginController : Controller
    {
        public string Index()
        {
            return "Login page"; 
        }
    }
}