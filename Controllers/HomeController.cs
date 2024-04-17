
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IFormFile = Microsoft.AspNetCore.Http.IFormFile;
using Microsoft.AspNetCore.Hosting;



namespace DrugFreePortal.Models
{
    public class HomeController : Controller
    {


        private MyContext _context;
        public IWebHostEnvironment _webHostEnvironment;

        public HomeController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("")] // This is the route for the index page
        public IActionResult index()
        {
            return View("index");
        }



        [HttpGet("/dashboard")] // This is the route for the index page
        public IActionResult dashboard()
        {

            // get user id from session
            int? UserIdInSession = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine($"----------------UserId in session Home:GetUsers => {UserIdInSession}");

            string accountType = HttpContext.Session.GetString("AccountType") ?? string.Empty;
            System.Console.WriteLine($"-------------------AccountType in session Home:Dashboard => {accountType}");

            // if user is not logged in 
            if (UserIdInSession == null)
            {
                return RedirectToAction("index");
            }

            User? UserIndb = _context.Users?
                .FirstOrDefault(u => u.UserId == UserIdInSession);



            ViewBag.ToDisplay = UserIndb;
            ViewBag.allUserLogs = _context.Users
                ?.Where(ul => ul.UserId == UserIdInSession)
                .ToList();

            // List<User> ActiveUser = _context.Users
            //                 ?.Where(ul => ul.UserId == UserIdInSession)
            //                 .ToList() ?? new List<User>();

            DashboardWrapper wMod = new DashboardWrapper();
            wMod.User = UserIndb;



            return View("dashboard", wMod);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            System.Console.WriteLine("Reached backend of get users");

            // lambda expression to get all users with null or empty check net8.0 new feature
            List<User> AllUsers = _context.Users?.Select(u => u).ToList() ?? new List<User>();

            return Ok(new { Status = "Success", UsersList = AllUsers });
        }

        // file upload

        [HttpPost("UploadSingleFile")]
        public async Task<IActionResult> UploadSingleFileMethod(IFormFile file, UploadFile fromUser)
        {
            System.Console.WriteLine("Reached backend of UploadSingleFileMethod-----");

            if (file.Length > 5 * 1024 * 1024) // 5MB
            {
                return BadRequest("File is too large");
            }


            string encodedFileName = System.Web.HttpUtility.UrlEncode(file.FileName);

            // Generate a timestamp
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            System.Console.WriteLine("Time Stamp => ", timeStamp);

            // combine the timestamp with the file name
            string fileName = $"{timeStamp}_{encodedFileName}";

            // Combine the current directory path with the destination path for the uploaded file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "uploads", fileName);
            // file name for databse to render on the frontend
            var shortFilePath = Path.Combine("img", "uploads", fileName);


            // Create a new file stream and open the file in create mode
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Copy the contents of the uploaded file to the file stream asynchronously
                await file.CopyToAsync(stream);
            }

            int? UserIdInSession = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine($"----------------UserId in session Home:UloadSingleFile => {UserIdInSession}");


            // get and set defult file infomation
            fromUser.UserId = UserIdInSession ?? 0;
            fromUser.FileName = fileName;
            fromUser.FilePath = shortFilePath;
            fromUser.FileType = file.ContentType;
            fromUser.FileSize = file.Length;

            // Add the file information to the database
            _context.Add(fromUser);
            _context.SaveChanges();

            // Create a JSON object with the file information
            var fileInfo = new
            {
                UserId = fromUser.UserId,
                FileName = fromUser.FileName,
                FilePath = fromUser.FilePath,
                FileType = fromUser.FileType,
                FileSize = fromUser.FileSize
            };



            // Return the file path as a response
            return Ok(new { FileInfo = fileInfo });
        }

        [HttpGet("getStudentFiles")]
        public IActionResult getStudentFilesMethod()
        {
            System.Console.WriteLine("Reached backend of get files");

            // Get session user id
            int? UserIdInSession = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine($"----------------UserId in session Home:GetUsers => {UserIdInSession}");

            // lambda expression to get all files by userid from session with null or empty check net8.0 new feature
            List<UploadFile> StudentFiles = _context.UploadFiles?
            .Where(f => f.UserId == UserIdInSession)
            .ToList() ?? new List<UploadFile>();

            return Ok(new { Status = "Success", StudentFiles = StudentFiles });
        }

        [HttpPost("CheckIn")]
        public IActionResult CheckInMethod(Queue DataFromUser)
        {
            System.Console.WriteLine("Reached backend of check in");
            // Get session user id
            int? UserIdInSession = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine($"----------------UserId in session Home:CheckIn => {UserIdInSession}");


            // Check if the user id in session is in the database
            bool isUserInDatabase = _context.Users?.Any(u => u.UserId == UserIdInSession) ?? false;
            System.Console.WriteLine($"----------------isUserInDatabase in Home:CheckIn => {isUserInDatabase}");

            // if user is in data base return you are already in queue
            if (isUserInDatabase)
            {
                return Ok(new { Status = "InQueue", Message = "You are already in the queue" });
            }


            DataFromUser.StudentUserId = UserIdInSession?.ToString() ?? "0";

            // Add the file information to the database
            _context.Add(DataFromUser);
            _context.SaveChanges();







            // Return the updated user data
            return Ok(new { QueueData = DataFromUser, Status = "Success", Message = "Checked in successfully" });


        }


    }
}