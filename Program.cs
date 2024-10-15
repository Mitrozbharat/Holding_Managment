using HoardingManagement.Interface;
using HoardingManagement.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Hoarding_managment;

namespace HoardingManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Retrieve the connection string from the configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContextPool<db_hoarding_managementContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                ));

            // Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Prevent client-side script from accessing the cookie
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });

            // Register repositories
            builder.Services.AddScoped<IAuth, AuthRepository>();
            builder.Services.AddScoped<IDashboard, DashboardRepository>();
            builder.Services.AddScoped<ICustomer, CustomerRepository>();
            builder.Services.AddScoped<IVendor, VendorRepository>();
            builder.Services.AddScoped<IQuotation, QuotationRepository>();
            builder.Services.AddScoped<IOngoingCampain, OngoingCampainRepository>();
            builder.Services.AddScoped<AutocompleteService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable session before authentication
            app.UseSession();

            app.UseAuthorization();

            // Define the default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
