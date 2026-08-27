using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagement.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // =====================================================
            // ROLES
            // =====================================================

            string[] roles =
            {
                "Admin",
                "Lagerleiter",
                "Mitarbeiter"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Role '{role}' could not be created: " +
                            string.Join(", ",
                                result.Errors.Select(e => e.Description)));
                    }
                }
            }


            // =====================================================
            // ADMIN USER
            // =====================================================

            const string adminEmail = "admin@email.com";
            const string adminPassword = "Hallo123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Admin user could not be created: " +
                        string.Join(", ",
                            result.Errors.Select(e => e.Description)));
                }
            }


            // =====================================================
            // ADMIN ROLE
            // =====================================================

            if (!await userManager.IsInRoleAsync(
                adminUser,
                "Admin"))
            {
                var result = await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Admin role could not be assigned: " +
                        string.Join(", ",
                            result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}