
using Microsoft.AspNetCore.Mvc;



namespace DrugFreePortal.Models
{
    public class AdminController : Controller
    {


        private MyContext _context;
        public IWebHostEnvironment _webHostEnvironment;

        public readonly IConfiguration _config;


        public AdminController(MyContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _config = configuration;
        }


        [HttpGet("AdminCheckInStudent/{userId}")]
        public IActionResult AdminCheckInStudent(int userId)
        {  // Access the JSON UserToCheckIn from the request
            var checkQueue = _context.Queues?
                .FirstOrDefault(u => u.StudentUserId == userId);

            if (checkQueue != null)
            {
                return Ok(new { message = "User has already been check in." });
            }
            System.Console.WriteLine($"----------------UserId in Admin:GetUsers => {userId}");

            // get user from session with lambda expression
            var UserFound = _context.Users?
                .FirstOrDefault(u => u.UserId == userId);

            // use the user data to add to the queue; SchoolId, UserId, StudentName, StudentUserId, first name, last name, email, phone number and status = "Start"
            Queue newQueue = new Queue
            {
                SchoolId = UserFound.SchoolId,
                SchoolName = UserFound.School,
                StudentId = 0, // Set the value of the 'StudentId' property
                StudentUserId = UserFound.UserId, // Set the value of the 'StudentUserId' property
                FirstName = UserFound.FirstName,
                LastName = UserFound.LastName,
                Email = UserFound.Email,
                PhoneNumber = UserFound.PhoneNumber,
                Status = "Start"
            };

            // add the new queue to the database
            _context.Queues.Add(newQueue);
            _context.SaveChanges();

            return Ok(new
            {
                UserInSession = UserFound,
                QueueData = newQueue,
                message = "Successfully check-in."
            });
        }  // Access the JSON UserToCheckIn from the request


        [HttpGet("AdminSignInStudent/{userId}/{schoolId}")]
        public IActionResult AdminSignInStudent(int userId, int schoolId)
        {
            System.Console.WriteLine($"----------------UserId in Admin:GetUsers => {userId}");

            // set userId to session 
            HttpContext.Session.SetInt32("UserId", userId);
            // set schoolId to session session
            HttpContext.Session.SetInt32("SchoolIdInSession", schoolId);

            // return ok message "Session set"
            return Ok(new { message = "You are now being signin to the students dashboard! Select Ok to continue." });

        }




        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            System.Console.WriteLine("Reached backend of get users");

            // lambda expression to get all student users with null or empty check net8.0 new feature
            List<User> AllStudents = _context.Users?.Where(u => (int)u.AccountType == 2).ToList() ?? new List<User>();
            // descending order
            AllStudents = AllStudents.OrderByDescending(u => u.CreatedAt).ToList();


            // lambda expression to get all student users with null or empty check net8.0 new feature
            List<User> AllAdmins = _context.Users?.Where(u => (int)u.AccountType == 0).ToList() ?? new List<User>();
            // descending order
            AllAdmins = AllAdmins.OrderByDescending(u => u.CreatedAt).ToList();

            // lambda expression to get all evaluators
            List<User> AllEvaluators = _context.Users?.Where(u => (int)u.AccountType == 3).ToList() ?? new List<User>();

            // lambda expression to get all deans 1
            List<User> AllDeans = _context.Users?.Where(u => (int)u.AccountType == 1).ToList() ?? new List<User>();


            return Ok(new { Status = "Success", StudentList = AllStudents, AdminList = AllAdmins, EvaluatorList = AllEvaluators, DeanList = AllDeans });
        }






        [HttpPost("AddCompliance")]
        public IActionResult ComplianceMethod([FromBody] ComplianceType DataFromUser)
        {
            // Access the JSON DataFromUser from the request
            System.Console.WriteLine(DataFromUser.Name);

            // add to database
            _context.ComplianceTypes?.Add(DataFromUser);
            _context.SaveChanges();
            var complianceTypes = _context.ComplianceTypes?.OrderByDescending(c => c.Name).ToList();

            return Ok(new { data = complianceTypes, message = "Reached backend of adding new compliance" });
        }


        [HttpPut("EditCompliance")]
        public IActionResult EditMethod([FromBody] ComplianceType DataFromUser)
        {
            if (DataFromUser == null)
            {
                // Handle the case where DataFromUser is null
                return BadRequest("Data from user is null");
            }

            // Check for null properties if necessary
            if (DataFromUser.Name == null || DataFromUser.Details == null)
            {
                // Handle the case where required properties are null
                return BadRequest("One or more required properties are null");
            }

            try
            {
                // Your existing logic to edit the compliance type
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            System.Console.WriteLine($"Compliance type edited successfully:{DataFromUser.ComplianceTypeId} {DataFromUser.Name}, {DataFromUser.Details}");
            // save changes to the database
            _context.ComplianceTypes?.Update(DataFromUser);
            _context.SaveChanges();


            return Ok("Compliance type edited successfully");
        }

        [HttpGet("GetComplianceTypes")]
        public IActionResult GetComplianceTypes()
        {
            // Get all compliance types
            var complianceTypes = _context.ComplianceTypes?.ToList();

            return Ok(new { data = complianceTypes, message = "Reached backend of getting compliance types" });
        }


        [HttpPost("CreateSchool")]
        public IActionResult CreateSchoolMethod(NewSchool DataFromUser)
        {
            // // Access the JSON DataFromUser from the request
            System.Console.WriteLine("Reached backend of adding new school");
            System.Console.WriteLine($"Dean: {DataFromUser.Dean}");
            System.Console.WriteLine($"Name: {DataFromUser.Name}");
            System.Console.WriteLine($"Address: {DataFromUser.Address}");
            System.Console.WriteLine($"City: {DataFromUser.City}");
            System.Console.WriteLine($"State: {DataFromUser.State}");
            System.Console.WriteLine($"ZipCode: {DataFromUser.ZipCode}");
            System.Console.WriteLine($"OnsiteData: {DataFromUser.OnSiteDate}");


            // // add to database
            _context.NewSchools?.Add(DataFromUser);
            _context.SaveChanges();

            var schools = _context.NewSchools?.ToList();

            return Ok(new { schoolData = schools, message = "Reached backend of adding new school" });
        }

        [HttpGet("/GetAllSchools")]
        public IActionResult GetAllSchoolsMethod()
        {
            System.Console.WriteLine("Reached backend of getting schools");
            // Get all schools
            var schools = _context.NewSchools?.ToList();

            return Ok(new { schoolData = schools, message = "Reached backend of getting schools" });
        }


        [HttpGet("GetStudentInformation")]
        public IActionResult GetStudentInformation()
        {

            // Get the user ID from the session
            var userIdInSession = HttpContext.Session.GetInt32("UserId");
            // Get all students
            var student = _context.Users?.Where(u => u.UserId == userIdInSession).ToList();

            return Ok(new { studentData = student, message = "Reached backend of getting student" });
        }


        // http post to delete DeleteUser
        [HttpPost("DeleteUser")]
        public IActionResult DeleteUserMethod(User IdToDelete)
        {
            System.Console.WriteLine("Reached backend of deleting user", IdToDelete);
            // check if the data is null
            if (IdToDelete == null)
            {
                System.Console.WriteLine("User ID is null");
                return BadRequest(new { message = "User ID is null" });
            }

            // check if the data is empty
            if (IdToDelete.UserId == 0)
            {
                System.Console.WriteLine("User ID is empty");
                return BadRequest(new { message = "User ID is empty" });
            }

            // remove the user from the database
            _context.Users?.Remove(IdToDelete);
            _context.SaveChanges();

            System.Console.WriteLine("Id to delete user form Db", IdToDelete);



            // Perform delete operation using the userId

            return Ok(new { UserDeleted = IdToDelete, message = "Reached backend of deleting user" });
        }

        [HttpPost("CreateSemester")]
        public IActionResult CreateSemesterMethod(Semester DataFromUser)
        {
            // Access the JSON DataFromUser from the request
            System.Console.WriteLine("Reached backend of adding new semester");
            System.Console.WriteLine($"Title: {DataFromUser.Title}");
            DataFromUser.Tracker = "Current Semester";

            // add to database
            _context.Semesters?.Add(DataFromUser);
            _context.SaveChanges();

            return Ok(new { SemesterData = DataFromUser, message = "Successfully added new semester" });
        }

        [HttpGet("GetSemesters")]
        public IActionResult GetSemestersMethod()
        {
            // Get all semesters
            var semesters = _context.Semesters?.ToList();

            return Ok(new { SemesterData = semesters, message = "Successfully retrieved all semesters" });
        }


        [HttpPost("RemoveSemester")]
        public IActionResult RemoveSemesterMethod(Semester DataFromUser)
        {
            // Access the JSON DataFromUser from the request
            System.Console.WriteLine("Reached backend of removing semester");
            System.Console.WriteLine($"SemesterId===============>: {DataFromUser.Tracker}");

            // get the semester from the database
            Semester? semester = _context.Semesters?.FirstOrDefault(s => s.SemesterId == DataFromUser.SemesterId);

            // check if the semester is null
            if (semester == null)
            {
                return NotFound(new { message = "Semester not found" });
            }

            // update the semester tracker
            semester.Tracker = DataFromUser.Tracker;
            _context.SaveChanges();



            return Ok(new { SemesterData = DataFromUser, message = "Successfully removed semester" });
        }

        [HttpDelete("DeleteSchool")]
        public IActionResult DeleteSchoolMethod([FromBody] NewSchool DataFromUser)
        {
            // Access the JSON DataFromUser from the request
            System.Console.WriteLine("Reached backend of deleting school");
            System.Console.WriteLine($"SchoolId-------------->: {DataFromUser.SchoolId}");
            System.Console.WriteLine($"Password-------------->: {DataFromUser.Name}");

            System.Console.WriteLine($"SchoolId-------------->: {DataFromUser.SchoolId}");

            var password = _config["DeleteSchoolPassword:Password"];


            // check if DataFromUser.Name is = "delete123"
            if (DataFromUser.Name != password)
            {
                return BadRequest(new { message = "Invalid password" });
            }



            //check if the data is  empty
            if (DataFromUser.SchoolId == 0)
            {
                return BadRequest(new { message = "School ID is empty" });
            }




            // get the school from the database
            NewSchool? school = _context.NewSchools?.FirstOrDefault(s => s.SchoolId == DataFromUser.SchoolId);

            // check if the school is null
            if (school == null)
            {
                return NotFound(new { message = "School not found" });
            }

            // remove the school from the database
            _context.NewSchools?.Remove(school);
            _context.SaveChanges();

            return Ok(new { SchoolData = DataFromUser, message = "Successfully removed school" });
        }



    }
}