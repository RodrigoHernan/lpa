using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace AspNetCoreTodo.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        private static async Task EnsureRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager
                .RoleExistsAsync(Role.Admin);
            
            if (alreadyExists) return;

            await roleManager.CreateAsync(
                new IdentityRole(Role.Admin));
        }

        private static async Task EnsureTestAdminAsync(
            UserManager<ApplicationUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();

            var yo = await userManager.Users
                .Where(x => x.UserName == "rodrigohernan.ramos2@gmail.com")
                .SingleOrDefaultAsync();
            // await userManager.AddToRoleAsync(
            //     yo, Role.Admin);

            if (testAdmin != null) return;

            testAdmin = new ApplicationUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };
            await userManager.CreateAsync(
                testAdmin, "NotSecure123!!");
            await userManager.AddToRoleAsync(
                testAdmin, Role.Admin);
        }

    }
}
