using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DrugFreePortal.Models;
using Stripe;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static DrugFreePortal.Controllers.PasswordResetController;
using Microsoft.AspNetCore.Identity;




namespace DrugFreePortal
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];

            services.AddDbContext<MyContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"], new MySqlServerVersion(new Version(8, 0, 26))));
            services.AddSession();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<MyContext>()
            .AddDefaultTokenProviders();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                      .AddJwtBearer(options =>
                      {
                          var secretKey = Configuration["Jwt:SecretKey"];
                          if (secretKey == null)
                          {
                              throw new Exception("Jwt:SecretKey is not set in the configuration.");
                          }

                          options.TokenValidationParameters = new TokenValidationParameters
                          {
                              ValidateIssuer = true,
                              ValidateAudience = true,
                              ValidateLifetime = true,
                              ValidateIssuerSigningKey = true,
                              ValidIssuer = Configuration["Jwt:Issuer"],
                              ValidAudience = Configuration["Jwt:Audience"],
                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                          };

                      });

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}