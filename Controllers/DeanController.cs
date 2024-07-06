

using Microsoft.AspNetCore.Mvc;


namespace DrugFreePortal.Models
{
    public class DeanController : Controller
    {


        private MyContext _context;
        public IWebHostEnvironment _webHostEnvironment;

        public DeanController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet("GetRecords")]
        public IActionResult GetRecords(Record record, Semester semester, User user)
        {
            System.Console.WriteLine("You reached backend for GetRecords");

            //get SchoolIdInSession from session
            int? SchoolIdInSession = HttpContext.Session.GetInt32("SchoolIdInSession");
            //get all users
            List<User> allUsers = _context.Users?
            .Where(u => u.AccountType == AccountType.Student && u.SchoolId == SchoolIdInSession)
            .ToList() ?? new List<User>();

            // get semester that is = "Current Semester"
            Semester CurrentSemester = _context.Semesters
                ?.FirstOrDefault(s => s.Tracker == "Current Semester") ?? new Semester { Title = "Default Title", Tracker = "Default Tracker" };
            //get all records
            List<Record> allRecords = _context.Records?
                .Where(r => r.SemesterId == CurrentSemester.SemesterId)
                .ToList() ?? new List<Record>();

            //combine users and records
            var usersWithRecords = allUsers.Select(user => new
            {
                User = user,
                Records = allRecords.Where(record => record.UserId == user.UserId).ToList()
            }).ToList();

            return Ok(new
            {
                UserWithRecordsData = usersWithRecords,
                // UsersData = allUsers,
                // SemesterData = CurrentSemester,
                // AllRecordsDB = allRecords,
                message = "You reached backend for GetRecords"
            });
        }

        [HttpPost("GetDeanStudentUploads")]
        public IActionResult GetDeanStudentUploads(UploadFile uploadFile)
        {
            System.Console.WriteLine($"You reached backend for GetDeanStudentUploads {uploadFile.UserId}");
            // get all upload files filter by the user Id
            List<UploadFile> allUploadFiles = _context.UploadFiles?
                .Where(u => u.UserId == uploadFile.UserId)
                .ToList() ?? new List<UploadFile>();


            return Ok(new
            {
                UploadFilesData = allUploadFiles,
                message = "You reached backend for GetRecords"
            });
        }

        [HttpGet("GetSchoolEvaluators")]
        public IActionResult GetSchoolEvaluators()
        {
            System.Console.WriteLine("You reached backend for GetSchoolEvaluators");

            //get SchoolIdInSession from session
            int? SchoolIdInSession = HttpContext.Session.GetInt32("SchoolIdInSession");
            //get all users
            List<User> allUsers = _context.Users?
            .Where(u => u.AccountType == AccountType.Evaluator && u.SchoolId == SchoolIdInSession)
            .ToList() ?? new List<User>();

            return Ok(new
            {
                SchoolEvaluatorsData = allUsers,
                message = "You reached backend for GetSchoolEvaluators"
            });
        }


    }
}