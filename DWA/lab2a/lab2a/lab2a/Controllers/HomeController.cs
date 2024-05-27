using Microsoft.AspNetCore.Mvc;

namespace lab2a.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}