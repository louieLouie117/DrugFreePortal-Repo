

using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace DrugFreePortal.Models
{
    public class PaymentController : Controller
    {


        private MyContext _context;
        public readonly IConfiguration _config;



        public PaymentController(MyContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;

        }



    }
}