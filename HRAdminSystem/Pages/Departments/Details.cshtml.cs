using HRAdminSystem.Data;
using HRAdminSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HRAdminSystem.Pages.Departments
{
    [Authorize(Policy = "HRAdminOnly")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public DepartmentDetailsViewModel Department { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.DepartmentEmployees)
                    .ThenInclude(de => de.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            Department = new DepartmentDetailsViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Status = department.Status,
                Employees = department.DepartmentEmployees
                    .Select(de => $"{de.Employee.FirstName} {de.Employee.LastName}")
                    .ToList()
            };

            return Page();
        }
    }
}
