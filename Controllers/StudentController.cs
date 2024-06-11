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

            // combine records and semesters
            var recordsListWithSemesters = (
                // for each semester in semestersList
                from semester in semestersList
                    // get records in semester
                let recordsInSemester = recordsList?.Where(r => r.SemesterId == semester.SemesterId)?.ToList() ?? new List<Record>()
                select new
                {
                    // select semester and records
                    Semester = semester,
                    // select records in semester
                    Records = recordsInSemester.ToList()
                }).ToList();

            System.Console.WriteLine("Reached backend of studentResults");
            return Ok(new { Data = recordsListWithSemesters, Message = "Reached backend of studentResults" });


        }
    }
}