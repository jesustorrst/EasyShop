using EasyShop.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EasyShop.Identity.Infrastructure.Persistence;

public static class AppIdentityDbContextSeed
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Administrator"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Administrator" });
        }

        if (!await roleManager.RoleExistsAsync("Client"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Client" });
        }

        var adminEmail = "admin@easyshop.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "System",
                DateOfBirth = new DateTime(1990, 1, 1),
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "AdminWsx1!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator");
            }
        }

        var clientEmail = "client@easyshop.com";
        if (await userManager.FindByEmailAsync(clientEmail) == null)
        {
            var clientUser = new ApplicationUser
            {
                UserName = "client",
                Email = clientEmail,
                FirstName = "Juan",
                LastName = "Perez",
                DateOfBirth = new DateTime(1995, 5, 15),
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(clientUser, "ClientWsx1!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(clientUser, "Client");
            }
        }
    }
}