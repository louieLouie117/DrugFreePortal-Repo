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

        [HttpPost("DeleteFromQueue")]
        public IActionResult DeleteFromQueueMethod(Queue DeleteID)
        {

            System.Console.WriteLine("DeleteFromQueueMethod reached");
            // Queue queue = _context.Queues.FirstOrDefault(q => q.QueueId == DeleteID.QueueId);

            // _context.Queues.Remove(queue);
            // _context.SaveChanges();
            return Ok(new { message = "You have reached the backend of DeleteFromQueue" });
        }




    }
}