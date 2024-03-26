
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
        [HttpPost("RegisterStudentMethod")]
        public IActionResult RegisterStudentMethod(User dataFromUser)
        {

            // check if any fields are empty with a list
            List<string> emptyFields = new List<string>();
            if (string.IsNullOrEmpty(dataFromUser.School))
            {
                emptyFields.Add("School");
            }
            if (string.IsNullOrEmpty(dataFromUser.StudentId))
            {
                emptyFields.Add("StudentId");
            }

            if (string.IsNullOrEmpty(dataFromUser.FirstName))
            {
                emptyFields.Add("FirstName");
            }

            if (string.IsNullOrEmpty(dataFromUser.LastName))
            {
                emptyFields.Add("LastName");
            }

            if (string.IsNullOrEmpty(dataFromUser.Email))
            {
                emptyFields.Add("Email");
            }

            if (string.IsNullOrEmpty(dataFromUser.Password))
            {
                emptyFields.Add("Password");
            }

            if (string.IsNullOrEmpty(dataFromUser.PhoneNumber))
            {
                emptyFields.Add("PhoneNumber");
            }

            if (emptyFields.Any())
            {
                return Json(new { Status = "cannot be empty", Fields = emptyFields });

            }

            // return json reach backend message
            System.Console.WriteLine("Reached backend of register student");
            System.Console.WriteLine($"Reached backend of student registration");
            System.Console.WriteLine($"Account type: {AccountType.Student}");
            System.Console.WriteLine($"school: {dataFromUser.School}");
            System.Console.WriteLine($"student id: {dataFromUser.StudentId}");

            System.Console.WriteLine($"First Name: {dataFromUser.FirstName}");
            System.Console.WriteLine($"Last Name: {dataFromUser.LastName}");
            System.Console.WriteLine($"email: {dataFromUser.Email}");
            System.Console.WriteLine($"password: {dataFromUser.Password}");
            System.Console.WriteLine($"phone number: {dataFromUser.PhoneNumber}");

            // stripe customer id none
            dataFromUser.StripeCustomerId = "none";
            System.Console.WriteLine($"stripe customer id: {dataFromUser.StripeCustomerId}");
            System.Console.WriteLine($"subscription status: {dataFromUser.SubscriptionStatus}");

            // set terms to true
            dataFromUser.AcceptedTerms = true;

            System.Console.WriteLine($"accepted terms: {dataFromUser.AcceptedTerms}");

            // set release version to R1.0
            dataFromUser.ReleaseVersion = "R1.0";
            System.Console.WriteLine($"release version: {dataFromUser.ReleaseVersion}");




            // check if user already exists
            User? userExists = _context.Users?.FirstOrDefault(u => u.Email == dataFromUser.Email);
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

            // add user id to session
            HttpContext.Session.SetInt32("UserId", dataFromUser.UserId);


            return Json(new { Status = "Registered", Fields = emptyFields });





        }


        [HttpPost("RegisterEvaluatorMethod")]
        public IActionResult RegisterEvaluatorMethod(User dataFromUser)
        {

            // check if any fields are empty with a list
            List<string> emptyFields = new List<string>();

            if (string.IsNullOrEmpty(dataFromUser.FirstName))
            {
                emptyFields.Add("FirstName");
            }

            if (string.IsNullOrEmpty(dataFromUser.LastName))
            {
                emptyFields.Add("LastName");
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
                return Json(new { Status = "cannot be empty", Fields = emptyFields });
            }
            // check if user already exists
            User? userExists = _context.Users?.FirstOrDefault(u => u.Email == dataFromUser.Email);
            if (userExists != null)
            {
                return Json(new { Status = "User already exists" });
            }

            // account type student
            dataFromUser.AccountType = AccountType.Evaluator;
            dataFromUser.AcceptedTerms = true;
            dataFromUser.ReleaseVersion = "R1.0";

            System.Console.WriteLine("Reached backend of register evaluator");

            System.Console.WriteLine($"Account type: {dataFromUser.AccountType}");
            System.Console.WriteLine($"First Name: {dataFromUser.FirstName}");
            System.Console.WriteLine($"Last Name: {dataFromUser.LastName}");
            System.Console.WriteLine($"email: {dataFromUser.Email}");
            System.Console.WriteLine($"password: {dataFromUser.Password}");


            System.Console.WriteLine($"school: {dataFromUser.School}");
            System.Console.WriteLine($"student id: {dataFromUser.StudentId}");
            System.Console.WriteLine($"phone number: {dataFromUser.PhoneNumber}");

            // Not needed for evaluator
            dataFromUser.StripeCustomerId = "Not needed for evaluator";
            dataFromUser.SubscriptionStatus = SubscriptionStatus.Active;
            dataFromUser.School = "Not needed for evaluator";
            dataFromUser.StudentId = "Not needed for evaluator";
            dataFromUser.CheckedIn = false;
            dataFromUser.PhoneNumber = "Not needed for evaluator";



            // hash password
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            dataFromUser.Password = Hasher.HashPassword(dataFromUser, dataFromUser.Password);

            // save to database
            if (_context.Users != null)
            {
                _context.Users.Add(dataFromUser);
                _context.SaveChanges();
            }

            HttpContext.Session.SetInt32("UserId", dataFromUser.UserId);



            return Json(new { Status = "Evaluator Registered", Fields = emptyFields });
        }




        [HttpPost("RegisterDeanMethod")]
        public IActionResult RegisterDeanMethod(User dataFromUser)
        {

            // check if any fields are empty with a list
            List<string> emptyFields = new List<string>();

            if (string.IsNullOrEmpty(dataFromUser.School))
            {
                emptyFields.Add("School");
            }

            if (string.IsNullOrEmpty(dataFromUser.FirstName))
            {
                emptyFields.Add("FirstName");
            }

            if (string.IsNullOrEmpty(dataFromUser.LastName))
            {
                emptyFields.Add("LastName");
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
                return Json(new { Status = "cannot be empty", Fields = emptyFields });
            }
            // check if user already exists
            User? userExists = _context.Users?.FirstOrDefault(u => u.Email == dataFromUser.Email);
            if (userExists != null)
            {
                return Json(new { Status = "User already exists" });
            }

            // account type student
            dataFromUser.AccountType = AccountType.Dean;
            dataFromUser.AcceptedTerms = true;
            dataFromUser.ReleaseVersion = "R1.0";

            System.Console.WriteLine("Reached backend of register Dean");

            System.Console.WriteLine($"Account type: {dataFromUser.AccountType}");
            System.Console.WriteLine($"First Name: {dataFromUser.FirstName}");
            System.Console.WriteLine($"Last Name: {dataFromUser.LastName}");
            System.Console.WriteLine($"email: {dataFromUser.Email}");
            System.Console.WriteLine($"password: {dataFromUser.Password}");


            System.Console.WriteLine($"school: {dataFromUser.School}");
            System.Console.WriteLine($"student id: {dataFromUser.StudentId}");
            System.Console.WriteLine($"phone number: {dataFromUser.PhoneNumber}");

            // Not needed for Dean
            dataFromUser.StripeCustomerId = "Not needed for Dean";
            dataFromUser.SubscriptionStatus = SubscriptionStatus.Active;
            dataFromUser.StudentId = "Not needed for Dean";
            dataFromUser.CheckedIn = false;
            dataFromUser.PhoneNumber = "Not needed for Dean";



            // hash password
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            dataFromUser.Password = Hasher.HashPassword(dataFromUser, dataFromUser.Password);

            // save to database
            if (_context.Users != null)
            {
                _context.Users.Add(dataFromUser);
                _context.SaveChanges();
            }

            HttpContext.Session.SetInt32("UserId", dataFromUser.UserId);



            return Json(new { Status = "Dean Registered", Fields = emptyFields });
        }


        [HttpPost("RegisterAdminMethod")]
        public IActionResult RegisterAdminMethod(User dataFromUser)
        {

            // check if any fields are empty with a list
            List<string> emptyFields = new List<string>();

            if (string.IsNullOrEmpty(dataFromUser.FirstName))
            {
                emptyFields.Add("FirstName");
            }

            if (string.IsNullOrEmpty(dataFromUser.LastName))
            {
                emptyFields.Add("LastName");
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
                return Json(new { Status = "cannot be empty", Fields = emptyFields });
            }
            // check if user already exists
            User? userExists = _context.Users?.FirstOrDefault(u => u.Email == dataFromUser.Email);
            if (userExists != null)
            {
                return Json(new { Status = "User already exists" });
            }

            // check password = to AdminPassword!DRP
            if (dataFromUser.Password != "Admin!DFPPa$$2Da$h")
            {
                return Json(new { Status = "Admin Password Not working!" });
            }

            // account type student
            dataFromUser.AccountType = AccountType.Admin;
            dataFromUser.Password = "AdminPassword!DRP";
            dataFromUser.AcceptedTerms = true;
            dataFromUser.ReleaseVersion = "R1.0";

            System.Console.WriteLine("Reached backend of register Dean");

            System.Console.WriteLine($"Account type: {dataFromUser.AccountType}");
            System.Console.WriteLine($"First Name: {dataFromUser.FirstName}");
            System.Console.WriteLine($"Last Name: {dataFromUser.LastName}");
            System.Console.WriteLine($"email: {dataFromUser.Email}");
            System.Console.WriteLine($"password: {dataFromUser.Password}");


            System.Console.WriteLine($"school: {dataFromUser.School}");
            System.Console.WriteLine($"student id: {dataFromUser.StudentId}");
            System.Console.WriteLine($"phone number: {dataFromUser.PhoneNumber}");

            // Not needed for Dean
            dataFromUser.StripeCustomerId = "Not needed for Dean";
            dataFromUser.SubscriptionStatus = SubscriptionStatus.Active;
            dataFromUser.School = "Admin not assigned school";
            dataFromUser.StudentId = "Not needed for Dean";
            dataFromUser.CheckedIn = false;
            dataFromUser.PhoneNumber = "Not needed for Dean";



            // hash password
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            dataFromUser.Password = Hasher.HashPassword(dataFromUser, dataFromUser.Password);

            // save to database
            if (_context.Users != null)
            {
                _context.Users.Add(dataFromUser);
                _context.SaveChanges();
            }

            HttpContext.Session.SetInt32("UserId", dataFromUser.UserId);



            return Json(new { Status = "Admin Registered", Fields = emptyFields });
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser fromData)
        {
            System.Console.WriteLine("Reached backend of login");
            // writeline user email and password
            System.Console.WriteLine($"Email: {fromData.Email}");
            System.Console.WriteLine($"Password: {fromData.Password}");




            return Json(new { Status = "Login Successfule" });


        }


        [HttpPost("LoginFetch")]
        public IActionResult LoginFetch([FromBody] LoginUser loginData)
        {
            System.Console.WriteLine("Reached backend of login fetch");
            System.Console.WriteLine($"EmailFormData: {loginData.Email}");
            System.Console.WriteLine($"PasswordFormData: {loginData.Password}");
            // careatea list and check if email is null or empty if null or empty add to list
            List<string> emptyFields = new List<string>();
            if (string.IsNullOrEmpty(loginData.Email))
            {
                emptyFields.Add("Email");
            }
            if (string.IsNullOrEmpty(loginData.Password))
            {
                emptyFields.Add("Password");
            }
            // if any fields are empty return json cannot be empty
            if (emptyFields.Any())
            {
                return Json(new { Status = "cannot be empty", Fields = emptyFields });
            }

            // check if user is in database if not return json user not found
            if (string.IsNullOrEmpty(loginData.Password))
            {
                return Json(new { Status = "Password cannot be empty" });
            }


            User? userInDB = _context.Users?.FirstOrDefault(u => u.Email == loginData.Email);

            if (userInDB == null)
            {
                Console.WriteLine($"email error");

                ModelState.AddModelError("Email", "Invalid Email/Password");
                return Json(new { Status = "email error no user found" });
            }


            var hasher = new PasswordHasher<LoginUser>();
            var passwordResult = userInDB != null ? hasher.VerifyHashedPassword(loginData, userInDB.Password, loginData.Password) : 0;
            if (passwordResult == 0)
            {
                // Still need these for debugging? Console.Writelines should be removed
                // something else should happer here besides a WriteLine
                return Json(new { Status = "password error" });
            }

            System.Console.WriteLine($"----------------userID from DB {userInDB?.UserId}");
            HttpContext.Session.SetInt32("UserId", userInDB?.UserId ?? 0);
            // get user id from session
            int? UserId = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine($"----------------UserId in session Reg:LogInFetch {UserId}");



            return Ok(new { Status = "Login Fetch Successfule", Fields = emptyFields });
        }




    }
}