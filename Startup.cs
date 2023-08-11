using ECommerceWebApplication.Data.Cart;
using ECommerceWebApplication.Data.Services;
using ECommerceWebApplication.Data;
using ECommerceWebApplication.Models;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebApplication
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
            //DbContext configuration
            _ = services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            //Services configuration
            _ = services.AddScoped<IActorsService, ActorsService>();
            _ = services.AddScoped<IProducersService, ProducersService>();
            _ = services.AddScoped<ICinemasService, CinemasService>();
            _ = services.AddScoped<IMoviesService, MoviesService>();
            _ = services.AddScoped<IOrdersService, OrdersService>();

            _ = services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            _ = services.AddScoped(ShoppingCart.GetShoppingCart);

            //Authentication and authorization
            _ = services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            _ = services.AddMemoryCache();
            _ = services.AddSession();
            _ = services.AddAuthentication(options =>
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            _ = services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                _ = app.UseHsts();
            }

            _ = app.UseHttpsRedirection();
            _ = app.UseStaticFiles();

            _ = app.UseRouting();
            _ = app.UseSession();

            //Authentication & Authorization
            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            //app.UseAuthorization();

            //route mapping
            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllerRoute(
                    "default",
                    "{controller=Movies}/{action=Index}/{id?}");
            });

            //Seed database
            AppDbInitializer.Seed(app);
            AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();
        }
    }
}