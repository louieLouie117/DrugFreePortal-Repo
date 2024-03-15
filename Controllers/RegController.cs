
using Microsoft.AspNetCore.Mvc;


namespace DrugFreePortal.Models
{
    public class RegController : Controller
    {

        private MyContext _context;
        public RegController(MyContext context)
        {
            _context = context;
        }




    }
}