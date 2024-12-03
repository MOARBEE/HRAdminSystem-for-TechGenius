using HRAdminSystem.Data;
using HRAdminSystem.Enums;
using HRAdminSystem.Models;
using HRAdminSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRAdminSystem.Pages.Employees
{
    [Authorize(Policy = "HRAdminOnly")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public EmployeeFormViewModel Employee { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadFormData();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Validation error: {error.ErrorMessage}");
                    }
                }
                await LoadFormData();
                return Page();
            }

            var employee = new Employee
            {
                FirstName = Employee.FirstName,
                LastName = Employee.LastName,
                EmailAddress = Employee.EmailAddress,
                TelephoneNumber = Employee.TelephoneNumber,
                ManagerId = Employee.ManagerId,
                Status = Status.Active
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Create user account for the employee
            var user = new IdentityUser
            {
                UserName = Employee.EmailAddress,
                Email = Employee.EmailAddress,
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(user, "Password123#");
            await _userManager.AddToRoleAsync(user, "Employee");

            // Add department associations
            if (Employee.SelectedDepartments != null)
            {
                foreach (var deptId in Employee.SelectedDepartments)
                {
                    _context.DepartmentEmployees.Add(new DepartmentEmployee
                    {
                        EmployeeId = employee.Id,
                        DepartmentId = deptId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private async Task LoadFormData()
        {
            Employee = Employee ?? new EmployeeFormViewModel();

            Employee.AvailableManagers = await _context.Employees
                .Where(e => e.Status == Status.Active)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = $"{e.FirstName} {e.LastName}"
                })
                .ToListAsync();

            Employee.AvailableDepartments = await _context.Departments
                .Where(d => d.Status == Status.Active)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToListAsync();
        }
    }
}
