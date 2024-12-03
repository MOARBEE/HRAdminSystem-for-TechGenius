using HRAdminSystem.Data;
using HRAdminSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HRAdminSystem.Pages.Employees
{
    [Authorize(Policy = "HRAdminOnly")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public EmployeeDetailsViewModel Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Manager)
                .Include(e => e.DepartmentEmployees)
                    .ThenInclude(de => de.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            Employee = new EmployeeDetailsViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailAddress = employee.EmailAddress,
                TelephoneNumber = employee.TelephoneNumber,
                ManagerName = employee.Manager != null ? $"{employee.Manager.FirstName} {employee.Manager.LastName}" : "None",
                Status = employee.Status,
                Departments = employee.DepartmentEmployees.Select(de => de.Department.Name).ToList()
            };

            return Page();
        }
    }
}
