
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

            return Ok(new { data = DataFromUser, message = "Reached backend of adding new compliance" });
        }


    }
}