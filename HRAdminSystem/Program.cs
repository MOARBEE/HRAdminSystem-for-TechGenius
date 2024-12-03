using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HRAdminSystem.Data;
using HRAdminSystem.Models;
using Microsoft.AspNetCore.Authorization;
using HRAdminSystem.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Employees");
    options.Conventions.AuthorizeFolder("/Departments");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HRAdminOnly", policy =>
        policy.RequireRole("HRAdmin"));
    options.AddPolicy("EmployeeOrManager", policy =>
        policy.RequireRole("Employee", "Manager", "HRAdmin"));
});

builder.Services.AddScoped<IAuthorizationHandler, HRAdminHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.Initialize(services);

    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var user = await userManager.FindByEmailAsync("hradmin@test.com");
    System.Diagnostics.Debug.WriteLine($"HR Admin user exists: {user != null}");
    if (user != null)
    {
        var roles = await userManager.GetRolesAsync(user);
        System.Diagnostics.Debug.WriteLine($"HR Admin roles: {string.Join(", ", roles)}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
