using OnlineQuizSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineQuizSystem.Models;
using Microsoft.EntityFrameworkCore;
using OnlineQuizSystem.Filters;

namespace OnlineQuizSystem
{
    public class Program
    {
        public static void Main(string[] args)
         {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options => options.Filters.Add(new GlobalExceptionHandler()));
            builder.Services.AddSession();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<SessionManager>();

            string connection = builder.Configuration.GetConnectionString("default");
            builder.Services.AddDbContext<OnlineQuizSystemContext>(options => options.UseMySql(connection, ServerVersion.Parse("8.0.29-mysql")));
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //options.SlidingExpiration = false;
                options.LoginPath = "/Admin/Login";
                options.AccessDeniedPath = "/Home/Error";   
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            app.Run();

        }
    }
}