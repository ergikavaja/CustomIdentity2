using CustomIdentity2.Data;
using CustomIdentity2.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentity2
{
    public class Seeder
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider, 
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            AppDbContext dbContext)
        {
            var roleNames = new[] { "Admin", "User" };

            // Create the roles if they don't exist
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create the default admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
            if (adminUser == null)
            {
                var user = new AppUser { UserName = "admin@admin.com", Email = "admin@admin.com", Name = "" };
                var result = await userManager.CreateAsync(user, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            // Note: ndrryshimi i emailit dhe pasw
            //else
            //{
            //    adminUser.Name = "Eris";
            //    adminUser.UserName = "eris@durres2001.com";
            //    adminUser.Email = "eris@durres2001.com";

            //    await dbContext.SaveChangesAsync();

            //    await userManager.ChangePasswordAsync(adminUser, "newPassword", "newPassword");
            //}
        }
    }
}
