using GliglockTest.appCore;
using GliglockTest.appCore.Account;
using GliglockTest.DbLogic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GliglockTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //.AddCookieTempDataProvider();
            builder.Services.AddSingleton(provider =>
            {
                var connectionString = builder.Configuration["ConnectionStrings"];//"Server=localhost\\SQLEXPRESS;Database=GliglockTestDB;Trusted_Connection=True;TrustServerCertificate=True;";
                return new TestsDbContext(connectionString);
            });

            builder.Services.AddIdentity<BaseUser, IdentityRole>()
                    .AddEntityFrameworkStores<TestsDbContext>()
                    .AddDefaultTokenProviders();
            builder.Services.AddScoped<SignInManager<BaseUser>>();


            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ModelProfile>();
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/account/SignUp";
               })
               .AddGoogle(options =>
               {
                   options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                   options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
               });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
