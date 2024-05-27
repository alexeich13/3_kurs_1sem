using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace laba3a.Controllers
{
    public class CalcController : Controller
    {
        public IActionResult Index()
        {
            return View("Calc");
        }
        [HttpPost]
        public IActionResult Sum(float? x, float? y)
        {
            ViewBag.press = "+";
            if (x is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            else if (y is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            float? xValue = x;
            float? yValue = y;
            float? zValue;
            zValue = xValue + yValue;
            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = zValue;
            return View("Calc");
        }
        [HttpPost]
        public IActionResult Sub(float? x, float? y)
        {
            ViewBag.press = "-";
            if (x is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            else if (y is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            float? xValue = x;
            float? yValue = y;
            float? zValue;
            zValue = xValue - yValue;
            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = zValue;
            return View("Calc");
        }
        [HttpPost]
        public IActionResult Mul(float? x, float? y)
        {
            ViewBag.press = "*";
            if (x is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            else if (y is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            float? xValue = x;
            float? yValue = y;
            float? zValue;
            zValue = xValue * yValue;
            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = zValue;
            return View("Calc");
        }
        [HttpPost]
        public IActionResult Div(float? x, float? y)
        {
            ViewBag.press = "/";
            if (x is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            else if (y is null)
            {
                ViewBag.z = null;
                ViewBag.Error = "--ERROR--";
                return View("Calc");
            }
            else if (y == 0)
            {
                ViewBag.z = null;
                ViewBag.Error = "Division by zero not allowed";
                return View("Calc");
            }
            float? xValue = x;
            float? yValue = y;
            float? zValue;
            zValue = xValue / yValue;
            ViewBag.x = xValue;
            ViewBag.y = yValue;
            ViewBag.z = zValue;
            return View("Calc");
        }
        [Route("/")]
        public IActionResult _Calc(string? press, float? x, float? y)
        {
            ViewBag.press = press;
            ViewBag.x = x;
            ViewBag.y = y;
            ViewBag.z = 0;
            return View("Calc");
        }
        public IActionResult _CalcLink()
        {
            return View("Calc");
        }
    }
}
