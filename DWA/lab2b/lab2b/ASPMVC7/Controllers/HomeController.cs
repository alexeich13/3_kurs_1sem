using ASPMVC7.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ASPMVC7.Controllers
{
    public class TAResearchController : Controller
    {
        [HttpGet]
        [Route("it/{n:int}/{inputString}", Name = "MO4")]
        public IActionResult MO4(int n, string inputString)
        {
            string responseMessage = $"GET:M04:/{n}/{inputString}";
            return Content(responseMessage);
        }

        [HttpGet]
        [Route("it/{b:bool}/{letters}", Name = "GetM05")]
        public IActionResult GetM05(bool b, string letters)
        {
            string responseMessage = $"GET:M05:/{b}/{letters}";
            return Content(responseMessage);
        }

        [HttpPost]
        [Route("it/{b:bool}/{letters}", Name = "GetM05")]
        public IActionResult PostM05(bool b, string letters)
        {
            string responseMessage = $"POST:M05:/{b}/{letters}";
            return Content(responseMessage);
        }

        [HttpGet]
        [Route("it/{f:float}/{letters:length(2,5)}")]
        public IActionResult GetM06(float f, string letters)
        {
            string responseMessage = $"GET:M06:/{f}/{letters}";
            return Content(responseMessage);
        }

        [HttpDelete]
        [Route("it/{f:float}/{letters:length(2,5)}")]
        public IActionResult DeleteM06(float f, string letters)
        {
            string responseMessage = $"Delete:M06:/{f}/{letters}";
            return Content(responseMessage);
        }
        [HttpPut]
        [Route("it/{letters:length(3,4)}/{n:int:range(100,200)}/")]
        public IActionResult PutM07(string letters, int n)
        {
            string responseMessage = $"PUT:M07:/{letters}/{n}/";
            return Content(responseMessage);
        }

        [HttpPost]
        [Route("it/{mail}")]
        public IActionResult M08(string mail) => IsGmailAddress(mail) ? Ok($"POST08:{mail}") : BadRequest("Недействительный адрес Gmail");
        private static bool IsGmailAddress(string mail)
        {
            var gmailPattern = @"^[a-zA-Z0-9._%+-]+@gmail.com$";
            return Regex.IsMatch(mail, gmailPattern);
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