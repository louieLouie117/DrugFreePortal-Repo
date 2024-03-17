
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

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            System.Console.WriteLine("Reached backend of get users");

            // List<User> AllUsers = _context.Users.ToList();


            // lambda expression to get all users
            // List<User> AllUsers = _context.Users.Select(u => u).ToList();

            // lambda expression to get all users with null or empty check net8.0 new feature
            List<User> AllUsers = _context.Users?.Select(u => u).ToList() ?? new List<User>();



            return Json(AllUsers);
        }
    }
}