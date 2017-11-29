using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using MusicStore.Auth;
using MusicStore.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MusicStore
{
    // https://github.com/aspnet/Docs/blob/master/aspnetcore/tutorials/first-mvc-app/working-with-sql.md#aspnet-core-2x
    // Add the seed initializer to the Main method in the Program.cs file:


    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);


            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var env = services.GetRequiredService<IHostingEnvironment>();
                
                try
                {
                    // create user admin with Administrator role to get into admin menu

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    if (!userManager.Users.Select(user => user.UserName).Contains("admin"))
                    {
                        var adminUser = new ApplicationUser() {
                            UserName = "admin",
                            Email = "admin@test.com",
                            Birthdate = new DateTime(1990,12,31)};
                        userManager.CreateAsync(adminUser, "Admin12345!").Wait();
                        if (!roleManager.Roles.Select(role => role.Name).Contains("Administrators"))
                        {
                            roleManager.CreateAsync(new IdentityRole("Administrators")).Wait();
                        }
                        var taskAddRole = userManager.AddToRoleAsync(adminUser, "Administrators");

                        var taskBirthDayClaim = userManager.AddClaimAsync(adminUser, new Claim(ClaimTypes.DateOfBirth, "1990-12-31"));
                        var taskClaimDeleteAlbum = userManager.AddClaimAsync(adminUser, new Claim("Delete Album", "Delete Album"));
                        taskAddRole.Wait();
                        taskBirthDayClaim.Wait();
                        taskClaimDeleteAlbum.Wait();
                    }
                   

                    if (env.IsDevelopment())
                    {
                        //DbInitializer.Initialize(services);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
