using Microsoft.AspNetCore.Identity;

namespace HRAdminSystem.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            System.Diagnostics.Debug.WriteLine("Starting database initialization");

            context.Database.EnsureCreated();

            var existingUser = await userManager.FindByEmailAsync("hradmin@test.com");
            System.Diagnostics.Debug.WriteLine($"Existing user check: {existingUser?.Email ?? "not found"}");
            if (existingUser != null)
            {
                var roles = await userManager.GetRolesAsync(existingUser);
                System.Diagnostics.Debug.WriteLine($"Existing user roles: {string.Join(", ", roles)}");
                var passwordValid = await userManager.CheckPasswordAsync(existingUser, "TestPass1234!");
                System.Diagnostics.Debug.WriteLine($"Password valid: {passwordValid}");
            }



            if (!await roleManager.RoleExistsAsync("HRAdmin"))
            {
                System.Diagnostics.Debug.WriteLine("Creating HRAdmin role");
                await roleManager.CreateAsync(new IdentityRole("HRAdmin"));
            }

            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                System.Diagnostics.Debug.WriteLine("Creating Employee role");
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

                if (await userManager.FindByEmailAsync("hradmin@test.com") == null)
            {
                System.Diagnostics.Debug.WriteLine("Creating HR Admin user");
                var user = new IdentityUser
                {
                    UserName = "hradmin@test.com",
                    Email = "hradmin@test.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "TestPass1234!");
                if (result.Succeeded)
                {
                    System.Diagnostics.Debug.WriteLine("HR Admin user created successfully");
                    await userManager.AddToRoleAsync(user, "HRAdmin");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to create HR Admin user: {string.Join(", ", result.Errors)}");
                }
            }
        }
    }
}
