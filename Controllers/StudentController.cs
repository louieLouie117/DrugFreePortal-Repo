using Microsoft.AspNetCore.Mvc;

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
            int? SchoolIdInSession = HttpContext.Session.GetInt32("SchoolIdInSession");
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
    }
}