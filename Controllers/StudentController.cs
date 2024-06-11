using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DrugFreePortal.Models
{
    public class StudentController : Controller
    {
        // Add your controller actions here

        // Example action method

        private MyContext _context;
        public IWebHostEnvironment _webHostEnvironment;

        public StudentController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetStudentSchoolCompliance")]

        public IActionResult GetStudentSchoolCompliance()
        {
            // get user id from session
            int? SchoolIdInSession = HttpContext.Session?.GetInt32("SchoolIdInSession");
            System.Console.WriteLine($"----------------UserId in session Home:GetUsers => {SchoolIdInSession}");

            // if user is not logged in 
            if (SchoolIdInSession == null)
            {
                System.Console.WriteLine("User not logged in");
            }

            var ComplianceList = _context?.ComplianceTypes?
                .Where(u => u.IdFromSchool == SchoolIdInSession)
                .ToList();

            return Ok(new { ComplianceListData = ComplianceList, Message = "Reached backend of compliance" });
        }

        [HttpGet("studentResults")]
        public IActionResult studentResults()
        {


            // get user from session
            int? UserIdInSession = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine($"----------------UserId in session StudentController:GetUsers => {UserIdInSession}");


            // get records filter by user in session
            var recordsList = _context?.Records?
                .Where(u => u.UserId == UserIdInSession)
                .ToList();


            // get list of all semesters
            var semestersList = _context?.Semesters?
                .ToList();

            var recordsListWithSemesters = (// Create a new list of objects
                from semester in semestersList // Iterate through each semester in the semestersList
                let recordsInSemester = recordsList?.Where(r => r.SemesterId == semester.SemesterId)?.ToList() ?? new List<Record>() // Filter the recordsList to get only the records that belong to the current semester
                select new // Create a new anonymous object with the semester and the filtered records
                {
                    Semester = semester, // Store the current semester
                    Records = recordsInSemester.ToList() // Store the filtered records in a list
                }).ToList(); // Convert the result into a list


            System.Console.WriteLine("Reached backend of studentResults");
            return Ok(new { Data = recordsListWithSemesters, Message = "Reached backend of studentResults" });


        }
    }
}