
using Microsoft.AspNetCore.Mvc;



namespace DrugFreePortal.Models
{
    public class AdminController : Controller
    {


        private MyContext _context;
        public IWebHostEnvironment _webHostEnvironment;

        public AdminController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpPost("DeleteSchool")]
        public IActionResult DeleteSchoolMethod(NewSchool DataFromUser)
        {
            // Access the JSON DataFromUser from the request
            System.Console.WriteLine("Reached backend of deleting school");
            System.Console.WriteLine($"SchoolId-------------->: {DataFromUser.SchoolId}");

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