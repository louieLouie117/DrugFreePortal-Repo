
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


    }
}