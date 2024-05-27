using lab2b.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace lab2b.Controllers
{
    public class TMResearch : Controller
    {

        [HttpGet("/MResearch/M01/1")]
        [HttpGet("/MResearch/M01")]
        [HttpGet("/MResearch")]
        [HttpGet("/")]
        [HttpGet("V2/MResearch/M01")]
        [HttpGet("V3/MResearch/{inputString}/M01")]
        public IActionResult MO1()
        {
            return Content("GET:MO1");
        }
        [HttpGet("/V2")]
        [HttpGet("V2/MResearch")]
        [HttpGet("V2/MResearch/M02")]
        [HttpGet("/MResearch/M02")]
        [HttpGet("V3/MResearch/string/M02")]
        public IActionResult MO2()
        {
            return Content("GET:MO2");
        }
        [HttpGet("/V3")]
        [HttpGet("V3/MResearch/string/")]
        [HttpGet("V3/MResearch/string/M03")]


        public IActionResult MO3()
        {
            return Content("GET:MO3");
        }
        [HttpGet("{*uri}")]
        public IActionResult MOXX()
        {
            return Content("GET:MXX");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}