using HRAdminSystem.Data;
using HRAdminSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRAdminSystem.Pages.Departments
{
    [Authorize(Policy = "HRAdminOnly")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DepartmentFormViewModel Department { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            Department = new DepartmentFormViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Status = department.Status
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var department = await _context.Departments.FindAsync(Department.Id);

            if (department == null)
            {
                return NotFound();
            }

            department.Name = Department.Name;
            department.Status = Department.Status;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
