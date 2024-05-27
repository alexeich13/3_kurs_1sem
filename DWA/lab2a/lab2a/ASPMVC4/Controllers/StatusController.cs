using Microsoft.AspNetCore.Mvc;

namespace ASPMVC04.Controllers
{
    public class StatusController : Controller
    {
        private Random rnd;
        public StatusController()
        {
            rnd = new Random();
        }
        public IActionResult Index()
        {
            return StatusCode(200);
        }
        public IActionResult s200()
        {
            int stat1 = rnd.Next(200, 299);
            return StatusCode(stat1);
        }
        public IActionResult s300()
        {
            int stat2 = rnd.Next(300, 399);
            return StatusCode(stat2);
        }
        public IActionResult s500()
        {
            int stat3 = rnd.Next(500, 599);
            return StatusCode(stat3);
        }
    }
}
