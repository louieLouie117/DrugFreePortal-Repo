using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Stripe;

using System.Text;

using MimeKit;
using MimeKit.Text;
using MailKit.Security;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

using Microsoft.Extensions.Options;
using DrugFreePortal.Models;

// using Microsoft.AspNetCore.WebUtilities;




namespace DrugFreePortal.Controllers
{
    public class PasswordResetController : Controller
    {
        private MyContext _context;
        public readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        public readonly SignInManager<IdentityUser> _signInManager;
        public readonly ILogger<AccountController> _logger;


        private readonly JwtSettings _jwtSettings;



        public PasswordResetController(MyContext context, IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _config = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        public class JwtSettings
        {
            public required string SecretKey { get; set; }
        }


        [HttpPost("ForgotPasswordMailKit")]
        public IActionResult ForgotPasswordMailKit(User FromForm)
        {
            System.Console.WriteLine("reach the backend of MailKit");
            // Check if the email exists in the database
            User userEmail = _context.Users.FirstOrDefault(u => u.Email == FromForm.Email);
            if (userEmail == null)
            {
                // Email not found in the database
                return Json(new { StatusCode = "Not a valid email!" });
            }
            System.Console.WriteLine($"user email is {userEmail.Email}");

            // Generate a password reset simple token---------------------------------------
            // var token = userEmail.Email;
            // var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // Generate the link to reset the password with JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userEmail.Email),
                    new Claim("CustomClaimType", "Hello Claim") // 
                }),
                Expires = DateTime.UtcNow.AddMinutes(15), // Token will expire in 15 minutes
                // Expires = DateTime.UtcNow.AddDays(30), // for development purposes
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            // Generate the link to reset the password
            var callbackUrl = Url.Action(
                "GenerateURL",
                "PasswordReset",
                new { userEmail.Email, token = encodedToken },
                protocol: HttpContext.Request.Scheme);
            Console.WriteLine($"call back URL {callbackUrl}");

            // Create the email for password reset
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:Email"]));
            email.To.Add(MailboxAddress.Parse(FromForm.Email));
            email.Subject = "Reset your password.";
            email.Body = new TextPart(TextFormat.Html) { Text = $"<div style='background-Color: whites'> <h1 style=' color: black'>Reset Password</h1> <p>We have received a request to reset your password. If you did not make this request, please contact us.</p> <p>To reset your password, please click on the following <a href = '{callbackUrl}'>link.</a> </p><p>Please note that this link will expire in 15 minutes. If you need more time, please request another password reset.</p> <p>If you have any questions or concerns, please contact our support team</p> <p>Thank you.</p> <p>Journal Pocket</p></div>" };

            System.Console.WriteLine($"email is {email}");

            // Send email
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            // smtp.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            // smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
            smtp.Connect(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["EmailSettings:Email"], _config["EmailSettings:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);



            Console.WriteLine($"email was sent!!");

            return Json(new { StatusCode = "Email sent" });
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> GenerateURLAsync(string email, string token, string newPassword)
        {

            System.Console.WriteLine("REset Password method was called!!!!!");
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // User not found
                return Json(new { StatusCode = "User not found" });
            }

            // Reset the user's password
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                // Password reset successful
                return Json(new { StatusCode = "Password reset successful" });
            }
            else
            {
                // Password reset failed
                return Json(new { StatusCode = "Password reset failed", Errors = result.Errors });
            }
        }




        [HttpPost("NewPasswordMethod")]
        // create a method with jsons IActionResult
        public JsonResult NewPasswordMethod(User FromForm)
        {
            // get user from db by email using FromForm.Email
            User user = _context.Users.SingleOrDefault(u => u.Email == FromForm.Email);

            System.Console.WriteLine($"user new password: {FromForm.Password}");


            System.Console.WriteLine($"user is {user.UserId}");
            System.Console.WriteLine($"user is {user.Email}");


            // update user db with new password using FromForm.NewPassword
            // hash the password
            PasswordHasher<User> Hasher = new PasswordHasher<User>();

            var newPasswordHashed = Hasher.HashPassword(FromForm, FromForm.Password);
            user.Password = newPasswordHashed;

            // // save changes to db
            _context.SaveChanges();

            System.Console.WriteLine("New password was saved to db");
            return Json(new { StatusCode = "Password has been updated." });
        }




    }


}