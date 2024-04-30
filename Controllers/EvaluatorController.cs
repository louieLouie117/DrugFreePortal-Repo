using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IFormFile = Microsoft.AspNetCore.Http.IFormFile;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;




namespace DrugFreePortal.Models
{
    public class EvaluatorController : Controller
    {


        private MyContext _context;
        public IWebHostEnvironment _webHostEnvironment;

        public EvaluatorController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet("GetQueue")]
        public IActionResult GetQueueMethod()
        {
            System.Console.WriteLine("Reached backend of get queue");


            // lambda expression to get all data from queue with null or empty check net8.0 new feature
            List<Queue> Queue = _context.Queues?.Select(q => q).ToList() ?? new List<Queue>();


            // Check if any data has been changed since the last call

            return Ok(new { Status = "Success", QueueData = Queue });
        }

        [HttpPost("ChangeQueueStatus")]
        public IActionResult ChangeQueueStatusMethod(Queue DataFromUser)
        {
            System.Console.WriteLine("Reached backend of change queue status");

            // Get the user from the database
            Queue? UserInQueue = _context.Queues?.FirstOrDefault(u => u.QueueId == DataFromUser.QueueId);


            var UserIdInProgress = HttpContext.Session.GetInt32("UserIdInProgress");
            System.Console.WriteLine($"----------------UserIdInProgress => {UserIdInProgress}");

            if (UserIdInProgress == null || UserIdInProgress == 0)
            {
                HttpContext.Session.SetInt32("UserIdInProgress", DataFromUser.StudentUserId);
                // Update database if UserInQueue is not null
                if (UserInQueue != null)
                {
                    UserInQueue.Status = DataFromUser.Status;
                    _context.SaveChanges();
                }
            }
            else
            {
                return Ok(new { Status = "In Progress Error", Message = "There is already a user in progress. You must return student to queue before working on another student." });
            }


            return Ok(new { Status = "Success", Message = "Queue status updated successfully" });
        }

        [HttpPost("ReturnQueueStatus")]
        public IActionResult ReturnQueueStatusMethod()
        {
            System.Console.WriteLine("Reached backend of return to queue");

            // Get the user in progress from the session
            int? UserIdInProgress = HttpContext.Session.GetInt32("UserIdInProgress");
            System.Console.WriteLine($"----------------UserIdInProgress => {UserIdInProgress}");

            // Get the user from the database
            Queue? UserInQueue = _context.Queues?.FirstOrDefault(u => u.StudentUserId == UserIdInProgress);

            // Change status to start if UserInQueue is not null
            if (UserInQueue != null)
            {
                UserInQueue.Status = "Start";
                HttpContext.Session.Remove("UserIdInProgress");
                _context.SaveChanges();
            }


            return Ok(new { Status = "Success", Message = "Returned to queue successfully" });
        }


        [HttpGet("GetStudentInProgress")]
        public IActionResult GetStudentInProgressMethod()
        {
            System.Console.WriteLine("Reached backend of get student in progress");

            // Get the user in progress from the session
            int? UserIdInProgress = HttpContext.Session.GetInt32("UserIdInProgress");
            System.Console.WriteLine($"----------------UserIdInProgress => {UserIdInProgress}");

            if (UserIdInProgress == null || UserIdInProgress == 0)
            {
                return Ok(new { Status = "No User In Progress", Message = "There is no user in progress." });
            }

            // Get the user from the database
            User? UserInQueue = _context.Users?.FirstOrDefault(u => u.UserId == UserIdInProgress);

            return Ok(new { Status = "Success", UserInQueueData = UserInQueue });
        }





        [HttpPost("DeleteFromQueue")]
        public IActionResult DeleteFromQueueMethod(Queue DeleteID)
        {
            System.Console.WriteLine("DeleteFromQueueMethod reached");

            Queue? queue = _context.Queues?.FirstOrDefault(q => q.QueueId == DeleteID.QueueId);

            if (queue != null)
            {
                _context.Queues?.Remove(queue);
                _context.SaveChanges();
                return Ok(new { message = "You have reached the backend of DeleteFromQueue" });
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("RenderSchoolCompliance")]
        public IActionResult RenderSchoolComplianceMethod(ComplianceType DataFromUser)
        {
            System.Console.WriteLine($"=========>Reached backend of render school compliance {DataFromUser.IdFromSchool}");

            int FilterById = DataFromUser.IdFromSchool;
            // Get school compliance data that have the same id
            List<ComplianceType> ComplianceList = _context.ComplianceTypes?.Where(c => c.IdFromSchool == FilterById).ToList() ?? new List<ComplianceType>();

            return Ok(new { ComplianceData = ComplianceList, message = "You have reached the backend of RenderSchoolCompliance" });
        }


        [HttpPost("GetUserUploads")]
        public IActionResult GetUserUploadsMethod(UploadFile DataFromUser)
        {
            System.Console.WriteLine("Reached backend of get user uploads");

            // Get the user from the database
            List<UploadFile> Uploads = _context.UploadFiles?.Where(u => u.UserId == DataFromUser.UserId).ToList() ?? new List<UploadFile>();


            return Ok(new { UploadData = Uploads, message = "You have reached the backend of GetUserUploads" });
        }

    }
}