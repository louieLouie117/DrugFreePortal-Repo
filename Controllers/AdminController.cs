
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

            var complianceTypes = _context.ComplianceTypes?.ToList();

            return Ok(new { data = complianceTypes, message = "Reached backend of adding new compliance" });
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




    }
}