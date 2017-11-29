using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStore.Auth;
using MusicStore.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MusicStore
{



    public class Startup
    {

        private IConfigurationRoot _configurationRoot;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _configurationRoot = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json")
                .Build();
        }
         


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.User.RequireUniqueEmail = true;

                })
                .AddEntityFrameworkStores<AppDbContext>();


            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IAlbumRepository, AlbumRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IAlbumReviewRepository, AlbumReviewRepository>();
            services.AddScoped<IAuthorizationHandler, MinimumOrderAgeRequirementHandler>();

            //specify options for the anti forgery here
            // https://github.com/aspnet/Announcements/issues/257
            // Old API
            //services.AddAntiforgery(opts => { opts.RequireSsl = true; });

            // New API
            //services.AddAntiforgery(opts => { opts.Cookie.SecurePolicy = CookieSecurePolicy.Always; });

            


            //anti forgery as global filter
            //services.AddMvc(options =>
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            services.AddMvc();

            // Google OAUTH authentication
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "270122139324-j23sp1q4t1ua5oplthdol9o143nvmi8b.apps.googleusercontent.com";
                    options.ClientSecret = "s0HbbYeWFyLsDyme1UPFMj9B";
                });

            //Claims-based
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("DeleteAlbum", policy => policy.RequireClaim("Delete Album", "Delete Album"));
                options.AddPolicy("AddAlbum", policy => policy.RequireClaim("Add Album", "Add Album"));
                options.AddPolicy("MinimumOrderAge", policy => policy.Requirements.Add(new MinimumOrderAgeRequirement(18)));
            });

            // Session state is stored in memory (which may cause scalability problems when running in web-farms)
            // for more info to use Redis or SQL Server as session store:
            // Introduction to session and application state in ASP.NET Core
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?tabs=aspnetcore2x

            services.AddMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(_configurationRoot.GetSection("Logging"));
            loggerFactory.AddDebug();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/AppException");
            }

            app.UseStaticFiles();
            app.UseSession();

            // obsolete =>  app.UseIdentity();
            app.UseAuthentication();

            
            //app.UseMvcWithDefaultRoute();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "categoryfilter",
                    template: "Album/List/{category?}",
                    defaults: new { Controller = "Album", action = "List" });


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


            });

        }
    }
}
