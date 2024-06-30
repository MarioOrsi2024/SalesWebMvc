using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services;

namespace SalesWebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            string connection = "";
            connection = Configuration["ConnectionStrings:SalesWebMvcContext"];

            services.AddDbContext<SalesWebMvcContext>(options =>
                                        options.UseMySql(connection, ServerVersion.AutoDetect(connection), builder =>
                                            builder.MigrationsAssembly("SalesWebMvc")));

            // Add services to the container.
            services.AddControllersWithViews();

            services.AddScoped<SeedingService>();
            services.AddScoped<SellerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedingService seedingService) {
            var builder = WebApplication.CreateBuilder();

            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                seedingService.Seed();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
