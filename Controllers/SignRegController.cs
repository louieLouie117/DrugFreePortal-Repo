
using Microsoft.AspNetCore.Mvc;


namespace DrugFreePortal.Models
{
    public class SignRegController : Controller
    {

        private MyContext _context;
        public SignRegController(MyContext context)
        {
            _context = context;
        }




    }
}