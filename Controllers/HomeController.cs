
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

            // get user id from session
            int? UserIdInSession = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine($"----------------UserId in session Home:GetUsers => {UserIdInSession}");

            string accountType = HttpContext.Session.GetString("AccountType") ?? string.Empty;
            System.Console.WriteLine($"-------------------AccountType in session Home:Dashboard => {accountType}");

            // if user is not logged in 
            if (UserIdInSession == null)
            {
                return RedirectToAction("index");
            }

            User? UserIndb = _context.Users?
                .FirstOrDefault(u => u.UserId == UserIdInSession);



            ViewBag.ToDisplay = UserIndb;
            ViewBag.allUserLogs = _context.Users
                ?.Where(ul => ul.UserId == UserIdInSession)
                .ToList();

            // List<User> ActiveUser = _context.Users
            //                 ?.Where(ul => ul.UserId == UserIdInSession)
            //                 .ToList() ?? new List<User>();

            DashboardWrapper wMod = new DashboardWrapper();
            wMod.User = UserIndb;



            return View("dashboard", wMod);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            System.Console.WriteLine("Reached backend of get users");

            // lambda expression to get all users with null or empty check net8.0 new feature
            List<User> AllUsers = _context.Users?.Select(u => u).ToList() ?? new List<User>();

            return Ok(new { Status = "Success", UsersList = AllUsers });
        }

        // file upload
        [HttpPost("UploadFile")]
        public IActionResult UploadFile(UploadFile file)
        {
            System.Console.WriteLine("Reached backend of UploadFile");
            System.Console.WriteLine($"-------------------File => {file.FileName}");






            return RedirectToAction("dashboard");
        }
    }
}