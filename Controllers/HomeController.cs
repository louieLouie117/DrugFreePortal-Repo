
using Microsoft.AspNetCore.Mvc;


namespace DrugFreePortal.Models
{
    public class HomeController : Controller
    {

        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")] // This is the route for the index page
        public IActionResult index()
        {
            return View("index");
        }


        [HttpGet("/dashboard")] // This is the route for the index page
        public IActionResult dashboard()
        {
            return View("dashboard");
        }
    }
}