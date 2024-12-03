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
    [Authorize(Policy = "EmployeeOrManager")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public EmployeeFormViewModel Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.DepartmentEmployees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            Employee = new EmployeeFormViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailAddress = employee.EmailAddress,
                TelephoneNumber = employee.TelephoneNumber,
                ManagerId = employee.ManagerId,
                Status = employee.Status,
                SelectedDepartments = employee.DepartmentEmployees.Select(de => de.DepartmentId).ToList()
            };

            var currentUser = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("HRAdmin") && currentUser.Email != employee.EmailAddress)
            {
                return Forbid();
            }

            await LoadFormData();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadFormData();
                return Page();
            }

            var employee = await _context.Employees
                .Include(e => e.DepartmentEmployees)
                .FirstOrDefaultAsync(e => e.Id == Employee.Id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = Employee.FirstName;
            employee.LastName = Employee.LastName;
            employee.EmailAddress = Employee.EmailAddress;
            employee.TelephoneNumber = Employee.TelephoneNumber;
            employee.ManagerId = Employee.ManagerId;

            if (User.IsInRole("HRAdmin"))
            {
                employee.Status = Employee.Status;
            }

            // Update departments
            _context.DepartmentEmployees.RemoveRange(employee.DepartmentEmployees);

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
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        private async Task LoadFormData()
        {
            Employee.AvailableManagers = await _context.Employees
                .Where(e => e.Status == Status.Active && e.Id != Employee.Id)
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
