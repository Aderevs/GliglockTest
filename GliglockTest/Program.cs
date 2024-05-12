using AutoMapper;
using GliglockTest.appCore;
using GliglockTest.DbLogic;
using System.Reflection;

namespace GliglockTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddCookieTempDataProvider();
            builder.Services.AddScoped(provider =>
            {
                var connectionString = "Server=localhost\\SQLEXPRESS;Database=GliglockTestDB;Trusted_Connection=True;TrustServerCertificate=True;";
                return new TestsDbContext(connectionString);
            });
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ModelMapper>();
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
