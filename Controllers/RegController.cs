
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace DrugFreePortal.Models
{
    public class RegController : Controller
    {

        private MyContext _context;
        public RegController(MyContext context)
        {
            _context = context;
        }

        // register a new student   
        [HttpPost("RegisterStudent")]
        public IActionResult RegisterStudent(User fromData)
        {
            System.Console.WriteLine($"Reached backend of student registration");
            System.Console.WriteLine($"email: {fromData.Email}");

            return Json(new { Status = "Reached backend of student registration" });
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser fromData)
        {
            System.Console.WriteLine("Reached backend of login");

            return Json(new { Status = false });


        }



    }
}