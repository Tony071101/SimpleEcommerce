using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleEcommerce.Data;
using SimpleEcommerce.Models.Entities;

namespace SimpleEcommerce.Utilities
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Migrate the database if not already done
                await dbContext.Database.MigrateAsync();

                // Seed Roles
                string[] roleNames = { "Admin", "Customer" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                        Console.WriteLine($"Role '{roleName}' created.");
                    }
                }

                // Seed Admin User
                var adminUser = await userManager.FindByEmailAsync("admin@example.com");
                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                        PhoneNumber = "123456789",
                        CreatedDate = DateTime.UtcNow
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin@123"); // Consider using environment variable for admin password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        Console.WriteLine("Admin user created and assigned 'Admin' role.");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        Console.WriteLine($"Failed to create admin user: {errors}");
                    }
                }
            }
        }
    }
}