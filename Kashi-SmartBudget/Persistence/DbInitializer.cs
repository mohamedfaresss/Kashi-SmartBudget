using Kashi.Domain;
using Kashi_SmartBudget.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kashi_SmartBudget.Persistence
{
    public static class DbInitializer
    {

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roles = new[] { "Admin", "User" };
            foreach (var r in roles)
                if (!await roleManager.RoleExistsAsync(r)) 
            await roleManager.CreateAsync(new IdentityRole(r));
             
            var adminEmail = "admin@kashi.local";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if(admin == null)
            {
                admin=new ApplicationUser {UserName="admin",Email=adminEmail};
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");

            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new[]
                {
                    new Category { Name = "Food" },
                    new Category { Name = "Transport" },
                    new Category { Name = "Rent" }
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
