

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


        [HttpGet("/re")] // This is the route for the index page
        public IActionResult index()
        {
            return View("index");
        }
        [HttpPost("create-checkout-session")]
        public ActionResult Create()
        {
            System.Console.WriteLine("PaymentController: Reach the backend of stripe session");
            // var domain = "https://journalpocketapp.com/";
            var domain = "http://localhost:5043";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {

                    Price = "price_1Ow7aSKJxcOxvTTBxSNOnvyG", // this is the price for client server
                    Quantity = 1,
                  },
                },

                Mode = "subscription",

                SuccessUrl = "http://localhost:5043/order/success?session_id={CHECKOUT_SESSION_ID}",
                // SuccessUrl = "https://journalpocketapp.com/order/success?session_id={CHECKOUT_SESSION_ID}",

                CancelUrl = domain,
            };
            var service = new SessionService();
            Session session = service.Create(options);

            System.Console.WriteLine($"Customer Phone number {session.Customer}");
            // Response.Headers.Add("Location", session.Url);
            Response.Headers.Append("Location", session.Url);
            return new StatusCodeResult(303);
        }
        [HttpGet("/order/success")]
        public IActionResult OrderSuccess([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            var customerService = new CustomerService();
            Customer customer = customerService.Get(session.CustomerId);

            System.Console.WriteLine($"After Card payment Customer {customer}");
            System.Console.WriteLine($"Stripe Customer Id {customer.Name}");
            System.Console.WriteLine($"Stripe Customer Id {customer.Id}");
            System.Console.WriteLine($"Stripe Customer email {customer.Email}");

            HttpContext.Session.SetString("StripeCustomerName", customer.Name);
            HttpContext.Session.SetString("StripeCustomerId", customer.Id);
            HttpContext.Session.SetString("StripeCustomerEmail", customer.Email);


            var stripeCustomerId = HttpContext.Session.GetString("StripeCustomerId");


            var service = new CustomerService();
            service.Get(stripeCustomerId);

            System.Console.WriteLine($"Data After card {service}");

            // return Content($"<html><body><h1>Thanks for your order, {customer.Id} {customer.Name} {customer.Email} {customer.Phone}!</h1></body></html>");

            // return RedirectToAction("Index", "Register", viewModel);
            return Redirect("/register");


        }




    }
}