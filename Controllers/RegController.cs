
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
        public IActionResult RegisterStudent(User dataFromUser)
        {

            // return json reach backend message
            System.Console.WriteLine("Reached backend of register student");

            System.Console.WriteLine($"Reached backend of student registration");
            System.Console.WriteLine($"Account type: {AccountType.Student}");
            System.Console.WriteLine($"First Name: {dataFromUser.FirstName}");
            System.Console.WriteLine($"Last Name: {dataFromUser.LastName}");
            System.Console.WriteLine($"email: {dataFromUser.Email}");

            // account type student
            dataFromUser.AccountType = AccountType.Student;

            // save to database
            if (_context.Users != null)
            {
                _context.Users.Add(dataFromUser);
                _context.SaveChanges();
            }

            return Json(new { Status = "reach backend" });



        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser fromData)
        {
            System.Console.WriteLine("Reached backend of login");

            return Json(new { Status = false });


        }



    }
}