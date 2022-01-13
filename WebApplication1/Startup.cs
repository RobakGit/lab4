using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<WebApplication1Context>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WebApplication1Context")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.CreateScope())
                {

                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<WebApplication1Context>();

                    dbContext.Database.EnsureCreated();
                    if (!dbContext.Class.Any())
                    {
                        Class classData1 = new() { Name = "Jan", Surrname = "Kowalski", Group = "lab_6" };
                        Class classData2 = new() { Name = "Mariusz", Surrname = "Mazowiecki", Group = "lab_3" };
                        Class classData3 = new() { Name = "Karolina", Surrname = "Nowak", Group = "lab_2" };
                        Class classData4 = new() { Name = "Wojciech", Surrname = "Kowalczyk", Group = "lab_5" };

                        var classData = new Class[] { classData1, classData2, classData3, classData4 };

                        foreach(Class item in classData)
                        {
                            dbContext.Add(item);
                        }
                        dbContext.SaveChanges();
                    }
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
