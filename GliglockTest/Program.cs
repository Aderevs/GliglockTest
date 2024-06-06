using AutoMapper;
using GliglockTest.appCore;
using GliglockTest.appCore.Interfaces;
using GliglockTest.DbLogic;
using GliglockTest.DbLogic.Repositories;
using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

namespace GliglockTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(provider =>
            {
                var connectionString = builder.Configuration["ConnectionString"];
                return new TestsDbContext(connectionString);
            });
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ModelProfile>();
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/account/SignIn";
               });
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ITestsRepository, TestsRepository>();
            builder.Services.AddScoped<ITeachersRepository, TeachersRepository>();
            builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
            builder.Services.AddScoped<IPassedTestsRepository, PassedTestsRepository>();

            builder.Services.AddScoped<StudentTestTaker>();
            builder.Services.AddScoped<TeacherTestCreator>();
            builder.Services.AddScoped<ICacheProvider, CacheProvider>();

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
