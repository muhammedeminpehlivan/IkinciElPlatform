using IkinciElPlatform.Data;
using Microsoft.AspNetCore.Mvc;

namespace IkinciElPlatform.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // ❌ HİÇ DB OKUMA YOK
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
