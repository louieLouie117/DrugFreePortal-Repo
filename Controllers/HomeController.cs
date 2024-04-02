
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
        [HttpPost("ModelUplder")]

        public async Task<IActionResult> ModelUplder(List<IFormFile> files, UploadFile fForm)
        {
            System.Console.WriteLine("Reached backend of ModelUplder");

            if (files.Count == 0)
            {
                return BadRequest("No files received");
            }
            long size = files.Sum(f => f.Length);


            var filePaths = new List<string>();
            System.Console.WriteLine(new { count = files.Count, size, filePaths });

            System.Console.WriteLine($"file paths---- {filePaths.Count}");
            foreach (var formFile in files)
            {

                System.Console.WriteLine("file is greater than 0");

                System.Console.WriteLine("loop is running");
                if (formFile.Length > 0)
                {
                    System.Console.WriteLine("file is greater than 0");
                    // TimeStamp
                    string timeStampMonth = DateTime.Now.Month.ToString("00");
                    string timeStampDay = DateTime.Now.Day.ToString("00");
                    string timeStampHour = DateTime.Now.Hour.ToString("00");
                    string timeStampMinutes = DateTime.Now.Minute.ToString("00");
                    string timeStampSeconds = DateTime.Now.Second.ToString("00");

                    string timeStamp = $"{timeStampMonth}{timeStampDay}{timeStampHour}{timeStampMinutes}{timeStampSeconds}";

                    //Place to save file
                    string folderPath = $"img/uploads/";
                    // folderPath += timeStamp + formFile.FileName;

                    System.Console.WriteLine($"******folder path name {folderPath}");
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
                    // Assign name to be saved to the db
                    string newName = $"{timeStamp}{formFile.FileName}";
                    fForm.FilePath = newName;


                    filePaths.Add(serverFolder);
                    using (var stream = new FileStream(serverFolder, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);

                    }
                    // await formFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));



                }
            }

            fForm.UserId = 1;

            // add to data base
            // _context.Add(fForm);
            // _context.SaveChanges();

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            // return RedirectToAction("index");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error.ErrorMessage);
                }
                return BadRequest("model state is invalid");
            }
            return Ok(new { Status = "Success reach the backend" });

        }

        [HttpPost("upload")]
        public async Task<IActionResult> ServerFileUploaderNotworking(List<IFormFile> files, [FromBody] UploadFile DataFromUser)
        {
            System.Console.WriteLine($"Reached backend of file upload with Ai");
            System.Console.WriteLine($"FileName {DataFromUser.FileName}");
            // set defult value for Filetype and Filepath

            DataFromUser.FileType = "Image";
            DataFromUser.FilePath = "img/uploads/" + DataFromUser.FileName;
            System.Console.WriteLine($"files data count---------------{files.Count}");
            System.Console.WriteLine($"files data Path---------------{DataFromUser.FilePath}");

            if (ModelState.IsValid)
            {
                // Process the uploaded file(s)
                foreach (var formFile in files)
                {
                    // write line item
                    System.Console.WriteLine($"FileName {formFile.FileName}");
                    System.Console.WriteLine($"ContentType {formFile.ContentType}");
                    System.Console.WriteLine($"Length {formFile.Length}");

                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "uploads", formFile.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }

                // Additional processing logic (e.g., save file paths to database, etc.)

                // Redirect to a success page or return a JSON response
                return RedirectToAction("UploadSuccess");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error.ErrorMessage);
                }
                return BadRequest("model state is invalid");
            }

            // If model state is invalid, return to the form with validation errors
            return BadRequest("model state is invalid");
        }


        [HttpPost("FileUploaderFetch")]

        public IActionResult FileUploaderFetch(UploadFile UploadData, List<IFormFile> files)
        {
            System.Console.WriteLine("Reached backend FileUploaderFetch");
            System.Console.WriteLine($"FileName {UploadData.FileName}");

            // check if files has files
            if (files.Count == 0)
            {
                return BadRequest("No files received");
            }


            return Ok(new { Status = "Success", UploadData = UploadData });


        }

        [HttpPost("UploadFileSingleFile")]
        public async Task<IActionResult> UploadFileSingleFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // return RedirectToAction("Files");
            return Ok(new { Status = "Success", path = path });
        }

        [HttpPost("UploadMultiFiles")]
        public async Task<IActionResult> UploadMultiFiles(IFormFile[] files, [FromBody] UploadFile DataFromUser)
        {
            // file count check
            System.Console.WriteLine($"files data count---------------{files.Length}");
            System.Console.WriteLine($"FileName {DataFromUser.FileName}");

            if (files.Length == 0)
            {
                return BadRequest("No files received");
            }
            foreach (var file in files)
            {
                System.Console.WriteLine($"FileName {file.FileName}");
                if (file == null || file.Length == 0)
                    return BadRequest("file not selected");

                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot\\img\\uploads",
                            Path.GetFileName(file.FileName));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Ok(new { Status = "Success", path = path });
            }


            // return RedirectToAction("Files");
            return Ok(new { Status = "Success" });


        }

        [HttpPost("UploadMultiFilesAjax")]
        public async Task<IActionResult> UploadMultiFilesAjax(List<IFormFile> files, [FromBody] UploadFile DataFromUser)
        {
            System.Console.WriteLine("**********Reached backend of UploadMultiFilesAjax");

            foreach (var file in files)
            {
                System.Console.WriteLine($"FileName {file.FileName}");
                if (file == null || file.Length == 0)
                    continue;

                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot",
                            file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            if (files.Count == 0)
            {
                return BadRequest("No files received on ajax call");
            }
            return Ok(new { message = "Files uploaded successfully." });
        }


        [HttpPost("UploadFiles")]
        public IActionResult UploadFiles(List<IFormFile> files)
        {


            System.Console.WriteLine("Reached backend of UploadFiles");
            if (files.Count == 0)
            {
                return BadRequest("No files received");
            }
            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot",
                            file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return Ok(new { message = "Files uploaded successfully." });
        }

        [HttpPost("UploadFileDefult")]
        public async Task<IActionResult> UploadFileDefult(IFormFile file)
        {
            System.Console.WriteLine("Reached backend of UploadFileDefult");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "uploads", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { filePath });
        }



    }
}