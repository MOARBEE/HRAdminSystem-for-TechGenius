using HRAdminSystem.Data;
using HRAdminSystem.Enums;
using HRAdminSystem.Models;
using HRAdminSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRAdminSystem.Pages.Departments
{
    [Authorize(Policy = "HRAdminOnly")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DepartmentFormViewModel Department { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var department = new Department
            {
                Name = Department.Name,
                Status = Status.Active
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
