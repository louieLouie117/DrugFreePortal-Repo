
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

            // check if any fields are empty with a list
            List<string> emptyFields = new List<string>();

            if (string.IsNullOrEmpty(dataFromUser.FirstName))
            {
                emptyFields.Add("First name");
            }

            if (string.IsNullOrEmpty(dataFromUser.LastName))
            {
                emptyFields.Add("Last name");
            }

            if (string.IsNullOrEmpty(dataFromUser.Email))
            {
                emptyFields.Add("Email");
            }

            if (string.IsNullOrEmpty(dataFromUser.Password))
            {
                emptyFields.Add("Password");
            }

            if (emptyFields.Any())
            {
                return Json(new { Status = $"{string.Join(", ", emptyFields)} cannot be empty" });
            }

            // return json reach backend message
            System.Console.WriteLine("Reached backend of register student");
            System.Console.WriteLine($"Reached backend of student registration");
            System.Console.WriteLine($"Account type: {AccountType.Student}");
            System.Console.WriteLine($"First Name: {dataFromUser.FirstName}");
            System.Console.WriteLine($"Last Name: {dataFromUser.LastName}");
            System.Console.WriteLine($"email: {dataFromUser.Email}");

            // check if user already exists
            User userExists = _context.Users.FirstOrDefault(u => u.Email == dataFromUser.Email);
            if (userExists != null)
            {
                return Json(new { Status = "User already exists" });
            }
            // account type student
            dataFromUser.AccountType = AccountType.Student;

            // hash password
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            dataFromUser.Password = Hasher.HashPassword(dataFromUser, dataFromUser.Password);


            // save to database
            if (_context.Users != null)
            {
                _context.Users.Add(dataFromUser);
                _context.SaveChanges();
            }


            return Json(new { Status = "Success" });





        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser fromData)
        {
            System.Console.WriteLine("Reached backend of login");

            return Json(new { Status = false });


        }



    }
}